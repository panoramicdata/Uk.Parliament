namespace Uk.Parliament.Models.OralQuestions;

/// <summary>
/// Represents an oral parliamentary question
/// </summary>
public class OralQuestion
{
	/// <summary>
	/// Question identifier
	/// </summary>
	[JsonPropertyName("Id")]
	public int Id { get; set; }

	/// <summary>
	/// Unique Identifier Number (UIN)
	/// </summary>
	[JsonPropertyName("UIN")]
	public int Uin { get; set; }

	/// <summary>
	/// Question type
	/// </summary>
	[JsonPropertyName("QuestionType")]
	public int QuestionType { get; set; }

	/// <summary>
	/// Question text
	/// </summary>
	[JsonPropertyName("QuestionText")]
	public string QuestionText { get; set; } = string.Empty;

	/// <summary>
	/// Question status
	/// </summary>
	[JsonPropertyName("Status")]
	public int Status { get; set; }

	/// <summary>
	/// Question number
	/// </summary>
	[JsonPropertyName("Number")]
	public int Number { get; set; }

	/// <summary>
	/// Date the question was tabled
	/// </summary>
	[JsonPropertyName("TabledWhen")]
	public DateTime? TabledWhen { get; set; }

	/// <summary>
	/// Date removed from "to be asked"
	/// </summary>
	[JsonPropertyName("RemovedFromToBeAskedWhen")]
	public DateTime? RemovedFromToBeAskedWhen { get; set; }

	/// <summary>
	/// Declarable interest detail
	/// </summary>
	[JsonPropertyName("DeclarableInterestDetail")]
	public string? DeclarableInterestDetail { get; set; }

	/// <summary>
	/// Hansard link
	/// </summary>
	[JsonPropertyName("HansardLink")]
	public string? HansardLink { get; set; }

	/// <summary>
	/// Date when the question will be/was answered
	/// </summary>
	[JsonPropertyName("AnsweringWhen")]
	public DateTime? AnsweringWhen { get; set; }

	/// <summary>
	/// Answering body ID
	/// </summary>
	[JsonPropertyName("AnsweringBodyId")]
	public int? AnsweringBodyId { get; set; }

	/// <summary>
	/// Answering body name
	/// </summary>
	[JsonPropertyName("AnsweringBody")]
	public string? AnsweringBody { get; set; }

	/// <summary>
	/// Answering minister title
	/// </summary>
	[JsonPropertyName("AnsweringMinisterTitle")]
	public string? AnsweringMinisterTitle { get; set; }

	/// <summary>
	/// Member who asked the question
	/// </summary>
	[JsonPropertyName("AskingMember")]
	public OralQuestionMember? AskingMember { get; set; }

	/// <summary>
	/// Minister answering the question
	/// </summary>
	[JsonPropertyName("AnsweringMinister")]
	public OralQuestionMember? AnsweringMinister { get; set; }

	/// <summary>
	/// ID of member who asked
	/// </summary>
	[JsonPropertyName("AskingMemberId")]
	public int AskingMemberId { get; set; }

	/// <summary>
	/// ID of answering minister
	/// </summary>
	[JsonPropertyName("AnsweringMinisterId")]
	public int? AnsweringMinisterId { get; set; }
}

/// <summary>
/// Represents a member in an oral question context
/// </summary>
public class OralQuestionMember
{
	/// <summary>
	/// MNIS member ID
	/// </summary>
	[JsonPropertyName("MnisId")]
	public int MnisId { get; set; }

	/// <summary>
	/// PIMS member ID
	/// </summary>
	[JsonPropertyName("PimsId")]
	public int? PimsId { get; set; }

	/// <summary>
	/// Member name
	/// </summary>
	[JsonPropertyName("Name")]
	public string? Name { get; set; }

	/// <summary>
	/// How the member should be listed
	/// </summary>
	[JsonPropertyName("ListAs")]
	public string? ListAs { get; set; }

	/// <summary>
	/// Member's constituency
	/// </summary>
	[JsonPropertyName("Constituency")]
	public string? Constituency { get; set; }

	/// <summary>
	/// Member status
	/// </summary>
	[JsonPropertyName("Status")]
	public string? Status { get; set; }

	/// <summary>
	/// Member's party
	/// </summary>
	[JsonPropertyName("Party")]
	public string? Party { get; set; }

	/// <summary>
	/// Party ID
	/// </summary>
	[JsonPropertyName("PartyId")]
	public int? PartyId { get; set; }

	/// <summary>
	/// Party colour
	/// </summary>
	[JsonPropertyName("PartyColour")]
	public string? PartyColour { get; set; }

	/// <summary>
	/// Photo URL
	/// </summary>
	[JsonPropertyName("PhotoUrl")]
	public string? PhotoUrl { get; set; }
}
