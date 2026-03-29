using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Uk.Parliament.Extensions;

internal static class PaginationHelper
{
  public static async IAsyncEnumerable<TItem> GetAllOffsetAsync<TRequest, TResponse, TItem>(
		TRequest request,
		int pageSize,
		Func<TRequest, int, int, TRequest> withPagination,
      Func<TRequest, CancellationToken, Task<TResponse>> fetchPage,
		Func<TResponse, IReadOnlyList<TItem>?> getItems,
		Func<TResponse, int> getTotalResults,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (!cancellationToken.IsCancellationRequested)
		{
			var pageRequest = withPagination(request, skip, pageSize);
			var response = await fetchPage(pageRequest, cancellationToken);
			var items = response is null ? null : getItems(response);

			if (items is null || items.Count == 0)
			{
				yield break;
			}

			foreach (var item in items)
			{
				yield return item;
			}

         if (items.Count < pageSize || skip + pageSize >= getTotalResults(response!))
			{
				yield break;
			}

			skip += pageSize;
		}
	}

    public static async IAsyncEnumerable<TItem> GetAllPageAsync<TRequest, TResponse, TItem>(
		TRequest request,
		int pageSize,
		Func<TRequest, int, int, TRequest> withPagination,
      Func<TRequest, CancellationToken, Task<TResponse>> fetchPage,
		Func<TResponse, IReadOnlyList<TItem>?> getItems,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var page = 1;

		while (!cancellationToken.IsCancellationRequested)
		{
			var pageRequest = withPagination(request, page, pageSize);
			var response = await fetchPage(pageRequest, cancellationToken);
			var items = response is null ? null : getItems(response);

			if (items is null || items.Count == 0)
			{
				yield break;
			}

			foreach (var item in items)
			{
				yield return item;
			}

			if (items.Count < pageSize)
			{
				yield break;
			}

			page++;
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
