using System;
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
	/// The date the government responded
	/// </summary>
	[JsonPropertyName("responded_on")]
	public string RespondedOn { get; set; }

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
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// When the response was updated
	/// </summary>
	[JsonPropertyName("updated_at")]
	public DateTime UpdatedAt { get; set; }
}