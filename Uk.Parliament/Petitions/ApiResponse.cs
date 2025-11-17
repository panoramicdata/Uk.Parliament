using System.Text.Json.Serialization;

namespace Uk.Parliament.Petitions;

/// <summary>
/// An API response
/// </summary>
public class ApiResponse<T>
{
	/// <summary>
	/// Links
	/// </summary>
	[JsonPropertyName("links")]
	public Links Links { get; set; }

	/// <summary>
	/// The data
	/// </summary>
	[JsonPropertyName("data")]
	public T Data { get; set; }
}