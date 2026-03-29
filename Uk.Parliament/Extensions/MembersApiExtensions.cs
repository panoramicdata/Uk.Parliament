using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Members;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IMembersApi to provide additional functionality
/// </summary>
public static class MembersApiExtensions
{
	/// <summary>
	/// Get all members by automatically paginating through all results.
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all members</returns>
	public static IAsyncEnumerable<Member> GetAllAsync(
		this IMembersApi api,
		SearchMembersRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new SearchMembersRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.SearchAsync(pageRequest, token),
			static response => response.Items?.ConvertAll(static item => item.Value)
				?? response.Results?.ConvertAll(static item => item.Value),
			static response => response.TotalResults,
			cancellationToken);
	}

	/// <summary>
	/// Get all members by automatically paginating through all results
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="name">Optional name to search for</param>
	/// <param name="house">Optional house filter (1 = Commons, 2 = Lords)</param>
	/// <param name="isCurrentMember">Optional filter for current members only</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all members</returns>
   public static IAsyncEnumerable<Member> GetAllAsync(
		this IMembersApi api,
		string? name = null,
		int? house = null,
		bool? isCurrentMember = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllAsync(
			new SearchMembersRequest
			{
				Name = name,
				House = house,
				IsCurrentMember = isCurrentMember,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all members as a materialized list.
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all members</returns>
	public static Task<List<Member>> GetAllListAsync(
		this IMembersApi api,
		SearchMembersRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all members as a materialized list
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="name">Optional name to search for</param>
	/// <param name="house">Optional house filter (1 = Commons, 2 = Lords)</param>
	/// <param name="isCurrentMember">Optional filter for current members only</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all members</returns>
 public static Task<List<Member>> GetAllListAsync(
		this IMembersApi api,
		string? name = null,
		int? house = null,
		bool? isCurrentMember = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(api.GetAllAsync(name, house, isCurrentMember, pageSize, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all constituencies by automatically paginating through all results.
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all constituencies</returns>
	public static IAsyncEnumerable<Constituency> GetAllConstituenciesAsync(
		this IMembersApi api,
		SearchConstituenciesRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new SearchConstituenciesRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.SearchConstituenciesAsync(pageRequest, token),
			static response => response.Items?.ConvertAll(static item => item.Value)
				?? response.Results?.ConvertAll(static item => item.Value),
			static response => response.TotalResults,
			cancellationToken);
	}

	/// <summary>
	/// Get all constituencies by automatically paginating through all results
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="searchText">Optional search text</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all constituencies</returns>
   public static IAsyncEnumerable<Constituency> GetAllConstituenciesAsync(
		this IMembersApi api,
		string? searchText = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllConstituenciesAsync(
			new SearchConstituenciesRequest
			{
				SearchText = searchText,
				Take = pageSize
			},
			cancellationToken);
}
