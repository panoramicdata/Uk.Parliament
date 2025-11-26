namespace Uk.Parliament.Models.ErskineMay;

/// <summary>
/// Represents a part of Erskine May (the parliamentary procedure reference)
/// </summary>
public class ErskineMayPart
{
	/// <summary>
	/// Part number
	/// </summary>
	[JsonPropertyName("number")]
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
	/// Chapters in this part
	/// </summary>
	[JsonPropertyName("chapters")]
	public List<ErskineMayChapter>? Chapters { get; set; }
}
