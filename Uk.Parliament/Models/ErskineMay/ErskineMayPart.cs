using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.ErskineMay;

/// <summary>
/// Represents a part of Erskine May (the parliamentary procedure reference)
/// </summary>
public class ErskineMayPart
{
	/// <summary>
	/// Part number
	/// </summary>
	[JsonPropertyName("partNumber")]
	public int PartNumber { get; set; }

	/// <summary>
	/// Part title
	/// </summary>
	[JsonPropertyName("title")]
	public required string Title { get; set; }

	/// <summary>
	/// Part description
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }

	/// <summary>
	/// Number of chapters in this part
	/// </summary>
	[JsonPropertyName("chapterCount")]
	public int ChapterCount { get; set; }
}
