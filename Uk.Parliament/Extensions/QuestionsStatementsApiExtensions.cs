using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IQuestionsStatementsApi to provide additional functionality
/// </summary>
public static class QuestionsStatementsApiExtensions
{
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
			options.AskingMemberId,
			options.AnsweringMemberId,
			options.House,
			options.TabledWhenFrom,
			options.TabledWhenTo,
			options.IsAnswered,
			options.PageSize,
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
	public static async IAsyncEnumerable<WrittenQuestion> GetAllWrittenQuestionsAsync(
		this IQuestionsStatementsApi api,
		int? askingMemberId = null,
		int? answeringMemberId = null,
		string? house = null,
		DateTime? tabledWhenFrom = null,
		DateTime? tabledWhenTo = null,
		bool? isAnswered = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await foreach (var question in PaginateAsync(
			pageSize,
			async skip => await api.GetWrittenQuestionsAsync(
				askingMemberId,
				answeringMemberId,
				null,
				house,
				tabledWhenFrom,
				tabledWhenTo,
				null,
				null,
				isAnswered,
				skip,
				pageSize,
				cancellationToken),
			cancellationToken))
		{
			yield return question;
		}
	}

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
			options.AskingMemberId,
			options.AnsweringMemberId,
			options.House,
			options.TabledWhenFrom,
			options.TabledWhenTo,
			options.IsAnswered,
			options.PageSize,
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
	public static async Task<List<WrittenQuestion>> GetAllWrittenQuestionsListAsync(
		this IQuestionsStatementsApi api,
		int? askingMemberId = null,
		int? answeringMemberId = null,
		string? house = null,
		DateTime? tabledWhenFrom = null,
		DateTime? tabledWhenTo = null,
		bool? isAnswered = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allQuestions = new List<WrittenQuestion>();

		await foreach (var question in api.GetAllWrittenQuestionsAsync(
			askingMemberId,
			answeringMemberId,
			house,
			tabledWhenFrom,
			tabledWhenTo,
			isAnswered,
			pageSize,
			cancellationToken))
		{
			allQuestions.Add(question);
		}

		return allQuestions;
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
	public static async IAsyncEnumerable<WrittenStatement> GetAllWrittenStatementsAsync(
		this IQuestionsStatementsApi api,
		int? makingMemberId = null,
		string? department = null,
		string? house = null,
		DateTime? madeWhenFrom = null,
		DateTime? madeWhenTo = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await foreach (var statement in PaginateAsync(
			pageSize,
			async skip => await api.GetWrittenStatementsAsync(
				makingMemberId,
				department,
				house,
				madeWhenFrom,
				madeWhenTo,
				skip,
				pageSize,
				cancellationToken),
			cancellationToken))
		{
			yield return statement;
		}
	}

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
	public static async Task<List<WrittenStatement>> GetAllWrittenStatementsListAsync(
		this IQuestionsStatementsApi api,
		int? makingMemberId = null,
		string? department = null,
		string? house = null,
		DateTime? madeWhenFrom = null,
		DateTime? madeWhenTo = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allStatements = new List<WrittenStatement>();

		await foreach (var statement in api.GetAllWrittenStatementsAsync(
			makingMemberId,
			department,
			house,
			madeWhenFrom,
			madeWhenTo,
			pageSize,
			cancellationToken))
		{
			allStatements.Add(statement);
		}

		return allStatements;
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
	public static async IAsyncEnumerable<DailyReport> GetAllDailyReportsAsync(
		this IQuestionsStatementsApi api,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		string? house = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await foreach (var report in PaginateAsync(
			pageSize,
			async skip => await api.GetDailyReportsAsync(
				dateFrom,
				dateTo,
				house,
				skip,
				pageSize,
				cancellationToken),
			cancellationToken))
		{
			yield return report;
		}
	}

	/// <summary>
	/// Generic pagination helper that iterates through all pages of a paginated response
	/// </summary>
	private static async IAsyncEnumerable<T> PaginateAsync<T>(
		int pageSize,
		Func<int, Task<PaginatedResponse<T>?>> fetchPage,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (!cancellationToken.IsCancellationRequested)
		{
			var response = await fetchPage(skip);
			var items = GetItemsFromResponse(response);

			if (items is null || items.Count == 0)
			{
				yield break;
			}

			foreach (var item in items)
			{
				yield return item.Value;
			}

			if (IsLastPage(items.Count, skip, pageSize, response!.TotalResults))
			{
				yield break;
			}

			skip += pageSize;
		}
	}

	/// <summary>
	/// Gets items from the response, checking both Items and Results properties (API uses both)
	/// </summary>
	private static List<ValueWrapper<T>>? GetItemsFromResponse<T>(PaginatedResponse<T>? response)
		=> response?.Items ?? response?.Results;

	/// <summary>
	/// Determines if this is the last page of results
	/// </summary>
	private static bool IsLastPage(int itemCount, int skip, int pageSize, int totalResults)
		=> itemCount < pageSize || skip + pageSize >= totalResults;
}