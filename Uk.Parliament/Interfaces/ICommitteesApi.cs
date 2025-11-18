using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models.Committees;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Committees API client using Refit
/// </summary>
public interface ICommitteesApi
{
	/// <summary>
	/// Get list of committees
	/// </summary>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of committees</returns>
	[Get("/api/Committees")]
	Task<CommitteesListResponse<Committee>> GetCommitteesAsync(
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);
}
