using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Interests;

/// <summary>
/// Represents a category of interest in the Register of Members' Financial Interests
/// </summary>
public class InterestCategory
{
	/// <summary>
	/// Category identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Category name
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; set; }

	/// <summary>
	/// Category description
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }

	/// <summary>
	/// Sort order for display
	/// </summary>
	[JsonPropertyName("sortOrder")]
	public int? SortOrder { get; set; }
}
