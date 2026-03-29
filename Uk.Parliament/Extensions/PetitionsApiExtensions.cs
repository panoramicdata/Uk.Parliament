using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IPetitionsApi to provide additional functionality
/// </summary>
public static class PetitionsApiExtensions
{
	/// <summary>
	/// Get all petitions by automatically paginating through all results.
	/// </summary>
	/// <param name="api">The petitions API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all petitions</returns>
	public static IAsyncEnumerable<Petition> GetAllAsync(
		this IPetitionsApi api,
		GetPetitionsRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetPetitionsRequest();
		var pageSize = request.PageSize ?? 50;

		return PaginationHelper.GetAllPageAsync(
			request,
			pageSize,
			static (current, page, size) => current with { Page = page, PageSize = size },
			(pageRequest, token) => api.GetAsync(pageRequest, token),
			static response => response.Data,
			cancellationToken);
	}

	/// <summary>
	/// Get all petitions by automatically paginating through all results
	/// </summary>
	/// <param name="api">The petitions API</param>
	/// <param name="search">Optional search term</param>
	/// <param name="state">Optional state filter</param>
	/// <param name="pageSize">Items per page (default: 50)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all petitions</returns>
 public static IAsyncEnumerable<Petition> GetAllAsync(
		this IPetitionsApi api,
		string? search = null,
		string? state = null,
		int pageSize = 50,
     CancellationToken cancellationToken = default)
	   => api.GetAllAsync(
			new GetPetitionsRequest
			{
				Search = search,
				State = state,
				PageSize = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all petitions as a materialized list.
	/// </summary>
	/// <param name="api">The petitions API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all petitions</returns>
	public static Task<List<Petition>> GetAllListAsync(
		this IPetitionsApi api,
		GetPetitionsRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all petitions as a materialized list
	/// </summary>
	/// <param name="api">The petitions API</param>
	/// <param name="search">Optional search term</param>
	/// <param name="state">Optional state filter</param>
	/// <param name="pageSize">Items per page (default: 50)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all petitions</returns>
   public static Task<List<Petition>> GetAllListAsync(
		this IPetitionsApi api,
		string? search = null,
		string? state = null,
		int pageSize = 50,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(api.GetAllAsync(search, state, pageSize, cancellationToken), cancellationToken);
}
