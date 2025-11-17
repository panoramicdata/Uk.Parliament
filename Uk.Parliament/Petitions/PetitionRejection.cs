using System.Text.Json.Serialization;

namespace Uk.Parliament.Petitions;

/// <summary>
/// Petition rejection details
/// </summary>
public class PetitionRejection
{
	/// <summary>
	/// The rejection code
	/// </summary>
	[JsonPropertyName("code")]
	public string Code { get; set; }

	/// <summary>
	/// The rejection details
	/// </summary>
	[JsonPropertyName("details")]
	public string Details { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Code}: {Details}";
}
