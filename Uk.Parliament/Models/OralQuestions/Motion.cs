using System;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.OralQuestions;

/// <summary>
/// Represents a parliamentary motion
/// </summary>
public class Motion
{
	/// <summary>
	/// Motion identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Reference number for the motion
	/// </summary>
	[JsonPropertyName("reference")]
	public string? Reference { get; set; }

	/// <summary>
	/// Member who proposed the motion
	/// </summary>
	[JsonPropertyName("proposingMemberId")]
	public int ProposingMemberId { get; set; }

	/// <summary>
	/// Name of the member who proposed
	/// </summary>
	[JsonPropertyName("proposingMember")]
	public string? ProposingMember { get; set; }

	/// <summary>
	/// House where motion was proposed (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public string? House { get; set; }

	/// <summary>
	/// Date the motion was tabled
	/// </summary>
	[JsonPropertyName("dateTabled")]
	public DateTime DateTabled { get; set; }

	/// <summary>
	/// Motion title/subject
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// Motion text/content
	/// </summary>
	[JsonPropertyName("motionText")]
	public string? MotionText { get; set; }

	/// <summary>
	/// Type of motion (e.g., "Early Day Motion", "Amendment")
	/// </summary>
	[JsonPropertyName("motionType")]
	public string? MotionType { get; set; }

	/// <summary>
	/// Number of signatures/supporters
	/// </summary>
	[JsonPropertyName("signatureCount")]
	public int SignatureCount { get; set; }

	/// <summary>
	/// Whether the motion is still active
	/// </summary>
	[JsonPropertyName("isActive")]
	public bool IsActive { get; set; }

	/// <summary>
	/// Whether the motion was withdrawn
	/// </summary>
	[JsonPropertyName("isWithdrawn")]
	public bool IsWithdrawn { get; set; }

	/// <summary>
	/// Motion status (e.g., "Active", "Closed", "Withdrawn")
	/// </summary>
	[JsonPropertyName("Status")]
	public string? Status { get; set; }

	/// <summary>
	/// Related document URL
	/// </summary>
	[JsonPropertyName("documentUrl")]
	public string? DocumentUrl { get; set; }
}
