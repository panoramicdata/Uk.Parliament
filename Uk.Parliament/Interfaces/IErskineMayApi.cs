using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models;
using Uk.Parliament.Models.ErskineMay;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Erskine May API client using Refit
/// </summary>
/// <remarks>
/// Provides access to Erskine May, the authoritative guide to parliamentary procedure
/// </remarks>
public interface IErskineMayApi
{
	/// <summary>
	/// Get all parts of Erskine May
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of Erskine May parts</returns>
	[Get("/api/Parts")]
	Task<List<ErskineMayPart>> GetPartsAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get chapters for a specific part
	/// </summary>
	/// <param name="partNumber">Part number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of chapters in the part</returns>
	[Get("/api/Parts/{partNumber}/Chapters")]
	Task<List<ErskineMayChapter>> GetChaptersAsync(
		int partNumber,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get sections for a specific chapter
	/// </summary>
	/// <param name="chapterNumber">Chapter number</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of sections</returns>
	[Get("/api/Chapters/{chapterNumber}/Sections")]
	Task<PaginatedResponse<ErskineMaySection>> GetSectionsAsync(
		int chapterNumber,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific section by ID
	/// </summary>
	/// <param name="id">Section identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Section details</returns>
	[Get("/api/Sections/{id}")]
	Task<ErskineMaySection> GetSectionByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search Erskine May content
	/// </summary>
	/// <param name="searchTerm">Search term</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated search results</returns>
	[Get("/api/Search")]
	Task<PaginatedResponse<ErskineMaySearchResult>> SearchAsync(
		[Query] string searchTerm,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);
}
