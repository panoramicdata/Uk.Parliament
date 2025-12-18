namespace Uk.Parliament.Extensions;

/// <summary>
/// Options for querying oral questions
/// </summary>
public class OralQuestionsQueryOptions
{
	/// <summary>
	/// Filter by member who asked
	/// </summary>
	public int? AskingMemberId { get; set; }

	/// <summary>
	/// Filter by answering department
	/// </summary>
	public string? AnsweringDepartment { get; set; }

	/// <summary>
	/// Filter by house
	/// </summary>
	public string? House { get; set; }

	/// <summary>
	/// Filter by date from
	/// </summary>
	public DateTime? DateFrom { get; set; }

	/// <summary>
	/// Filter by date to
	/// </summary>
	public DateTime? DateTo { get; set; }

	/// <summary>
	/// Filter by answered status
	/// </summary>
	public bool? IsAnswered { get; set; }

	/// <summary>
	/// Items per page (default: 20)
	/// </summary>
	public int PageSize { get; set; } = 20;
}

/// <summary>
/// Options for querying motions
/// </summary>
public class MotionsQueryOptions
{
	/// <summary>
	/// Filter by member who proposed
	/// </summary>
	public int? ProposingMemberId { get; set; }

	/// <summary>
	/// Filter by house
	/// </summary>
	public string? House { get; set; }

	/// <summary>
	/// Filter by tabled date from
	/// </summary>
	public DateTime? DateFrom { get; set; }

	/// <summary>
	/// Filter by tabled date to
	/// </summary>
	public DateTime? DateTo { get; set; }

	/// <summary>
	/// Filter by motion type
	/// </summary>
	public string? MotionType { get; set; }

	/// <summary>
	/// Filter by active status
	/// </summary>
	public bool? IsActive { get; set; }

	/// <summary>
	/// Items per page (default: 20)
	/// </summary>
	public int PageSize { get; set; } = 20;
}
