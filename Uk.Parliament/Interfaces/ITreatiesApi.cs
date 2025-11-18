using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Treaties;

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
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get business items for a specific treaty
	/// </summary>
	/// <param name="treatyId">Treaty identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of business items related to the treaty</returns>
	[Get("/api/Treaty/{treatyId}/BusinessItem")]
	Task<List<TreatyBusinessItem>> GetTreatyBusinessItemsAsync(
		int treatyId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all government organizations
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of government organizations</returns>
	[Get("/api/GovernmentOrganisation")]
	Task<List<GovernmentOrganisation>> GetGovernmentOrganisationsAsync(
		CancellationToken cancellationToken = default);
}
