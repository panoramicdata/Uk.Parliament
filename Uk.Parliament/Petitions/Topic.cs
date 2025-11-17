using System.Text.Json.Serialization;

namespace Uk.Parliament.Petitions;

/// <summary>
/// Topic classification for a petition
/// </summary>
public class Topic
{
	/// <summary>
	/// Topic code
	/// </summary>
	[JsonPropertyName("code")]
	public string Code { get; set; }

	/// <summary>
	/// Topic name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Code}: {Name}";
}
