using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Uk.Parliament.Interfaces;

namespace Uk.Parliament.Petitions;

/// <summary>
/// A petition resource
/// </summary>
[DataContract(Name = "petition")]
public class Petition : Resource
{
	/// <summary>
	/// Constructor
	/// </summary>
	public Petition() : base("petitions")
	{
	}

	/// <summary>
	///  The petition attributes
	/// </summary>
	[JsonPropertyName("attributes")]
	public PetitionAttributes Attributes { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Id}: {Attributes}";
}
