#pragma warning disable CS1591
using Refit;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving oral questions.
/// </summary>
public sealed record class GetOralQuestionsRequest : SkipTakeRequest
{
	[AliasAs("askingMemberId")]
	public int? AskingMemberId { get; init; }

	[AliasAs("answeringDepartment")]
	public string? AnsweringDepartment { get; init; }

	[AliasAs("house")]
	public string? House { get; init; }

	[AliasAs("dateFrom")]
	public DateTime? DateFrom { get; init; }

	[AliasAs("dateTo")]
	public DateTime? DateTo { get; init; }

	[AliasAs("isAnswered")]
	public bool? IsAnswered { get; init; }
}

/// <summary>
/// Request for retrieving motions.
/// </summary>
public sealed record class GetMotionsRequest : SkipTakeRequest
{
	[AliasAs("proposingMemberId")]
	public int? ProposingMemberId { get; init; }

	[AliasAs("house")]
	public string? House { get; init; }

	[AliasAs("dateFrom")]
	public DateTime? DateFrom { get; init; }

	[AliasAs("dateTo")]
	public DateTime? DateTo { get; init; }

	[AliasAs("motionType")]
	public string? MotionType { get; init; }

	[AliasAs("isActive")]
	public bool? IsActive { get; init; }
}
#pragma warning restore CS1591
