using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Members;

/// <summary>
/// Represents a political party
/// </summary>
public class Party
{
	/// <summary>
	/// The party's unique identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// The party name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Abbreviated party name
	/// </summary>
	[JsonPropertyName("abbreviation")]
	public string? Abbreviation { get; set; }

	/// <summary>
	/// Background color for the party (hex code)
	/// </summary>
	[JsonPropertyName("backgroundColour")]
	public string? BackgroundColour { get; set; }

	/// <summary>
	/// Foreground color for the party (hex code)
	/// </summary>
	[JsonPropertyName("foregroundColour")]
	public string? ForegroundColour { get; set; }

	/// <summary>
	/// Whether the party is a Lords main party
	/// </summary>
	[JsonPropertyName("isLordsMainParty")]
	public bool? IsLordsMainParty { get; set; }

	/// <summary>
	/// Whether the party is a Lords spiritual party
	/// </summary>
	[JsonPropertyName("isLordsSpiritualParty")]
	public bool? IsLordsSpiritualParty { get; set; }

	/// <summary>
	/// Government type classification
	/// </summary>
	[JsonPropertyName("governmentType")]
	public int? GovernmentType { get; set; }

	/// <summary>
	/// Whether the party is independent (singular)
	/// </summary>
	[JsonPropertyName("isIndependentParty")]
	public bool? IsIndependentParty { get; set; }
}
