using System;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Written Questions and Statements API client using Refit
/// </summary>
/// <remarks>
/// Provides access to written parliamentary questions and ministerial statements
/// </remarks>
public interface IQuestionsStatementsApi
{
	/// <summary>
	/// Get written questions with optional filtering
	/// </summary>
	/// <param name="askingMemberId">Filter by member who asked the question</param>
	/// <param name="answeringMemberId">Filter by member who answered</param>
	/// <param name="answeringDepartment">Filter by answering department</param>
	/// <param name="house">Filter by house (Commons/Lords)</param>
	/// <param name="tabledWhenFrom">Filter by tabled date from</param>
	/// <param name="tabledWhenTo">Filter by tabled date to</param>
	/// <param name="answeredWhenFrom">Filter by answered date from</param>
	/// <param name="answeredWhenTo">Filter by answered date to</param>
	/// <param name="isAnswered">Filter by answered status</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of written questions</returns>
	[Get("/api/writtenquestions/questions")]
	Task<PaginatedResponse<WrittenQuestion>> GetWrittenQuestionsAsync(
		[Query] int? askingMemberId = null,
		[Query] int? answeringMemberId = null,
		[Query] string? answeringDepartment = null,
		[Query] string? house = null,
		[Query] DateTime? tabledWhenFrom = null,
		[Query] DateTime? tabledWhenTo = null,
		[Query] DateTime? answeredWhenFrom = null,
		[Query] DateTime? answeredWhenTo = null,
		[Query] bool? isAnswered = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific written question by ID
	/// </summary>
	/// <param name="id">Question identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Written question details</returns>
	[Get("/api/writtenquestions/questions/{id}")]
	Task<WrittenQuestion> GetWrittenQuestionByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a written question by date and UIN
	/// </summary>
	/// <param name="date">Date the question was tabled</param>
	/// <param name="uin">Unique Identifier Number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Written question details</returns>
	[Get("/api/writtenquestions/questions/{date}/{uin}")]
	Task<WrittenQuestion> GetWrittenQuestionByDateAndUinAsync(
		DateTime date,
		string uin,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get written statements with optional filtering
	/// </summary>
	/// <param name="makingMemberId">Filter by member who made the statement</param>
	/// <param name="department">Filter by department</param>
	/// <param name="house">Filter by house (Commons/Lords)</param>
	/// <param name="madeWhenFrom">Filter by made date from</param>
	/// <param name="madeWhenTo">Filter by made date to</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of written statements</returns>
	[Get("/api/writtenstatements/statements")]
	Task<PaginatedResponse<WrittenStatement>> GetWrittenStatementsAsync(
		[Query] int? makingMemberId = null,
		[Query] string? department = null,
		[Query] string? house = null,
		[Query] DateTime? madeWhenFrom = null,
		[Query] DateTime? madeWhenTo = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific written statement by ID
	/// </summary>
	/// <param name="id">Statement identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Written statement details</returns>
	[Get("/api/writtenstatements/statements/{id}")]
	Task<WrittenStatement> GetWrittenStatementByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a written statement by date and UIN
	/// </summary>
	/// <param name="date">Date the statement was made</param>
	/// <param name="uin">Unique Identifier Number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Written statement details</returns>
	[Get("/api/writtenstatements/statements/{date}/{uin}")]
	Task<WrittenStatement> GetWrittenStatementByDateAndUinAsync(
		DateTime date,
		string uin,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get daily reports with optional filtering
	/// </summary>
	/// <param name="dateFrom">Filter by date from</param>
	/// <param name="dateTo">Filter by date to</param>
	/// <param name="house">Filter by house (Commons/Lords)</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of daily reports</returns>
	[Get("/api/dailyreports/dailyreports")]
	Task<PaginatedResponse<DailyReport>> GetDailyReportsAsync(
		[Query] DateTime? dateFrom = null,
		[Query] DateTime? dateTo = null,
		[Query] string? house = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);
}
