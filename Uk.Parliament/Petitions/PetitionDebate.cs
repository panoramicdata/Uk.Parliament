using System;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Petitions;

/// <summary>
/// Petition debate
/// </summary>
public class PetitionDebate
{
	/// <summary>
	/// The date on which the petition was debated
	/// </summary>
	[JsonPropertyName("debated_on")]
	public string DebatedOn { get; set; }

	/// <summary>
	/// The transcript URL
	/// </summary>
	[JsonPropertyName("transcript_url")]
	public string TranscriptUrl { get; set; }

	/// <summary>
	/// The video URL
	/// </summary>
	[JsonPropertyName("video_url")]
	public string VideoUrl { get; set; }

	/// <summary>
	/// The debate pack URL
	/// </summary>
	[JsonPropertyName("debate_pack_url")]
	public string DebatePackUrl { get; set; }

	/// <summary>
	/// The public engagement URL
	/// </summary>
	[JsonPropertyName("public_engagement_url")]
	public string PublicEngagementUrl { get; set; }

	/// <summary>
	/// The debate summary URL
	/// </summary>
	[JsonPropertyName("debate_summary_url")]
	public string DebateSummaryUrl { get; set; }

	/// <summary>
	/// The overview
	/// </summary>
	[JsonPropertyName("overview")]
	public string Overview { get; set; }
}