using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Petitions;

/// <summary>
/// A government response to a petition
/// </summary>
[DataContract]
public class PetitionGovernmentResponse
{
	/// <summary>
	/// The summary
	/// </summary>
	[JsonPropertyName("summary")]
	public string Summary { get; set; }

	/// <summary>
	/// The details
	/// </summary>
	[JsonPropertyName("details")]
	public string Details { get; set; }

	/// <summary>
	/// When the response was created
	/// </summary>
	[JsonPropertyName("created_at")]
	public string CreatedAtUtc { get; set; }

	/// <summary>
	/// When the response was updated
	/// </summary>
	[JsonPropertyName("updated_at")]
	public string UpdatedAtUtc { get; set; }
}