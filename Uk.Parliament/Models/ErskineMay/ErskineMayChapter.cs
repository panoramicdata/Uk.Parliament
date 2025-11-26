namespace Uk.Parliament.Models.ErskineMay;

/// <summary>
/// Represents a chapter within Erskine May
/// </summary>
public class ErskineMayChapter
{
	/// <summary>
	/// Part number this chapter belongs to
	/// </summary>
	[JsonPropertyName("partNumber")]
	public int PartNumber { get; set; }

	/// <summary>
	/// Chapter number
	/// </summary>
	[JsonPropertyName("number")]
	public int ChapterNumber { get; set; }

	/// <summary>
	/// Chapter title
	/// </summary>
	[JsonPropertyName("title")]
	public required string Title { get; set; }

	/// <summary>
	/// Chapter description
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }

	/// <summary>
	/// Sections in this chapter
	/// </summary>
	[JsonPropertyName("sections")]
	public List<ErskineMaySection>? Sections { get; set; }
}
