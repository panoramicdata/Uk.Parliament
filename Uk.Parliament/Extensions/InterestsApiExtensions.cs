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
	/// Get all interests by automatically paginating through results
	/// </summary>
	/// <param name="api">The interests API</param>
	/// <param name="memberId">Optional member filter</param>
	/// <param name="categoryId">Optional category filter</param>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all interests</returns>
	public static async IAsyncEnumerable<Interest> GetAllInterestsAsync(
		this IInterestsApi api,
		int? memberId = null,
		int? categoryId = null,
		string? searchTerm = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (true)
		{
			var response = await api.SearchInterestsAsync(memberId, categoryId, searchTerm, skip, pageSize, cancellationToken);

			if (response?.Items is null || response.Items.Count == 0)
			{
				yield break;
			}

			foreach (var interest in response.Items)
			{
				yield return interest;
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
	/// Get all interests as a materialized list
	/// </summary>
	/// <param name="api">The interests API</param>
	/// <param name="memberId">Optional member filter</param>
	/// <param name="categoryId">Optional category filter</param>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all interests</returns>
	public static async Task<List<Interest>> GetAllInterestsListAsync(
		this IInterestsApi api,
		int? memberId = null,
		int? categoryId = null,
		string? searchTerm = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allInterests = new List<Interest>();

		await foreach (var interest in api.GetAllInterestsAsync(memberId, categoryId, searchTerm, pageSize, cancellationToken))
		{
			allInterests.Add(interest);
		}

		return allInterests;
	}
}
