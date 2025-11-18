using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.ErskineMay;

/// <summary>
/// Represents a section within an Erskine May chapter
/// </summary>
public class ErskineMaySection
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
	public required string SectionNumber { get; set; }

	/// <summary>
	/// Chapter number this section belongs to
	/// </summary>
	[JsonPropertyName("chapterNumber")]
	public int ChapterNumber { get; set; }

	/// <summary>
	/// Section title/heading
	/// </summary>
	[JsonPropertyName("title")]
	public required string Title { get; set; }

	/// <summary>
	/// Section content/text
	/// </summary>
	[JsonPropertyName("content")]
	public required string Content { get; set; }

	/// <summary>
	/// References to other sections
	/// </summary>
	[JsonPropertyName("crossReferences")]
	public string? CrossReferences { get; set; }

	/// <summary>
	/// Whether this section has subsections
	/// </summary>
	[JsonPropertyName("hasSubsections")]
	public bool HasSubsections { get; set; }
}
