using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models;

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
	public required string RespondedOn { get; set; }

	/// <summary>
	/// The summary
	/// </summary>
	[JsonPropertyName("summary")]
	public required string Summary { get; set; }

	/// <summary>
	/// The details
	/// </summary>
	[JsonPropertyName("details")]
	public required string Details { get; set; }

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