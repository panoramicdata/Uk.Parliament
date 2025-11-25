using System;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Treaties;

/// <summary>
/// Represents a business item related to a treaty (debates, votes, etc.)
/// </summary>
public class TreatyBusinessItem
{
	/// <summary>
	/// Business item identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Treaty identifier this business item relates to
	/// </summary>
	[JsonPropertyName("treatyId")]
	public string TreatyId { get; set; } = string.Empty;

	/// <summary>
	/// Type of business item (e.g., "Debate", "Motion", "Vote")
	/// </summary>
	[JsonPropertyName("businessItemType")]
	public string? BusinessItemType { get; set; }

	/// <summary>
	/// Date of the business item
	/// </summary>
	[JsonPropertyName("date")]
	public DateTime Date { get; set; }

	/// <summary>
	/// House where business item occurred
	/// </summary>
	[JsonPropertyName("house")]
	public string? House { get; set; }

	/// <summary>
	/// Description/title of the business item
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }

	/// <summary>
	/// Link to Hansard or other record
	/// </summary>
	[JsonPropertyName("link")]
	public string? Link { get; set; }
}
