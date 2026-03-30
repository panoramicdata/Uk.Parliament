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

	/// <summary>
	/// Search for index terms
	/// </summary>
	/// <param name="searchTerm">Search term</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Search response with results</returns>
	[Get("/api/Search/IndexTermSearchResults/{searchTerm}")]
	Task<ErskineMaySearchResponse> SearchIndexTermsAsync(
		string searchTerm,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a paragraph by reference
	/// </summary>
	/// <param name="reference">Paragraph reference</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Search response with results</returns>
	[Get("/api/Search/Paragraph/{reference}")]
	Task<ErskineMaySearchResponse> GetParagraphByReferenceAsync(
		string reference,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get an index term by ID
	/// </summary>
	/// <param name="indexTermId">Index term identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Index term details</returns>
	[Get("/api/IndexTerm/{indexTermId}")]
	Task<object> GetIndexTermByIdAsync(
		int indexTermId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Browse index terms
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of index terms</returns>
	[Get("/api/IndexTerm/browse")]
	Task<object> BrowseIndexTermsAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a section with a specific step
	/// </summary>
	/// <param name="sectionId">Section identifier</param>
	/// <param name="step">Step value</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Section details</returns>
	[Get("/api/Section/{sectionId},{step}")]
	Task<ErskineMaySection> GetSectionWithStepAsync(
		int sectionId,
		int step,
		CancellationToken cancellationToken = default);
}
