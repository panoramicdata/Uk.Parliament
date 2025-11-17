using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Uk.Parliament.Models;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// An API resource
/// </summary>
/// <remarks>
///  Constructor
/// </remarks>
/// <param name="baseEndpoint"></param>
[KnownType(typeof(Petition))]
public abstract class Resource(string baseEndpoint)
{

	/// <summary>
	///  The Id
	/// </summary>
	[JsonPropertyName("id")]
	public required int Id { get; set; }

	/// <summary>
	///  The links
	/// </summary>
	[JsonPropertyName("links")]
	public required Links Links { get; set; }

	/// <summary>
	///  The API endpoint
	/// </summary>
	public string Endpoint => $"{baseEndpoint}{(Id == 0 ? string.Empty : $"/{Id}")}.json";
}