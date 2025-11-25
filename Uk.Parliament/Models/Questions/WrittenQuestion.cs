using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Questions;

/// <summary>
/// Represents a written parliamentary question
/// </summary>
public class WrittenQuestion
{
	/// <summary>
	/// Question identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Unique Identifier Number (UIN) for the question
	/// </summary>
	[JsonPropertyName("uin")]
	public required string Uin { get; set; }

	/// <summary>
	/// Member who asked the question
	/// </summary>
	[JsonPropertyName("askingMemberId")]
	public int AskingMemberId { get; set; }

	/// <summary>
	/// Name of the member who asked the question
	/// </summary>
	[JsonPropertyName("askingMember")]
	public string? AskingMember { get; set; }

	/// <summary>
	/// House where question was asked (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public required string House { get; set; }

	/// <summary>
	/// Member who answered the question
	/// </summary>
	[JsonPropertyName("answeringMemberId")]
	public int? AnsweringMemberId { get; set; }

	/// <summary>
	/// Name of the member who answered
	/// </summary>
	[JsonPropertyName("answeringMember")]
	public string? AnsweringMember { get; set; }

	/// <summary>
	/// Department responsible for answering
	/// </summary>
	[JsonPropertyName("answeringDepartment")]
	public string? AnsweringDepartment { get; set; }

	/// <summary>
	/// ID of the answering body/department
	/// </summary>
	[JsonPropertyName("answeringBodyId")]
	public int? AnsweringBodyId { get; set; }

	/// <summary>
	/// Name of the answering body/department
	/// </summary>
	[JsonPropertyName("answeringBodyName")]
	public string? AnsweringBodyName { get; set; }

	/// <summary>
	/// Date the question was tabled
	/// </summary>
	[JsonPropertyName("dateTabled")]
	public DateTime DateTabled { get; set; }

	/// <summary>
	/// Date for answer
	/// </summary>
	[JsonPropertyName("dateForAnswer")]
	public DateTime? DateForAnswer { get; set; }

	/// <summary>
	/// Date the question was answered
	/// </summary>
	[JsonPropertyName("dateAnswered")]
	public DateTime? DateAnswered { get; set; }

	/// <summary>
	/// Question text
	/// </summary>
	[JsonPropertyName("questionText")]
	public required string QuestionText { get; set; }

	/// <summary>
	/// Answer text
	/// </summary>
	[JsonPropertyName("answerText")]
	public string? AnswerText { get; set; }

	/// <summary>
	/// Whether the question has been answered
	/// </summary>
	[JsonPropertyName("isAnswered")]
	public bool IsAnswered { get; set; }

	/// <summary>
	/// Whether the question is named day
	/// </summary>
	[JsonPropertyName("isNamedDay")]
	public bool IsNamedDay { get; set; }

	/// <summary>
	/// Whether the question is withdrawn
	/// </summary>
	[JsonPropertyName("isWithdrawn")]
	public bool IsWithdrawn { get; set; }

	/// <summary>
	/// Whether the asking member has a registered interest
	/// </summary>
	[JsonPropertyName("memberHasInterest")]
	public bool MemberHasInterest { get; set; }

	/// <summary>
	/// Heading/subject of the question
	/// </summary>
	[JsonPropertyName("heading")]
	public string? Heading { get; set; }

	/// <summary>
	/// Related document URL
	/// </summary>
	[JsonPropertyName("documentUrl")]
	public string? DocumentUrl { get; set; }

	/// <summary>
	/// Grouped questions (if this question is grouped with others)
	/// </summary>
	[JsonPropertyName("groupedQuestions")]
	public List<int>? GroupedQuestions { get; set; }
}
