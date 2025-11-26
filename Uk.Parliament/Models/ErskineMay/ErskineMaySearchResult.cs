namespace Uk.Parliament.Models.ErskineMay;

/// <summary>
/// Represents search results from Erskine May
/// </summary>
public class ErskineMaySearchResult
{
	/// <summary>
	/// Section identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Section ID (alternative property name)
	/// </summary>
	[JsonPropertyName("sectionId")]
	public int? SectionId { get; set; }

	/// <summary>
	/// Section number
	/// </summary>
	[JsonPropertyName("sectionNumber")]
	public string? SectionNumber { get; set; }

	/// <summary>
	/// Section title
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// Section title (alternative property name)
	/// </summary>
	[JsonPropertyName("sectionTitle")]
	public string? SectionTitle { get; set; }

	/// <summary>
	/// Section title chain (breadcrumb of section titles)
	/// </summary>
	[JsonPropertyName("sectionTitleChain")]
	public string? SectionTitleChain { get; set; }

	/// <summary>
	/// Content excerpt/snippet
	/// </summary>
	[JsonPropertyName("excerpt")]
	public string? Excerpt { get; set; }

	/// <summary>
	/// Part title
	/// </summary>
	[JsonPropertyName("partTitle")]
	public string? PartTitle { get; set; }

	/// <summary>
	/// Part number
	/// </summary>
	[JsonPropertyName("partNumber")]
	public int? PartNumber { get; set; }

	/// <summary>
	/// Chapter title
	/// </summary>
	[JsonPropertyName("chapterTitle")]
	public string? ChapterTitle { get; set; }

	/// <summary>
	/// Chapter number
	/// </summary>
	[JsonPropertyName("chapterNumber")]
	public int? ChapterNumber { get; set; }

	/// <summary>
	/// Relevance score
	/// </summary>
	[JsonPropertyName("score")]
	public double Score { get; set; }
}
