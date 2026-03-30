#pragma warning disable CS1572, CS1573
using System.Net.Http;
using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models.Interests;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Member Interests API client using Refit
/// </summary>
/// <remarks>
/// Provides access to the Register of Members' Financial Interests
/// </remarks>
public interface IInterestsApi
{
	/// <summary>
	/// Get all interest categories
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of interest categories</returns>
	[Get("/api/v1/Categories")]
	Task<InterestsResponse<InterestCategory>> GetCategoriesAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific category by ID
	/// </summary>
	/// <param name="id">Category identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Interest category details</returns>
	[Get("/api/v1/Categories/{id}")]
	Task<InterestCategory> GetCategoryByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search interests across all members
	/// </summary>
	/// <param name="memberId">Optional member filter</param>
	/// <param name="categoryId">Optional category filter</param>
	/// <param name="searchTerm">Search term to filter interests</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of interests</returns>
	[Get("/api/v1/Interests")]
	Task<InterestsResponse<Interest>> SearchInterestsAsync(
		[Query] SearchInterestsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search interests across all members.
	/// </summary>
	/// <remarks>
	/// Use <see cref="SearchInterestsAsync(SearchInterestsRequest, CancellationToken)"/> instead.
	/// Example: <c>SearchInterestsAsync(new SearchInterestsRequest { MemberId = memberId, CategoryId = categoryId, SearchTerm = searchTerm, Skip = skip, Take = take }, cancellationToken)</c>.
	/// </remarks>
	[Obsolete("Use SearchInterestsAsync(SearchInterestsRequest request, CancellationToken cancellationToken) instead. Example: SearchInterestsAsync(new SearchInterestsRequest { MemberId = memberId, CategoryId = categoryId, SearchTerm = searchTerm, Skip = skip, Take = take }, cancellationToken).", true)]
	[Get("/api/v1/Interests")]
	Task<InterestsResponse<Interest>> SearchInterestsAsync(
		[Query] int? memberId = null,
		[Query] int? categoryId = null,
		[Query] string? searchTerm = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific interest by ID
	/// </summary>
	/// <param name="id">Interest identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Interest details</returns>
	[Get("/api/v1/Interests/{id}")]
	Task<Interest> GetInterestByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all registers
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of registers</returns>
	[Get("/api/v1/Registers")]
	Task<List<InterestRegister>> GetRegistersAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific register by ID
	/// </summary>
	/// <param name="id">Register identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Register details</returns>
	[Get("/api/v1/Registers/{id}")]
	Task<InterestRegister> GetRegisterByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a register document by register ID
	/// </summary>
	/// <param name="id">Register identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Register document as stream</returns>
	[Get("/api/v1/Registers/{id}/document")]
	Task<HttpResponseMessage> GetRegisterDocumentAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Export interests as CSV
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>CSV data as stream</returns>
	[Get("/api/v1/Interests/csv")]
	Task<HttpResponseMessage> ExportInterestsCsvAsync(
		CancellationToken cancellationToken = default);
}
#pragma warning restore CS1572, CS1573
