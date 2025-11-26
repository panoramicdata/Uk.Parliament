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
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Name { get; set; }

	/// <summary>
	/// Organization abbreviation/acronym
	/// </summary>
	[JsonPropertyName("abbreviation")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Abbreviation { get; set; }

	/// <summary>
	/// Whether the organization is currently active
	/// </summary>
	[JsonPropertyName("isActive")]
	public bool IsActive { get; set; }
}
