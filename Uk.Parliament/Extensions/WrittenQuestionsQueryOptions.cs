namespace Uk.Parliament.Extensions;

/// <summary>
/// Options for querying written questions
/// </summary>
public class WrittenQuestionsQueryOptions
{
	/// <summary>
	/// Filter by member who asked
	/// </summary>
	public int? AskingMemberId { get; set; }

	/// <summary>
	/// Filter by member who answered
	/// </summary>
	public int? AnsweringMemberId { get; set; }

	/// <summary>
	/// Filter by house
	/// </summary>
	public string? House { get; set; }

	/// <summary>
	/// Filter by tabled date from
	/// </summary>
	public DateTime? TabledWhenFrom { get; set; }

	/// <summary>
	/// Filter by tabled date to
	/// </summary>
	public DateTime? TabledWhenTo { get; set; }

	/// <summary>
	/// Filter by answered status
	/// </summary>
	public bool? IsAnswered { get; set; }

	/// <summary>
	/// Items per page (default: 20)
	/// </summary>
	public int PageSize { get; set; } = 20;
}
