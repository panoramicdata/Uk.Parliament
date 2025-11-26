namespace Uk.Parliament.Models;

/// <summary>
/// Country signatures
/// </summary>
public class CountrySignatures
{
	/// <summary>
	/// Country name
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; set; }

	/// <summary>
	/// Country code
	/// </summary>
	[JsonPropertyName("code")]
	public required string Code { get; set; }

	/// <summary>
	/// Signature count
	/// </summary>
	[JsonPropertyName("signature_count")]
	public required int SignatureCount { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Name} ({Code}): {SignatureCount} signatures";
}