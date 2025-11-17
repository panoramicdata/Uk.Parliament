using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Petitions API client using Refit
/// </summary>
public interface IPetitionsApi
{
	/// <summary>
	/// Get petitions with optional filtering and pagination
	/// </summary>
	/// <param name="search">Optional search term to filter petitions</param>
	/// <param name="state">Optional state filter (e.g., "open", "closed", "rejected")</param>
	/// <param name="page">Optional page number for pagination (1-based)</param>
	/// <param name="pageSize">Optional number of results per page</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>API response containing list of petitions</returns>
	[Get("/petitions.json")]
	Task<ParliamentApiResponse<List<Petition>>> GetAsync(
		[Query] string? search = null,
		[Query] string? state = null,
		[Query] int? page = null,
		[AliasAs("page_size")] int? pageSize = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a single petition by ID
	/// </summary>
	/// <param name="id">Petition ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>API response containing the petition</returns>
	[Get("/petitions/{id}.json")]
	Task<ParliamentApiResponse<Petition>> GetByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get archived petitions with optional filtering and pagination
	/// </summary>
	[Get("/archived/petitions.json")]
	Task<ParliamentApiResponse<List<Petition>>> GetArchivedAsync(
		[Query] string? search = null,
		[Query] string? state = null,
		[Query] int? page = null,
		[AliasAs("page_size")] int? pageSize = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a single archived petition by ID
	/// </summary>
	[Get("/archived/petitions/{id}.json")]
	Task<ParliamentApiResponse<Petition>> GetArchivedByIdAsync(
		int id,
		CancellationToken cancellationToken = default);
}
