namespace Uk.Parliament.Models.Now;

/// <summary>
/// Represents a scrolling message displayed on the annunciator
/// </summary>
public class ScrollingMessage
{
	/// <summary>
	/// Message identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Vertical alignment of the message
	/// </summary>
	[JsonPropertyName("verticalAlignment")]
	public string? VerticalAlignment { get; set; }

	/// <summary>
	/// The content/text of the scrolling message
	/// </summary>
	[JsonPropertyName("content")]
	public string? Content { get; set; }

	/// <summary>
	/// When the message should start being displayed
	/// </summary>
	[JsonPropertyName("displayFrom")]
	public DateTime? DisplayFrom { get; set; }

	/// <summary>
	/// When the message should stop being displayed
	/// </summary>
	[JsonPropertyName("displayTo")]
	public DateTime? DisplayTo { get; set; }

	/// <summary>
	/// The type of alert (e.g., "Standard")
	/// </summary>
	[JsonPropertyName("alertType")]
	public string? AlertType { get; set; }
}
