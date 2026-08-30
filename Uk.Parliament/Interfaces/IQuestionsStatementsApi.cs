using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Questions;
using Uk.Parliament.Requests;

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
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of written questions</returns>
	[Get("/api/writtenquestions/questions")]
	Task<PaginatedResponse<WrittenQuestion>> GetWrittenQuestionsAsync(
		[Query] GetWrittenQuestionsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific written question by ID
	/// </summary>
	/// <param name="id">Question identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Written question details wrapped in ValueWrapper</returns>
	[Get("/api/writtenquestions/questions/{id}")]
	Task<ValueWrapper<WrittenQuestion>> GetWrittenQuestionByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a written question by date and UIN
	/// </summary>
	/// <param name="date">Date the question was tabled</param>
	/// <param name="uin">Unique Identifier Number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Written question details wrapped in ValueWrapper</returns>
	[Get("/api/writtenquestions/questions/{date}/{uin}")]
	Task<ValueWrapper<WrittenQuestion>> GetWrittenQuestionByDateAndUinAsync(
		DateTime date,
		string uin,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get written statements with optional filtering
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of written statements</returns>
	[Get("/api/writtenstatements/statements")]
	Task<PaginatedResponse<WrittenStatement>> GetWrittenStatementsAsync(
		[Query] GetWrittenStatementsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific written statement by ID
	/// </summary>
	/// <param name="id">Statement identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Written statement details wrapped in ValueWrapper</returns>
	[Get("/api/writtenstatements/statements/{id}")]
	Task<ValueWrapper<WrittenStatement>> GetWrittenStatementByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a written statement by date and UIN
	/// </summary>
	/// <param name="date">Date the statement was made</param>
	/// <param name="uin">Unique Identifier Number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Written statement details wrapped in ValueWrapper</returns>
	[Get("/api/writtenstatements/statements/{date}/{uin}")]
	Task<ValueWrapper<WrittenStatement>> GetWrittenStatementByDateAndUinAsync(
		DateTime date,
		string uin,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get daily reports with optional filtering
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of daily reports</returns>
	[Get("/api/dailyreports/dailyreports")]
	Task<PaginatedResponse<DailyReport>> GetDailyReportsAsync(
		[Query] GetDailyReportsRequest request,
		CancellationToken cancellationToken = default);
}
