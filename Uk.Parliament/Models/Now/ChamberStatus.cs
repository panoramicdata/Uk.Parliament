namespace Uk.Parliament.Models.Now;

/// <summary>
/// Represents the current status of a chamber (Commons or Lords)
/// </summary>
public class ChamberStatus
{
	/// <summary>
	/// House identifier (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public required string House { get; set; }

	/// <summary>
	/// Whether the house is currently sitting
	/// </summary>
	[JsonPropertyName("isSitting")]
	public bool IsSitting { get; set; }

	/// <summary>
	/// Current session date
	/// </summary>
	[JsonPropertyName("sessionDate")]
	public DateTime? SessionDate { get; set; }

	/// <summary>
	/// Time the sitting started
	/// </summary>
	[JsonPropertyName("startTime")]
	public DateTime? StartTime { get; set; }

	/// <summary>
	/// Expected end time
	/// </summary>
	[JsonPropertyName("expectedEndTime")]
	public DateTime? ExpectedEndTime { get; set; }

	/// <summary>
	/// Current business being discussed
	/// </summary>
	[JsonPropertyName("currentBusiness")]
	public string? CurrentBusiness { get; set; }

	/// <summary>
	/// Next business item
	/// </summary>
	[JsonPropertyName("nextBusiness")]
	public string? NextBusiness { get; set; }

	/// <summary>
	/// Whether chamber is in recess
	/// </summary>
	[JsonPropertyName("isInRecess")]
	public bool IsInRecess { get; set; }

	/// <summary>
	/// Link to live stream if available
	/// </summary>
	[JsonPropertyName("liveStreamUrl")]
	public string? LiveStreamUrl { get; set; }
}
