using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Members;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Members API client using Refit
/// </summary>
public interface IMembersApi
{
	/// <summary>
	/// Search for members of parliament
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of members</returns>
	[Get("/api/Members/Search")]
	Task<PaginatedResponse<Member>> SearchAsync(
		[Query] SearchMembersRequest request,
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
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of constituencies</returns>
	[Get("/api/Location/Constituency/Search")]
	Task<PaginatedResponse<Constituency>> SearchConstituenciesAsync(
		[Query] SearchConstituenciesRequest request,
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
