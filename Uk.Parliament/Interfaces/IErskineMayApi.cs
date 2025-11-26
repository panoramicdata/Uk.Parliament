using System.Threading;
using System.Threading.Tasks;
using Refit;
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
	[Get("/api/Part")]
	Task<List<ErskineMayPart>> GetPartsAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific part by number
	/// </summary>
	/// <param name="partNumber">Part number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Part details</returns>
	[Get("/api/Part/{partNumber}")]
	Task<ErskineMayPart> GetPartAsync(
		int partNumber,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific chapter by number
	/// </summary>
	/// <param name="chapterNumber">Chapter number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Chapter details</returns>
	[Get("/api/Chapter/{chapterNumber}")]
	Task<ErskineMayChapter> GetChapterAsync(
		int chapterNumber,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific section by ID
	/// </summary>
	/// <param name="sectionId">Section identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Section details</returns>
	[Get("/api/Section/{sectionId}")]
	Task<ErskineMaySection> GetSectionByIdAsync(
		int sectionId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search for sections
	/// </summary>
	/// <param name="searchTerm">Search term</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Search response with results</returns>
	[Get("/api/Search/SectionSearchResults/{searchTerm}")]
	Task<ErskineMaySearchResponse> SearchAsync(
		string searchTerm,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search for paragraphs
	/// </summary>
	/// <param name="searchTerm">Search term</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Search response with results</returns>
	[Get("/api/Search/ParagraphSearchResults/{searchTerm}")]
	Task<ErskineMaySearchResponse> SearchParagraphsAsync(
		string searchTerm,
		CancellationToken cancellationToken = default);
}
