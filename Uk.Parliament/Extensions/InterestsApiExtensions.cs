using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Interests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IInterestsApi to provide additional functionality
/// </summary>
public static class InterestsApiExtensions
{
	/// <summary>
	/// Get all interests as an async enumerable
	/// Note: The Interests API returns all results at once, this method just wraps it for consistency
	/// </summary>
	/// <param name="api">The interests API</param>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="categoryId">Optional category filter</param>
	/// <param name="memberId">Optional member filter</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all interests</returns>
	public static async IAsyncEnumerable<Interest> GetAllInterestsAsync(
		this IInterestsApi api,
		string? searchTerm = null,
		int? categoryId = null,
		int? memberId = null,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var interests = await api.SearchInterestsAsync(memberId, categoryId, searchTerm, cancellationToken);

		foreach (var interest in interests)
		{
			yield return interest;
		}
	}

	/// <summary>
	/// Get all interests as a materialized list
	/// </summary>
	/// <param name="api">The interests API</param>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="categoryId">Optional category filter</param>
	/// <param name="memberId">Optional member filter</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all interests</returns>
	public static Task<List<Interest>> GetAllInterestsListAsync(
		this IInterestsApi api,
		string? searchTerm = null,
		int? categoryId = null,
		int? memberId = null,
		CancellationToken cancellationToken = default)
	{
		return api.SearchInterestsAsync(memberId, categoryId, searchTerm, cancellationToken);
	}
}
