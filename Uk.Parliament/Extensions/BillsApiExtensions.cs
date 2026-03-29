using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Bills;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IBillsApi to provide additional functionality
/// </summary>
public static class BillsApiExtensions
{
	/// <summary>
	/// Get all bills by automatically paginating through all results.
	/// </summary>
	/// <param name="api">The bills API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all bills</returns>
	public static IAsyncEnumerable<Bill> GetAllBillsAsync(
		this IBillsApi api,
		GetBillsRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetBillsRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.GetBillsAsync(pageRequest, token),
			static response => response.Items,
			static response => response.TotalResults,
			cancellationToken);
	}

	/// <summary>
	/// Get all bills by automatically paginating through all results
	/// </summary>
	/// <param name="api">The bills API</param>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="session">Optional session ID filter</param>
	/// <param name="currentHouse">Optional current house filter</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all bills</returns>
    public static IAsyncEnumerable<Bill> GetAllBillsAsync(
		this IBillsApi api,
		string? searchTerm = null,
		int? session = null,
		string? currentHouse = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllBillsAsync(
			new GetBillsRequest
			{
				SearchTerm = searchTerm,
				Session = session,
				CurrentHouse = currentHouse,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all bills as a materialized list.
	/// </summary>
	/// <param name="api">The bills API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all bills</returns>
	public static Task<List<Bill>> GetAllBillsListAsync(
		this IBillsApi api,
		GetBillsRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllBillsAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all bills as a materialized list
	/// </summary>
	/// <param name="api">The bills API</param>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="session">Optional session ID filter</param>
	/// <param name="currentHouse">Optional current house filter</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all bills</returns>
  public static Task<List<Bill>> GetAllBillsListAsync(
		this IBillsApi api,
		string? searchTerm = null,
		int? session = null,
		string? currentHouse = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(
			api.GetAllBillsAsync(searchTerm, session, currentHouse, pageSize, cancellationToken),
			cancellationToken);
}
