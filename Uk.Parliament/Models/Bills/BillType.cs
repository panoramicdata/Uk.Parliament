namespace Uk.Parliament.Models.Bills;

/// <summary>
/// Represents a type of bill
/// </summary>
public class BillType
{
	/// <summary>
	/// Bill type identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Category (e.g., Public, Private, Hybrid)
	/// </summary>
	[JsonPropertyName("category")]
	public string Category { get; set; } = string.Empty;

	/// <summary>
	/// Name of the bill type
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Description of the bill type (may contain HTML)
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }
}
