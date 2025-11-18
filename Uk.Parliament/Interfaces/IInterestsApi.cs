using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Interests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Member Interests API client using Refit
/// </summary>
/// <remarks>
/// Provides access to the Register of Members' Financial Interests
/// </remarks>
public interface IInterestsApi
{
	/// <summary>
	/// Get all interests for a specific member
	/// </summary>
	/// <param name="memberId">Member identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Member's registered interests grouped by category</returns>
	[Get("/api/Interests/Member/{memberId}")]
	Task<MemberInterests> GetMemberInterestsAsync(
		int memberId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all interest categories
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of interest categories</returns>
	[Get("/api/Interests/Category")]
	Task<List<InterestCategory>> GetCategoriesAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search interests across all members
	/// </summary>
	/// <param name="searchTerm">Search term to filter interests</param>
	/// <param name="categoryId">Optional category filter</param>
	/// <param name="memberId">Optional member filter</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of interests</returns>
	[Get("/api/Interests/Search")]
	Task<PaginatedResponse<Interest>> SearchInterestsAsync(
		[Query] string? searchTerm = null,
		[Query] int? categoryId = null,
		[Query] int? memberId = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);
}
