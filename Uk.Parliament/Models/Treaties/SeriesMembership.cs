namespace Uk.Parliament.Models.Treaties;

/// <summary>
/// Represents a treaty series membership category
/// </summary>
public class SeriesMembership
{
	/// <summary>
	/// Series membership identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Series description (e.g. "Country series", "European Union series")
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }
}
