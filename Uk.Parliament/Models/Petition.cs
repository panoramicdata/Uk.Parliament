using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models;

/// <summary>
/// A petition resource
/// </summary>
[DataContract(Name = "petition")]
public class Petition
{
	/// <summary>
	///  The Id
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// The resource type (always "petition")
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///  The petition attributes
	/// </summary>
	[JsonPropertyName("attributes")]
	public required PetitionAttributes Attributes { get; set; }

	/// <summary>
	///  The links
	/// </summary>
	[JsonPropertyName("links")]
	public Links? Links { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Id}: {Attributes}";
}
