using System;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.OralQuestions;

/// <summary>
/// Represents an oral parliamentary question
/// </summary>
public class OralQuestion
{
	/// <summary>
	/// Question identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Unique Identifier Number (UIN)
	/// </summary>
	[JsonPropertyName("uin")]
	public required string Uin { get; set; }

	/// <summary>
	/// Member who asked the question
	/// </summary>
	[JsonPropertyName("askingMemberId")]
	public int AskingMemberId { get; set; }

	/// <summary>
	/// Name of the member who asked
	/// </summary>
	[JsonPropertyName("askingMember")]
	public string? AskingMember { get; set; }

	/// <summary>
	/// House where question was asked (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public required string House { get; set; }

	/// <summary>
	/// Department responsible for answering
	/// </summary>
	[JsonPropertyName("answeringDepartment")]
	public string? AnsweringDepartment { get; set; }

	/// <summary>
	/// Date the question was asked
	/// </summary>
	[JsonPropertyName("dateAsked")]
	public DateTime DateAsked { get; set; }

	/// <summary>
	/// Question text
	/// </summary>
	[JsonPropertyName("questionText")]
	public required string QuestionText { get; set; }

	/// <summary>
	/// Type of oral question (e.g., "Topical", "Named Day")
	/// </summary>
	[JsonPropertyName("questionType")]
	public string? QuestionType { get; set; }

	/// <summary>
	/// Whether the question has been answered
	/// </summary>
	[JsonPropertyName("isAnswered")]
	public bool IsAnswered { get; set; }

	/// <summary>
	/// Whether the question was withdrawn
	/// </summary>
	[JsonPropertyName("isWithdrawn")]
	public bool IsWithdrawn { get; set; }

	/// <summary>
	/// Related document URL
	/// </summary>
	[JsonPropertyName("documentUrl")]
	public string? DocumentUrl { get; set; }

	/// <summary>
	/// Hansard reference
	/// </summary>
	[JsonPropertyName("hansardReference")]
	public string? HansardReference { get; set; }
}
