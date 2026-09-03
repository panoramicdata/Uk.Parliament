using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Petitions API client using Refit
/// </summary>
public interface IPetitionsApi
{
	/// <summary>
	/// Get petitions with optional filtering and pagination
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>API response containing list of petitions</returns>
	[Get("/petitions.json")]
	Task<ParliamentApiResponse<List<Petition>>> GetAsync(
		[Query] GetPetitionsRequest request,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get a single petition by ID
	/// </summary>
	/// <param name="id">Petition ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>API response containing the petition</returns>
	[Get("/petitions/{id}.json")]
	Task<ParliamentApiResponse<Petition>> GetByIdAsync(
		int id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get archived petitions with optional filtering and pagination
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	[Get("/archived/petitions.json")]
	Task<ParliamentApiResponse<List<Petition>>> GetArchivedAsync(
		[Query] GetPetitionsRequest request,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get a single archived petition by ID
	/// </summary>
	/// <param name="id">Petition identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	[Get("/archived/petitions/{id}.json")]
	Task<ParliamentApiResponse<Petition>> GetArchivedByIdAsync(
		int id,
		CancellationToken cancellationToken);
}
