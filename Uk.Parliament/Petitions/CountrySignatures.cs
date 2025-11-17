using System.Text.Json.Serialization;

namespace Uk.Parliament.Petitions;

/// <summary>
/// Country signatures
/// </summary>
public class CountrySignatures
{
	/// <summary>
	/// Name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>
	/// Code
	/// </summary>
	[JsonPropertyName("code")]
	public string Code { get; set; }

	/// <summary>
	/// Signature count
	/// </summary>
	[JsonPropertyName("signature_count")]
	public int SignatureCount { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Name} ({Code}): {SignatureCount} signatures";
}