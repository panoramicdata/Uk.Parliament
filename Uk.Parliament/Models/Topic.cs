using System.Text.Json.Serialization;

namespace Uk.Parliament.Models;

/// <summary>
/// Topic classification for a petition
/// </summary>
public class Topic
{
	/// <summary>
	/// Topic code
	/// </summary>
	[JsonPropertyName("code")]
	public required string Code { get; set; }

	/// <summary>
	/// Topic name
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Code}: {Name}";
}
