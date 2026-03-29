using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Interests;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IInterestsApi to provide additional functionality
/// </summary>
public static class InterestsApiExtensions
{
	/// <summary>
	/// Get all interests by automatically paginating through results.
	/// </summary>
	/// <param name="api">The interests API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all interests</returns>
	public static IAsyncEnumerable<Interest> GetAllInterestsAsync(
		this IInterestsApi api,
		SearchInterestsRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new SearchInterestsRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.SearchInterestsAsync(pageRequest, token),
			static response => response.Items,
			static response => response.TotalResults,
			cancellationToken);
	}

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
    public static IAsyncEnumerable<Interest> GetAllInterestsAsync(
		this IInterestsApi api,
		int? memberId = null,
		int? categoryId = null,
		string? searchTerm = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllInterestsAsync(
			new SearchInterestsRequest
			{
				MemberId = memberId,
				CategoryId = categoryId,
				SearchTerm = searchTerm,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all interests as a materialized list.
	/// </summary>
	/// <param name="api">The interests API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all interests</returns>
	public static Task<List<Interest>> GetAllInterestsListAsync(
		this IInterestsApi api,
		SearchInterestsRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllInterestsAsync(request, cancellationToken), cancellationToken);

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
  public static Task<List<Interest>> GetAllInterestsListAsync(
		this IInterestsApi api,
		int? memberId = null,
		int? categoryId = null,
		string? searchTerm = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(
			api.GetAllInterestsAsync(memberId, categoryId, searchTerm, pageSize, cancellationToken),
			cancellationToken);
}
