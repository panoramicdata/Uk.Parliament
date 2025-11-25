using System.Text.Json.Serialization;

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
	/// Chapter title
	/// </summary>
	[JsonPropertyName("chapterTitle")]
	public string? ChapterTitle { get; set; }

	/// <summary>
	/// Relevance score
	/// </summary>
	[JsonPropertyName("score")]
	public double Score { get; set; }
}
