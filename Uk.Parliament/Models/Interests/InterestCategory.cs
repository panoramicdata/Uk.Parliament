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
	/// Category number (e.g., "1", "2a")
	/// </summary>
	[JsonPropertyName("number")]
	public string? Number { get; set; }

	/// <summary>
	/// Category description
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }

	/// <summary>
	/// Parent category IDs
	/// </summary>
	[JsonPropertyName("parentCategoryIds")]
	public List<int>? ParentCategoryIds { get; set; }

	/// <summary>
	/// Category type (Commons/Lords)
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; set; }

	/// <summary>
	/// Links related to this category
	/// </summary>
	[JsonPropertyName("links")]
	public List<object>? Links { get; set; }

	/// <summary>
	/// Sort order for display
	/// </summary>
	[JsonPropertyName("sortOrder")]
	public int? SortOrder { get; set; }
}
