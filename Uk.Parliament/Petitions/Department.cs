using System.Text.Json.Serialization;

namespace Uk.Parliament.Petitions;

/// <summary>
/// Government department
/// </summary>
public class Department
{
	/// <summary>
	/// Department acronym
	/// </summary>
	[JsonPropertyName("acronym")]
	public string Acronym { get; set; }

	/// <summary>
	/// Department name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>
	/// Department URL
	/// </summary>
	[JsonPropertyName("url")]
	public string Url { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Acronym}: {Name}";
}
