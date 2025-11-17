using System.Text.Json.Serialization;

namespace Uk.Parliament.Petitions;

/// <summary>
/// Constituency signatures
/// </summary>
public class ConstituencySignatures
{
	/// <summary>
	/// The constituency name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>
	/// The ONS code
	/// </summary>
	[JsonPropertyName("ons_code")]
	public string OnsCode { get; set; }

	/// <summary>
	/// The member of parliament
	/// </summary>
	[JsonPropertyName("mp")]
	public string Mp { get; set; }

	/// <summary>
	/// The signature count
	/// </summary>
	[JsonPropertyName("signature_count")]
	public int SignatureCount { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Name}: {SignatureCount} signatures";
}
