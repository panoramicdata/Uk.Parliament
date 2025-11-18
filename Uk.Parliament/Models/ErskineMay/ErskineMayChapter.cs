using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.ErskineMay;

/// <summary>
/// Represents a chapter within Erskine May
/// </summary>
public class ErskineMayChapter
{
	/// <summary>
	/// Chapter number
	/// </summary>
	[JsonPropertyName("chapterNumber")]
	public int ChapterNumber { get; set; }

	/// <summary>
	/// Part number this chapter belongs to
	/// </summary>
	[JsonPropertyName("partNumber")]
	public int PartNumber { get; set; }

	/// <summary>
	/// Chapter title
	/// </summary>
	[JsonPropertyName("title")]
	public required string Title { get; set; }

	/// <summary>
	/// Chapter summary/introduction
	/// </summary>
	[JsonPropertyName("summary")]
	public string? Summary { get; set; }

	/// <summary>
	/// Number of sections in this chapter
	/// </summary>
	[JsonPropertyName("sectionCount")]
	public int SectionCount { get; set; }
}
