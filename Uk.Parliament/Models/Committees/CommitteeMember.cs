using System;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Committees;

/// <summary>
/// Represents a member of a parliamentary committee
/// </summary>
public class CommitteeMember
{
	/// <summary>
	/// Member identifier
	/// </summary>
	[JsonPropertyName("memberId")]
	public int MemberId { get; set; }

	/// <summary>
	/// Member name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Party affiliation
	/// </summary>
	[JsonPropertyName("party")]
	public string? Party { get; set; }

	/// <summary>
	/// Role on the committee
	/// </summary>
	[JsonPropertyName("role")]
	public string? Role { get; set; }

	/// <summary>
	/// Start date on committee
	/// </summary>
	[JsonPropertyName("startDate")]
	public DateTime? StartDate { get; set; }

	/// <summary>
	/// End date on committee (null if current)
	/// </summary>
	[JsonPropertyName("endDate")]
	public DateTime? EndDate { get; set; }
}
