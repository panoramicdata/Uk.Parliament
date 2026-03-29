#pragma warning disable CS1591
using Refit;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving written questions.
/// </summary>
public sealed record class GetWrittenQuestionsRequest : SkipTakeRequest
{
	[AliasAs("askingMemberId")]
	public int? AskingMemberId { get; init; }

	[AliasAs("answeringMemberId")]
	public int? AnsweringMemberId { get; init; }

	[AliasAs("answeringDepartment")]
	public string? AnsweringDepartment { get; init; }

	[AliasAs("house")]
	public string? House { get; init; }

	[AliasAs("tabledWhenFrom")]
	public DateTime? TabledWhenFrom { get; init; }

	[AliasAs("tabledWhenTo")]
	public DateTime? TabledWhenTo { get; init; }

	[AliasAs("answeredWhenFrom")]
	public DateTime? AnsweredWhenFrom { get; init; }

	[AliasAs("answeredWhenTo")]
	public DateTime? AnsweredWhenTo { get; init; }

	[AliasAs("isAnswered")]
	public bool? IsAnswered { get; init; }
}

/// <summary>
/// Request for retrieving written statements.
/// </summary>
public sealed record class GetWrittenStatementsRequest : SkipTakeRequest
{
	[AliasAs("makingMemberId")]
	public int? MakingMemberId { get; init; }

	[AliasAs("department")]
	public string? Department { get; init; }

	[AliasAs("house")]
	public string? House { get; init; }

	[AliasAs("madeWhenFrom")]
	public DateTime? MadeWhenFrom { get; init; }

	[AliasAs("madeWhenTo")]
	public DateTime? MadeWhenTo { get; init; }
}

/// <summary>
/// Request for retrieving daily reports.
/// </summary>
public sealed record class GetDailyReportsRequest : SkipTakeRequest
{
	[AliasAs("dateFrom")]
	public DateTime? DateFrom { get; init; }

	[AliasAs("dateTo")]
	public DateTime? DateTo { get; init; }

	[AliasAs("house")]
	public string? House { get; init; }
}
#pragma warning restore CS1591
