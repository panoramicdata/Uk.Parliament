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
	[JsonPropertyName("summary")]
	public DateTime DebatedOnDate { get; set; }

	/// <summary>
	/// The transcript url
	/// </summary>
	[JsonPropertyName("transcript_url")]
	public string TranscriptUrl { get; set; }

	/// <summary>
	/// The video_url
	/// </summary>
	[JsonPropertyName("video_url")]
	public string VideoUrl { get; set; }

	/// <summary>
	/// The overview
	/// </summary>
	[JsonPropertyName("overview")]
	public string Overview { get; set; }
}