using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Treaties;

/// <summary>
/// Represents a government organization responsible for treaties
/// </summary>
public class GovernmentOrganisation
{
	/// <summary>
	/// Organization identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Organization name
	/// </summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	/// <summary>
	/// Organization abbreviation/acronym
	/// </summary>
	[JsonPropertyName("abbreviation")]
	public string? Abbreviation { get; set; }

	/// <summary>
	/// Whether the organization is currently active
	/// </summary>
	[JsonPropertyName("isActive")]
	public bool IsActive { get; set; }
}
