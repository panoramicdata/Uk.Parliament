using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.OralQuestions;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IOralQuestionsMotionsApi to provide additional functionality
/// </summary>
public static class OralQuestionsMotionsApiExtensions
{
	/// <summary>
	/// Get all oral questions by automatically paginating through all results using a request model.
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all oral questions</returns>
	public static IAsyncEnumerable<OralQuestion> GetAllOralQuestionsAsync(
		this IOralQuestionsMotionsApi api,
		GetOralQuestionsRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetOralQuestionsRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.GetOralQuestionsAsync(pageRequest, token),
			static response => response.Response,
			static response => response.PagingInfo.Total,
			cancellationToken);
	}

	/// <summary>
	/// Get all oral questions by automatically paginating through all results using options
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="options">Query options</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all oral questions</returns>
	public static IAsyncEnumerable<OralQuestion> GetAllOralQuestionsAsync(
		this IOralQuestionsMotionsApi api,
		OralQuestionsQueryOptions options,
		CancellationToken cancellationToken = default)
		=> api.GetAllOralQuestionsAsync(
		 new GetOralQuestionsRequest
		 {
			 AskingMemberId = options.AskingMemberId,
			 AnsweringDepartment = options.AnsweringDepartment,
			 House = options.House,
			 DateFrom = options.DateFrom,
			 DateTo = options.DateTo,
			 IsAnswered = options.IsAnswered,
			 Take = options.PageSize
		 },
			cancellationToken);

	/// <summary>
	/// Get all oral questions by automatically paginating through all results
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="askingMemberId">Filter by member who asked</param>
	/// <param name="answeringDepartment">Filter by answering department</param>
	/// <param name="house">Filter by house</param>
	/// <param name="dateFrom">Filter by date from</param>
	/// <param name="dateTo">Filter by date to</param>
	/// <param name="isAnswered">Filter by answered status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all oral questions</returns>
    public static IAsyncEnumerable<OralQuestion> GetAllOralQuestionsAsync(
		this IOralQuestionsMotionsApi api,
		int? askingMemberId = null,
		string? answeringDepartment = null,
		string? house = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		bool? isAnswered = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllOralQuestionsAsync(
			new GetOralQuestionsRequest
			{
				AskingMemberId = askingMemberId,
				AnsweringDepartment = answeringDepartment,
				House = house,
				DateFrom = dateFrom,
				DateTo = dateTo,
				IsAnswered = isAnswered,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all oral questions as a materialized list using a request model.
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all oral questions</returns>
	public static Task<List<OralQuestion>> GetAllOralQuestionsListAsync(
		this IOralQuestionsMotionsApi api,
		GetOralQuestionsRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllOralQuestionsAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all oral questions as a materialized list using options
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="options">Query options</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all oral questions</returns>
	public static Task<List<OralQuestion>> GetAllOralQuestionsListAsync(
		this IOralQuestionsMotionsApi api,
		OralQuestionsQueryOptions options,
		CancellationToken cancellationToken = default)
		=> api.GetAllOralQuestionsListAsync(
		 new GetOralQuestionsRequest
		 {
			 AskingMemberId = options.AskingMemberId,
			 AnsweringDepartment = options.AnsweringDepartment,
			 House = options.House,
			 DateFrom = options.DateFrom,
			 DateTo = options.DateTo,
			 IsAnswered = options.IsAnswered,
			 Take = options.PageSize
		 },
			cancellationToken);

	/// <summary>
	/// Get all oral questions as a materialized list
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="askingMemberId">Filter by member who asked</param>
	/// <param name="answeringDepartment">Filter by answering department</param>
	/// <param name="house">Filter by house</param>
	/// <param name="dateFrom">Filter by date from</param>
	/// <param name="dateTo">Filter by date to</param>
	/// <param name="isAnswered">Filter by answered status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all oral questions</returns>
  public static Task<List<OralQuestion>> GetAllOralQuestionsListAsync(
		this IOralQuestionsMotionsApi api,
		int? askingMemberId = null,
		string? answeringDepartment = null,
		string? house = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		bool? isAnswered = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(
			api.GetAllOralQuestionsAsync(
				askingMemberId,
				answeringDepartment,
				house,
				dateFrom,
				dateTo,
				isAnswered,
				pageSize,
				cancellationToken),
			cancellationToken);

	/// <summary>
	/// Get all motions by automatically paginating through all results using a request model.
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all motions</returns>
	public static IAsyncEnumerable<Motion> GetAllMotionsAsync(
		this IOralQuestionsMotionsApi api,
		GetMotionsRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetMotionsRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.GetMotionsAsync(pageRequest, token),
			static response => response.Response,
			static response => response.PagingInfo.Total,
			cancellationToken);
	}

	/// <summary>
	/// Get all motions by automatically paginating through all results using options
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="options">Query options</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all motions</returns>
	public static IAsyncEnumerable<Motion> GetAllMotionsAsync(
		this IOralQuestionsMotionsApi api,
		MotionsQueryOptions options,
		CancellationToken cancellationToken = default)
		=> api.GetAllMotionsAsync(
		  new GetMotionsRequest
		  {
			  ProposingMemberId = options.ProposingMemberId,
			  House = options.House,
			  DateFrom = options.DateFrom,
			  DateTo = options.DateTo,
			  MotionType = options.MotionType,
			  IsActive = options.IsActive,
			  Take = options.PageSize
		  },
			cancellationToken);

	/// <summary>
	/// Get all motions by automatically paginating through all results
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="proposingMemberId">Filter by member who proposed</param>
	/// <param name="house">Filter by house</param>
	/// <param name="dateFrom">Filter by tabled date from</param>
	/// <param name="dateTo">Filter by tabled date to</param>
	/// <param name="motionType">Filter by motion type</param>
	/// <param name="isActive">Filter by active status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all motions</returns>
    public static IAsyncEnumerable<Motion> GetAllMotionsAsync(
		this IOralQuestionsMotionsApi api,
		int? proposingMemberId = null,
		string? house = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		string? motionType = null,
		bool? isActive = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllMotionsAsync(
			new GetMotionsRequest
			{
				ProposingMemberId = proposingMemberId,
				House = house,
				DateFrom = dateFrom,
				DateTo = dateTo,
				MotionType = motionType,
				IsActive = isActive,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all motions as a materialized list using a request model.
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all motions</returns>
	public static Task<List<Motion>> GetAllMotionsListAsync(
		this IOralQuestionsMotionsApi api,
		GetMotionsRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllMotionsAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all motions as a materialized list using options
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="options">Query options</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all motions</returns>
	public static Task<List<Motion>> GetAllMotionsListAsync(
		this IOralQuestionsMotionsApi api,
		MotionsQueryOptions options,
		CancellationToken cancellationToken = default)
		=> api.GetAllMotionsListAsync(
		  new GetMotionsRequest
		  {
			  ProposingMemberId = options.ProposingMemberId,
			  House = options.House,
			  DateFrom = options.DateFrom,
			  DateTo = options.DateTo,
			  MotionType = options.MotionType,
			  IsActive = options.IsActive,
			  Take = options.PageSize
		  },
			cancellationToken);

	/// <summary>
	/// Get all motions as a materialized list
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="proposingMemberId">Filter by member who proposed</param>
	/// <param name="house">Filter by house</param>
	/// <param name="dateFrom">Filter by tabled date from</param>
	/// <param name="dateTo">Filter by tabled date to</param>
	/// <param name="motionType">Filter by motion type</param>
	/// <param name="isActive">Filter by active status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all motions</returns>
  public static Task<List<Motion>> GetAllMotionsListAsync(
		this IOralQuestionsMotionsApi api,
		int? proposingMemberId = null,
		string? house = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		string? motionType = null,
		bool? isActive = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(
			api.GetAllMotionsAsync(
				proposingMemberId,
				house,
				dateFrom,
				dateTo,
				motionType,
				isActive,
				pageSize,
				cancellationToken),
			cancellationToken);
}
