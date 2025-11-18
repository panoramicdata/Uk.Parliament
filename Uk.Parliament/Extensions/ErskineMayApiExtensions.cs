using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.ErskineMay;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IErskineMayApi to provide additional functionality
/// </summary>
public static class ErskineMayApiExtensions
{
	/// <summary>
	/// Get all sections for a chapter by automatically paginating through all results
	/// </summary>
	/// <param name="api">The Erskine May API</param>
	/// <param name="chapterNumber">Chapter number</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all sections</returns>
	public static async IAsyncEnumerable<ErskineMaySection> GetAllSectionsAsync(
		this IErskineMayApi api,
		int chapterNumber,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (true)
		{
			var response = await api.GetSectionsAsync(chapterNumber, skip, pageSize, cancellationToken);

			if (response?.Items is null || response.Items.Count == 0)
			{
				yield break;
			}

			foreach (var item in response.Items)
			{
				yield return item.Value;
			}

			// Stop if this was the last page
			if (response.Items.Count < pageSize || skip + pageSize >= response.TotalResults)
			{
				yield break;
			}

			skip += pageSize;
		}
	}

	/// <summary>
	/// Search with automatic pagination through all results
	/// </summary>
	/// <param name="api">The Erskine May API</param>
	/// <param name="searchTerm">Search term</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all search results</returns>
	public static async IAsyncEnumerable<ErskineMaySearchResult> SearchAllAsync(
		this IErskineMayApi api,
		string searchTerm,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (true)
		{
			var response = await api.SearchAsync(searchTerm, skip, pageSize, cancellationToken);

			if (response?.Items is null || response.Items.Count == 0)
			{
				yield break;
			}

			foreach (var item in response.Items)
			{
				yield return item.Value;
			}

			// Stop if this was the last page
			if (response.Items.Count < pageSize || skip + pageSize >= response.TotalResults)
			{
				yield break;
			}

			skip += pageSize;
		}
	}
}
