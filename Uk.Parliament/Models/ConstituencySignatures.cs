namespace Uk.Parliament.Models;

/// <summary>
/// Constituency signatures
/// </summary>
public class ConstituencySignatures
{
	/// <summary>
	/// The constituency name
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; set; }

	/// <summary>
	/// The ONS code
	/// </summary>
	[JsonPropertyName("ons_code")]
	public required string OnsCode { get; set; }

	/// <summary>
	/// The member of parliament
	/// </summary>
	[JsonPropertyName("mp")]
	public required string Mp { get; set; }

	/// <summary>
	/// The signature count
	/// </summary>
	[JsonPropertyName("signature_count")]
	public required int SignatureCount { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Name}: {SignatureCount} signatures";
}
