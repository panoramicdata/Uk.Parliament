using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Members;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Members API client using Refit
/// </summary>
public interface IMembersApi
{
	/// <summary>
	/// Search for members of parliament
	/// </summary>
	/// <param name="name">Optional name to search for</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="house">House filter (1 = Commons, 2 = Lords)</param>
	/// <param name="isCurrentMember">Filter for current members only</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of members</returns>
	[Get("/api/Members/Search")]
	Task<PaginatedResponse<Member>> SearchAsync(
		[Query] string? name = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		[Query] int? house = null,
		[Query] bool? isCurrentMember = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific member by ID
	/// </summary>
	/// <param name="id">Member ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Member details wrapped in ValueWrapper</returns>
	[Get("/api/Members/{id}")]
	Task<ValueWrapper<Member>> GetByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search for constituencies
	/// </summary>
	/// <param name="searchText">Search text</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of constituencies</returns>
	[Get("/api/Location/Constituency/Search")]
	Task<PaginatedResponse<Constituency>> SearchConstituenciesAsync(
		[Query] string? searchText = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific constituency by ID
	/// </summary>
	/// <param name="id">Constituency ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Constituency details wrapped in ValueWrapper</returns>
	[Get("/api/Location/Constituency/{id}")]
	Task<ValueWrapper<Constituency>> GetConstituencyByIdAsync(
		int id,
		CancellationToken cancellationToken = default);
}
