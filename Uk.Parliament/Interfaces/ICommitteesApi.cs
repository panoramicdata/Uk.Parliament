using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models.Committees;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Committees API client using Refit
/// </summary>
public interface ICommitteesApi
{
	/// <summary>
	/// Get list of committees
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of committees</returns>
	[Get("/api/Committees")]
	Task<CommitteesListResponse<Committee>> GetCommitteesAsync(
		[Query] GetCommitteesRequest request,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get a specific committee by ID
	/// </summary>
	/// <param name="id">Committee identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Committee details</returns>
	[Get("/api/Committees/{id}")]
	Task<Committee> GetCommitteeByIdAsync(
		int id,
		CancellationToken cancellationToken);
}
