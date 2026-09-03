using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models.Bills;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Bills API client using Refit
/// </summary>
public interface IBillsApi
{
	/// <summary>
	/// Get list of bills with optional filtering
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of bills</returns>
	[Get("/api/v1/Bills")]
	Task<BillsListResponse<Bill>> GetBillsAsync(
		[Query] GetBillsRequest request,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get a specific bill by ID
	/// </summary>
	/// <param name="billId">Bill identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Bill details</returns>
	[Get("/api/v1/Bills/{billId}")]
	Task<Bill> GetBillByIdAsync(
		int billId,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get list of bill types
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of bill types</returns>
	[Get("/api/v1/BillTypes")]
	Task<BillsListResponse<BillType>> GetBillTypesAsync(
		CancellationToken cancellationToken);
}
