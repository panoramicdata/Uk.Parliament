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
	public string? SectionNumber { get; set; }

	/// <summary>
	/// Chapter number this section belongs to
	/// </summary>
	[JsonPropertyName("chapterNumber")]
	public int ChapterNumber { get; set; }

	/// <summary>
	/// Section title/heading
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// Title chain (breadcrumb of titles)
	/// </summary>
	[JsonPropertyName("titleChain")]
	public string? TitleChain { get; set; }

	/// <summary>
	/// Section content/text
	/// </summary>
	[JsonPropertyName("content")]
	public string? Content { get; set; }

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

	/// <summary>
	/// Subsections within this section
	/// </summary>
	[JsonPropertyName("subSections")]
	public List<ErskineMaySection>? SubSections { get; set; }
}
