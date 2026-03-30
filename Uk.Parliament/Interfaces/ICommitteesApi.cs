#pragma warning disable CS1572, CS1573
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
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of committees</returns>
	[Get("/api/Committees")]
	Task<CommitteesListResponse<Committee>> GetCommitteesAsync(
		[Query] GetCommitteesRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get list of committees.
	/// </summary>
	/// <remarks>
	/// Use <see cref="GetCommitteesAsync(GetCommitteesRequest, CancellationToken)"/> instead.
	/// Example: <c>GetCommitteesAsync(new GetCommitteesRequest { Skip = skip, Take = take }, cancellationToken)</c>.
	/// </remarks>
	[Obsolete("Use GetCommitteesAsync(GetCommitteesRequest request, CancellationToken cancellationToken) instead. Example: GetCommitteesAsync(new GetCommitteesRequest { Skip = skip, Take = take }, cancellationToken).", true)]
	[Get("/api/Committees")]
	Task<CommitteesListResponse<Committee>> GetCommitteesAsync(
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific committee by ID
	/// </summary>
	/// <param name="id">Committee identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Committee details</returns>
	[Get("/api/Committees/{id}")]
	Task<Committee> GetCommitteeByIdAsync(
		int id,
		CancellationToken cancellationToken = default);
}
#pragma warning restore CS1572, CS1573
