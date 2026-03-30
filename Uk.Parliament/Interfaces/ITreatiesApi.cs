#pragma warning disable CS1572, CS1573
using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Treaties;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Treaties API client using Refit
/// </summary>
/// <remarks>
/// Provides access to international treaties laid before Parliament
/// </remarks>
public interface ITreatiesApi
{
	/// <summary>
	/// Get treaties with optional filtering
	/// </summary>
	/// <param name="governmentOrganisationId">Filter by government organization</param>
	/// <param name="house">Filter by house (Commons/Lords/Both)</param>
	/// <param name="status">Filter by treaty status</param>
	/// <param name="dateLaidFrom">Filter by laid date from</param>
	/// <param name="dateLaidTo">Filter by laid date to</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of treaties</returns>
	[Get("/api/Treaty")]
	Task<PaginatedResponse<Treaty>> GetTreatiesAsync(
		[Query] GetTreatiesRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get treaties with optional filtering.
	/// </summary>
	/// <remarks>
	/// Use <see cref="GetTreatiesAsync(GetTreatiesRequest, CancellationToken)"/> instead.
	/// Example: <c>GetTreatiesAsync(new GetTreatiesRequest { GovernmentOrganisationId = governmentOrganisationId, House = house, Status = status, DateLaidFrom = dateLaidFrom, DateLaidTo = dateLaidTo, Skip = skip, Take = take }, cancellationToken)</c>.
	/// </remarks>
	[Obsolete("Use GetTreatiesAsync(GetTreatiesRequest request, CancellationToken cancellationToken) instead. Example: GetTreatiesAsync(new GetTreatiesRequest { GovernmentOrganisationId = governmentOrganisationId, House = house, Status = status, DateLaidFrom = dateLaidFrom, DateLaidTo = dateLaidTo, Skip = skip, Take = take }, cancellationToken).", true)]
	[Get("/api/Treaty")]
	Task<PaginatedResponse<Treaty>> GetTreatiesAsync(
		[Query] int? governmentOrganisationId = null,
		[Query] string? house = null,
		[Query] string? status = null,
		[Query] DateTime? dateLaidFrom = null,
		[Query] DateTime? dateLaidTo = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific treaty by ID
	/// </summary>
	/// <param name="id">Treaty identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Treaty details</returns>
	[Get("/api/Treaty/{id}")]
	Task<Treaty> GetTreatyByIdAsync(
		string id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get business items for a specific treaty
	/// </summary>
	/// <param name="treatyId">Treaty identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of business items related to the treaty</returns>
	[Get("/api/Treaty/{treatyId}/BusinessItem")]
	Task<List<TreatyBusinessItem>> GetTreatyBusinessItemsAsync(
		string treatyId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific business item by ID
	/// </summary>
	/// <param name="id">Business item identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Business item details</returns>
	[Get("/api/BusinessItem/{id}")]
	Task<TreatyBusinessItem> GetBusinessItemByIdAsync(
		string id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get series memberships
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of series memberships</returns>
	[Get("/api/SeriesMembership")]
	Task<object> GetSeriesMembershipsAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all government organizations
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of government organizations</returns>
	[Get("/api/GovernmentOrganisation")]
	Task<PaginatedResponse<GovernmentOrganisation>> GetGovernmentOrganisationsAsync(
		CancellationToken cancellationToken = default);
}
#pragma warning restore CS1572, CS1573
