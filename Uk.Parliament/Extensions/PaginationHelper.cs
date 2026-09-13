using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Uk.Parliament.Extensions;

internal static class PaginationHelper
{
	/// <summary>
	/// How to fetch one page of an API and read the items back out of it.
	/// </summary>
	/// <param name="WithPagination">Produces a request for the given index and page size.</param>
	/// <param name="FetchPage">Calls the API with that request.</param>
	/// <param name="GetItems">Reads the page's items out of the response.</param>
	private sealed record PageReader<TRequest, TResponse, TItem>(
		Func<TRequest, int, int, TRequest> WithPagination,
		Func<TRequest, CancellationToken, Task<TResponse>> FetchPage,
		Func<TResponse, IReadOnlyList<TItem>?> GetItems);

	/// <summary>
	/// How to walk from one page to the next. Offset-based and page-number-based APIs differ only here.
	/// </summary>
	/// <param name="GetCurrentIndex">The skip offset or page number to request next.</param>
	/// <param name="Advance">Moves the index on by one page.</param>
	/// <param name="IsLastPage">Whether the response just read was the final page.</param>
	private sealed record PageCursor<TResponse>(
		Func<int> GetCurrentIndex,
		Action<int> Advance,
		Func<TResponse, bool> IsLastPage);

	public static IAsyncEnumerable<TItem> GetAllOffsetAsync<TRequest, TResponse, TItem>(
		TRequest request,
		int pageSize,
		Func<TRequest, int, int, TRequest> withPagination,
		Func<TRequest, CancellationToken, Task<TResponse>> fetchPage,
		Func<TResponse, IReadOnlyList<TItem>?> getItems,
		Func<TResponse, int> getTotalResults,
		CancellationToken cancellationToken = default)
	{
		// Declared inside the method body so that re-enumerating restarts from the first page.
		var skip = 0;

		return GetAllPagesAsync(
			request,
			pageSize,
			new PageReader<TRequest, TResponse, TItem>(withPagination, fetchPage, getItems),
			new PageCursor<TResponse>(
				() => skip,
				_ => skip += pageSize,
				response => skip + pageSize >= getTotalResults(response)),
			cancellationToken);
	}

	public static IAsyncEnumerable<TItem> GetAllPageAsync<TRequest, TResponse, TItem>(
		TRequest request,
		int pageSize,
		Func<TRequest, int, int, TRequest> withPagination,
		Func<TRequest, CancellationToken, Task<TResponse>> fetchPage,
		Func<TResponse, IReadOnlyList<TItem>?> getItems,
		CancellationToken cancellationToken = default)
	{
		// Declared inside the method body so that re-enumerating restarts from the first page.
		var page = 1;

		return GetAllPagesAsync(
			request,
			pageSize,
			new PageReader<TRequest, TResponse, TItem>(withPagination, fetchPage, getItems),
			// A page-number API reports no total, so a short page is the only end-of-results signal.
			new PageCursor<TResponse>(() => page, _ => page++, static _ => false),
			cancellationToken);
	}

	private static async IAsyncEnumerable<TItem> GetAllPagesAsync<TRequest, TResponse, TItem>(
		TRequest request,
		int pageSize,
		PageReader<TRequest, TResponse, TItem> reader,
		PageCursor<TResponse> cursor,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			var pageRequest = reader.WithPagination(request, cursor.GetCurrentIndex(), pageSize);
			var response = await reader.FetchPage(pageRequest, cancellationToken);
			var items = response is null ? null : reader.GetItems(response);

			if (items is null || items.Count == 0)
			{
				yield break;
			}

			foreach (var item in items)
			{
				yield return item;
			}

			if (items.Count < pageSize || cursor.IsLastPage(response!))
			{
				yield break;
			}

			cursor.Advance(pageSize);
		}
	}

	public static async Task<List<TItem>> ToListAsync<TItem>(
		IAsyncEnumerable<TItem> source,
		CancellationToken cancellationToken = default)
	{
		var items = new List<TItem>();

		await foreach (var item in source.WithCancellation(cancellationToken))
		{
			items.Add(item);
		}

		return items;
	}
}
