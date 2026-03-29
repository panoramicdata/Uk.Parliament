using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Committees;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for ICommitteesApi to provide additional functionality
/// </summary>
public static class CommitteesApiExtensions
{
	/// <summary>
	/// Get all committees by automatically paginating through all results.
	/// </summary>
	/// <param name="api">The committees API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all committees</returns>
	public static IAsyncEnumerable<Committee> GetAllCommitteesAsync(
		this ICommitteesApi api,
		GetCommitteesRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetCommitteesRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.GetCommitteesAsync(pageRequest, token),
			static response => response.Items,
			static response => response.TotalResults,
			cancellationToken);
	}

	/// <summary>
	/// Get all committees by automatically paginating through all results
	/// </summary>
	/// <param name="api">The committees API</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all committees</returns>
  public static IAsyncEnumerable<Committee> GetAllCommitteesAsync(
		this ICommitteesApi api,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllCommitteesAsync(new GetCommitteesRequest { Take = pageSize }, cancellationToken);

	/// <summary>
	/// Get all committees as a materialized list.
	/// </summary>
	/// <param name="api">The committees API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all committees</returns>
	public static Task<List<Committee>> GetAllCommitteesListAsync(
		this ICommitteesApi api,
		GetCommitteesRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllCommitteesAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all committees as a materialized list
	/// </summary>
	/// <param name="api">The committees API</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all committees</returns>
    public static Task<List<Committee>> GetAllCommitteesListAsync(
		this ICommitteesApi api,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(api.GetAllCommitteesAsync(pageSize, cancellationToken), cancellationToken);
}
