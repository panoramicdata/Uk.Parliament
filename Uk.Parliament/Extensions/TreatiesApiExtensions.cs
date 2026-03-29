using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Treaties;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for ITreatiesApi to provide additional functionality
/// </summary>
public static class TreatiesApiExtensions
{
	/// <summary>
	/// Get all treaties by automatically paginating through all results.
	/// </summary>
	/// <param name="api">The treaties API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all treaties</returns>
	public static IAsyncEnumerable<Treaty> GetAllTreatiesAsync(
		this ITreatiesApi api,
		GetTreatiesRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetTreatiesRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.GetTreatiesAsync(pageRequest, token),
			static response => response.Items?.ConvertAll(static item => item.Value)
				?? response.Results?.ConvertAll(static item => item.Value),
			static response => response.TotalResults,
			cancellationToken);
	}

	/// <summary>
	/// Get all treaties by automatically paginating through all results
	/// </summary>
	/// <param name="api">The treaties API</param>
	/// <param name="governmentOrganisationId">Filter by government organization</param>
	/// <param name="house">Filter by house</param>
	/// <param name="status">Filter by treaty status</param>
	/// <param name="dateLaidFrom">Filter by laid date from</param>
	/// <param name="dateLaidTo">Filter by laid date to</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all treaties</returns>
   public static IAsyncEnumerable<Treaty> GetAllTreatiesAsync(
		this ITreatiesApi api,
		int? governmentOrganisationId = null,
		string? house = null,
		string? status = null,
		DateTime? dateLaidFrom = null,
		DateTime? dateLaidTo = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllTreatiesAsync(
			new GetTreatiesRequest
			{
				GovernmentOrganisationId = governmentOrganisationId,
				House = house,
				Status = status,
				DateLaidFrom = dateLaidFrom,
				DateLaidTo = dateLaidTo,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all treaties as a materialized list.
	/// </summary>
	/// <param name="api">The treaties API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all treaties</returns>
	public static Task<List<Treaty>> GetAllTreatiesListAsync(
		this ITreatiesApi api,
		GetTreatiesRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllTreatiesAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all treaties as a materialized list
	/// </summary>
	/// <param name="api">The treaties API</param>
	/// <param name="governmentOrganisationId">Filter by government organization</param>
	/// <param name="house">Filter by house</param>
	/// <param name="status">Filter by treaty status</param>
	/// <param name="dateLaidFrom">Filter by laid date from</param>
	/// <param name="dateLaidTo">Filter by laid date to</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all treaties</returns>
 public static Task<List<Treaty>> GetAllTreatiesListAsync(
		this ITreatiesApi api,
		int? governmentOrganisationId = null,
		string? house = null,
		string? status = null,
		DateTime? dateLaidFrom = null,
		DateTime? dateLaidTo = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(
			api.GetAllTreatiesAsync(
				governmentOrganisationId,
				house,
				status,
				dateLaidFrom,
				dateLaidTo,
				pageSize,
				cancellationToken),
			cancellationToken);
}
