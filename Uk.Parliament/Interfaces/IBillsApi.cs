using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models.Bills;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Bills API client using Refit
/// </summary>
public interface IBillsApi
{
	/// <summary>
	/// Get list of bills with optional filtering
	/// </summary>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="session">Optional session ID filter</param>
	/// <param name="currentHouse">Optional current house filter</param>
	/// <param name="skip">Number of results to skip (default: 0)</param>
	/// <param name="take">Number of results to take (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of bills</returns>
	[Get("/api/v1/Bills")]
	Task<BillsListResponse<Bill>> GetBillsAsync(
		[Query] string? searchTerm = null,
		[Query] int? session = null,
		[Query] string? currentHouse = null,
		[Query] int skip = 0,
		[Query] int take = 20,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific bill by ID
	/// </summary>
	/// <param name="billId">Bill identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Bill details</returns>
	[Get("/api/v1/Bills/{billId}")]
	Task<Bill> GetBillByIdAsync(
		int billId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get list of bill types
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of bill types</returns>
	[Get("/api/v1/BillTypes")]
	Task<BillsListResponse<BillType>> GetBillTypesAsync(
		CancellationToken cancellationToken = default);
}
