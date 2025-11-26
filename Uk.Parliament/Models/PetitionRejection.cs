namespace Uk.Parliament.Models;

/// <summary>
/// Petition rejection details
/// </summary>
public class PetitionRejection
{
	/// <summary>
	/// The rejection code
	/// </summary>
	[JsonPropertyName("code")]
	public required string Code { get; set; }

	/// <summary>
	/// The rejection details
	/// </summary>
	[JsonPropertyName("details")]
	public required string Details { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Code}: {Details}";
}
