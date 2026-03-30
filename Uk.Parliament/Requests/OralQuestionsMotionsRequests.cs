using Refit;
using Uk.Parliament.Models.OralQuestions;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving oral questions.
/// </summary>
public sealed record class GetOralQuestionsRequest : SkipTakeRequest, IPaginatedRequest<OralQuestion>
{
	/// <summary>
	/// Filter by member who asked.
	/// </summary>
	[AliasAs("askingMemberId")]
	public int? AskingMemberId { get; init; }

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
	/// Filter by answered status.
	/// </summary>
	[AliasAs("isAnswered")]
	public bool? IsAnswered { get; init; }
}

/// <summary>
/// Request for retrieving motions.
/// </summary>
public sealed record class GetMotionsRequest : SkipTakeRequest, IPaginatedRequest<Motion>
{
	/// <summary>
	/// Filter by member who proposed.
	/// </summary>
	[AliasAs("proposingMemberId")]
	public int? ProposingMemberId { get; init; }

	/// <summary>
	/// Filter by house (Commons/Lords).
	/// </summary>
	[AliasAs("house")]
	public string? House { get; init; }

	/// <summary>
	/// Filter by tabled date from.
	/// </summary>
	[AliasAs("dateFrom")]
	public DateTime? DateFrom { get; init; }

	/// <summary>
	/// Filter by tabled date to.
	/// </summary>
	[AliasAs("dateTo")]
	public DateTime? DateTo { get; init; }

	/// <summary>
	/// Filter by motion type.
	/// </summary>
	[AliasAs("motionType")]
	public string? MotionType { get; init; }

	/// <summary>
	/// Filter by active status.
	/// </summary>
	[AliasAs("isActive")]
	public bool? IsActive { get; init; }
}
