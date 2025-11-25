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
	/// Search as an async enumerable
	/// Note: The Erskine May API returns all results at once, this method just wraps it for consistency
	/// </summary>
	/// <param name="api">The Erskine May API</param>
	/// <param name="searchTerm">Search term</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of search results</returns>
	public static async IAsyncEnumerable<ErskineMaySearchResult> SearchAllAsync(
		this IErskineMayApi api,
		string searchTerm,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var results = await api.SearchAsync(searchTerm, cancellationToken);

		foreach (var result in results)
		{
			yield return result;
		}
	}
}
