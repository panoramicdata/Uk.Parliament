namespace Uk.Parliament.Models;

/// <summary>
///  Response links
/// </summary>
public class Links
{
	/// <summary>
	/// Self link
	/// </summary>
	[JsonPropertyName("self")]
	public string? Self { get; set; }

	/// <summary>
	/// First link
	/// </summary>
	[JsonPropertyName("first")]
	public string? First { get; set; }

	/// <summary>
	/// Last link
	/// </summary>
	[JsonPropertyName("last")]
	public string? Last { get; set; }

	/// <summary>
	/// Next link
	/// </summary>
	[JsonPropertyName("next")]
	public string? Next { get; set; }

	/// <summary>
	/// Previous link
	/// </summary>
	[JsonPropertyName("prev")]
	public string? Previous { get; set; }
}