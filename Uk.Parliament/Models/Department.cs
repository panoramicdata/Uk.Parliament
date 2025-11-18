using System.Text.Json.Serialization;

namespace Uk.Parliament.Models;

/// <summary>
/// Government department
/// </summary>
public class Department
{
	/// <summary>
	/// Department acronym
	/// </summary>
	[JsonPropertyName("acronym")]
	public required string Acronym { get; set; }

	/// <summary>
	/// Department name
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; set; }

	/// <summary>
	/// Department URL
	/// </summary>
	[JsonPropertyName("url")]
	public required string Url { get; set; }
	/// <inheritdoc />
	public override string ToString() => $"{Acronym}: {Name}";
}
