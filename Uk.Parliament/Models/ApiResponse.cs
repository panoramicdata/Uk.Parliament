using System.Text.Json.Serialization;

namespace Uk.Parliament.Models;

/// <summary>
/// Parliament API response wrapper
/// </summary>
public class ParliamentApiResponse<T>
{
	/// <summary>
	/// Links
	/// </summary>
	[JsonPropertyName("links")]
	public required Links Links { get; set; }

	/// <summary>
	/// The data
	/// </summary>
	[JsonPropertyName("data")]
	public required T Data { get; set; }
}