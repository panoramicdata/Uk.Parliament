using Refit;
using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving written questions.
/// </summary>
public sealed record class GetWrittenQuestionsRequest : SkipTakeRequest, IPaginatedRequest<WrittenQuestion>
{
	/// <summary>
	/// Filter by member who asked the question.
	/// </summary>
	[AliasAs("askingMemberId")]
	public int? AskingMemberId { get; init; }

	/// <summary>
	/// Filter by member who answered the question.
	/// </summary>
	[AliasAs("answeringMemberId")]
	public int? AnsweringMemberId { get; init; }

	/// <summary>
	/// Filter by answering department.
	/// </summary>
	[AliasAs("answeringDepartment")]
	public string? AnsweringDepartment { get; init; }

	/// <summary>
	/// Filter by house (Commons/Lords).
	/// </summary>
	[AliasAs("house")]
	public string? House { get; init; }

	/// <summary>
	/// Filter by tabled date from.
	/// </summary>
	[AliasAs("tabledWhenFrom")]
	public DateTime? TabledWhenFrom { get; init; }

	/// <summary>
	/// Filter by tabled date to.
	/// </summary>
	[AliasAs("tabledWhenTo")]
	public DateTime? TabledWhenTo { get; init; }

	/// <summary>
	/// Filter by answered date from.
	/// </summary>
	[AliasAs("answeredWhenFrom")]
	public DateTime? AnsweredWhenFrom { get; init; }

	/// <summary>
	/// Filter by answered date to.
	/// </summary>
	[AliasAs("answeredWhenTo")]
	public DateTime? AnsweredWhenTo { get; init; }

	/// <summary>
	/// Filter by answered status.
	/// </summary>
	[AliasAs("isAnswered")]
	public bool? IsAnswered { get; init; }
}

/// <summary>
/// Request for retrieving written statements.
/// </summary>
public sealed record class GetWrittenStatementsRequest : SkipTakeRequest, IPaginatedRequest<WrittenStatement>
{
	/// <summary>
	/// Filter by member who made the statement.
	/// </summary>
	[AliasAs("makingMemberId")]
	public int? MakingMemberId { get; init; }

	/// <summary>
	/// Filter by department.
	/// </summary>
	[AliasAs("department")]
	public string? Department { get; init; }

	/// <summary>
	/// Filter by house (Commons/Lords).
	/// </summary>
	[AliasAs("house")]
	public string? House { get; init; }

	/// <summary>
	/// Filter by made date from.
	/// </summary>
	[AliasAs("madeWhenFrom")]
	public DateTime? MadeWhenFrom { get; init; }

	/// <summary>
	/// Filter by made date to.
	/// </summary>
	[AliasAs("madeWhenTo")]
	public DateTime? MadeWhenTo { get; init; }
}

/// <summary>
/// Request for retrieving daily reports.
/// </summary>
public sealed record class GetDailyReportsRequest : SkipTakeRequest, IPaginatedRequest<DailyReport>
{
	/// <summary>
	/// Filter by date from.
	/// </summary>
	[AliasAs("dateFrom")]
	public DateTime? DateFrom { get; init; }

	/// <summary>
	/// Filter by date to.
	/// </summary>
	[AliasAs("dateTo")]
	public DateTime? DateTo { get; init; }

	/// <summary>
	/// Filter by house (Commons/Lords).
	/// </summary>
	[AliasAs("house")]
	public string? House { get; init; }
}
