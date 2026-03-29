using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Questions;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IQuestionsStatementsApi to provide additional functionality
/// </summary>
public static class QuestionsStatementsApiExtensions
{
	/// <summary>
	/// Get all written questions by automatically paginating through all results using a request model.
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all written questions</returns>
	public static IAsyncEnumerable<WrittenQuestion> GetAllWrittenQuestionsAsync(
		this IQuestionsStatementsApi api,
		GetWrittenQuestionsRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetWrittenQuestionsRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.GetWrittenQuestionsAsync(pageRequest, token),
			static response => GetWrappedItems(response),
			static response => response.TotalResults,
			cancellationToken);
	}

	/// <summary>
	/// Get all written questions by automatically paginating through all results using options
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="options">Query options</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all written questions</returns>
	public static IAsyncEnumerable<WrittenQuestion> GetAllWrittenQuestionsAsync(
		this IQuestionsStatementsApi api,
		WrittenQuestionsQueryOptions options,
		CancellationToken cancellationToken = default)
		=> api.GetAllWrittenQuestionsAsync(
		 new GetWrittenQuestionsRequest
		 {
			 AskingMemberId = options.AskingMemberId,
			 AnsweringMemberId = options.AnsweringMemberId,
			 House = options.House,
			 TabledWhenFrom = options.TabledWhenFrom,
			 TabledWhenTo = options.TabledWhenTo,
			 IsAnswered = options.IsAnswered,
			 Take = options.PageSize
		 },
			cancellationToken);

	/// <summary>
	/// Get all written questions by automatically paginating through all results
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="askingMemberId">Filter by member who asked</param>
	/// <param name="answeringMemberId">Filter by member who answered</param>
	/// <param name="house">Filter by house</param>
	/// <param name="tabledWhenFrom">Filter by tabled date from</param>
	/// <param name="tabledWhenTo">Filter by tabled date to</param>
	/// <param name="isAnswered">Filter by answered status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all written questions</returns>
  public static IAsyncEnumerable<WrittenQuestion> GetAllWrittenQuestionsAsync(
		this IQuestionsStatementsApi api,
		int? askingMemberId = null,
		int? answeringMemberId = null,
		string? house = null,
		DateTime? tabledWhenFrom = null,
		DateTime? tabledWhenTo = null,
		bool? isAnswered = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllWrittenQuestionsAsync(
			new GetWrittenQuestionsRequest
			{
				AskingMemberId = askingMemberId,
				AnsweringMemberId = answeringMemberId,
				House = house,
				TabledWhenFrom = tabledWhenFrom,
				TabledWhenTo = tabledWhenTo,
				IsAnswered = isAnswered,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all written questions as a materialized list using a request model.
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all written questions</returns>
	public static Task<List<WrittenQuestion>> GetAllWrittenQuestionsListAsync(
		this IQuestionsStatementsApi api,
		GetWrittenQuestionsRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllWrittenQuestionsAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all written questions as a materialized list using options
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="options">Query options</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all written questions</returns>
	public static Task<List<WrittenQuestion>> GetAllWrittenQuestionsListAsync(
		this IQuestionsStatementsApi api,
		WrittenQuestionsQueryOptions options,
		CancellationToken cancellationToken = default)
	 => api.GetAllWrittenQuestionsListAsync(
			new GetWrittenQuestionsRequest
			{
				AskingMemberId = options.AskingMemberId,
				AnsweringMemberId = options.AnsweringMemberId,
				House = options.House,
				TabledWhenFrom = options.TabledWhenFrom,
				TabledWhenTo = options.TabledWhenTo,
				IsAnswered = options.IsAnswered,
				Take = options.PageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all written questions as a materialized list
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="askingMemberId">Filter by member who asked</param>
	/// <param name="answeringMemberId">Filter by member who answered</param>
	/// <param name="house">Filter by house</param>
	/// <param name="tabledWhenFrom">Filter by tabled date from</param>
	/// <param name="tabledWhenTo">Filter by tabled date to</param>
	/// <param name="isAnswered">Filter by answered status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all written questions</returns>
    public static Task<List<WrittenQuestion>> GetAllWrittenQuestionsListAsync(
		this IQuestionsStatementsApi api,
		int? askingMemberId = null,
		int? answeringMemberId = null,
		string? house = null,
		DateTime? tabledWhenFrom = null,
		DateTime? tabledWhenTo = null,
		bool? isAnswered = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(
			api.GetAllWrittenQuestionsAsync(
				askingMemberId,
				answeringMemberId,
				house,
				tabledWhenFrom,
				tabledWhenTo,
				isAnswered,
				pageSize,
				cancellationToken),
			cancellationToken);

	/// <summary>
	/// Get all written statements by automatically paginating through all results using a request model.
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all written statements</returns>
	public static IAsyncEnumerable<WrittenStatement> GetAllWrittenStatementsAsync(
		this IQuestionsStatementsApi api,
		GetWrittenStatementsRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetWrittenStatementsRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.GetWrittenStatementsAsync(pageRequest, token),
			static response => GetWrappedItems(response),
			static response => response.TotalResults,
			cancellationToken);
	}

	/// <summary>
	/// Get all written statements by automatically paginating through all results
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="makingMemberId">Filter by member who made the statement</param>
	/// <param name="department">Filter by department</param>
	/// <param name="house">Filter by house</param>
	/// <param name="madeWhenFrom">Filter by made date from</param>
	/// <param name="madeWhenTo">Filter by made date to</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all written statements</returns>
    public static IAsyncEnumerable<WrittenStatement> GetAllWrittenStatementsAsync(
		this IQuestionsStatementsApi api,
		int? makingMemberId = null,
		string? department = null,
		string? house = null,
		DateTime? madeWhenFrom = null,
		DateTime? madeWhenTo = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllWrittenStatementsAsync(
			new GetWrittenStatementsRequest
			{
				MakingMemberId = makingMemberId,
				Department = department,
				House = house,
				MadeWhenFrom = madeWhenFrom,
				MadeWhenTo = madeWhenTo,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all written statements as a materialized list using a request model.
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all written statements</returns>
	public static Task<List<WrittenStatement>> GetAllWrittenStatementsListAsync(
		this IQuestionsStatementsApi api,
		GetWrittenStatementsRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllWrittenStatementsAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all written statements as a materialized list
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="makingMemberId">Filter by member who made the statement</param>
	/// <param name="department">Filter by department</param>
	/// <param name="house">Filter by house</param>
	/// <param name="madeWhenFrom">Filter by made date from</param>
	/// <param name="madeWhenTo">Filter by made date to</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all written statements</returns>
  public static Task<List<WrittenStatement>> GetAllWrittenStatementsListAsync(
		this IQuestionsStatementsApi api,
		int? makingMemberId = null,
		string? department = null,
		string? house = null,
		DateTime? madeWhenFrom = null,
		DateTime? madeWhenTo = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	   => PaginationHelper.ToListAsync(
			api.GetAllWrittenStatementsAsync(
				makingMemberId,
				department,
				house,
				madeWhenFrom,
				madeWhenTo,
				pageSize,
				cancellationToken),
			cancellationToken);

	/// <summary>
	/// Get all daily reports by automatically paginating through all results using a request model.
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all daily reports</returns>
	public static IAsyncEnumerable<DailyReport> GetAllDailyReportsAsync(
		this IQuestionsStatementsApi api,
		GetDailyReportsRequest? request = null,
		CancellationToken cancellationToken = default)
	{
		request ??= new GetDailyReportsRequest();
		var pageSize = request.Take ?? 20;

		return PaginationHelper.GetAllOffsetAsync(
			request,
			pageSize,
			static (current, skip, take) => current with { Skip = skip, Take = take },
			(pageRequest, token) => api.GetDailyReportsAsync(pageRequest, token),
			static response => GetWrappedItems(response),
			static response => response.TotalResults,
			cancellationToken);
	}

	/// <summary>
	/// Get all daily reports by automatically paginating through all results
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="dateFrom">Filter by date from</param>
	/// <param name="dateTo">Filter by date to</param>
	/// <param name="house">Filter by house</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all daily reports</returns>
  public static IAsyncEnumerable<DailyReport> GetAllDailyReportsAsync(
		this IQuestionsStatementsApi api,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		string? house = null,
		int pageSize = 20,
     CancellationToken cancellationToken = default)
	   => api.GetAllDailyReportsAsync(
			new GetDailyReportsRequest
			{
				DateFrom = dateFrom,
				DateTo = dateTo,
				House = house,
				Take = pageSize
			},
			cancellationToken);

	/// <summary>
	/// Get all daily reports as a materialized list using a request model.
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all daily reports</returns>
	public static Task<List<DailyReport>> GetAllDailyReportsListAsync(
		this IQuestionsStatementsApi api,
		GetDailyReportsRequest? request = null,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(api.GetAllDailyReportsAsync(request, cancellationToken), cancellationToken);

	/// <summary>
	/// Get all daily reports as a materialized list.
	/// </summary>
	/// <param name="api">The questions/statements API</param>
	/// <param name="dateFrom">Filter by date from</param>
	/// <param name="dateTo">Filter by date to</param>
	/// <param name="house">Filter by house</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all daily reports</returns>
	public static Task<List<DailyReport>> GetAllDailyReportsListAsync(
		this IQuestionsStatementsApi api,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		string? house = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
		=> PaginationHelper.ToListAsync(
			api.GetAllDailyReportsAsync(dateFrom, dateTo, house, pageSize, cancellationToken),
			cancellationToken);

	private static List<T>? GetWrappedItems<T>(PaginatedResponse<T>? response)
		=> response?.Items?.ConvertAll(static item => item.Value)
			?? response?.Results?.ConvertAll(static item => item.Value);
}