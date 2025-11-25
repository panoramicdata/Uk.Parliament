using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Interests;

/// <summary>
/// Paginated response for Interests API
/// </summary>
/// <typeparam name="T">Type of items</typeparam>
public class InterestsResponse<T>
{
	/// <summary>
	/// Links for pagination
	/// </summary>
	[JsonPropertyName("links")]
	public List<InterestLink> Links { get; set; } = new();

	/// <summary>
	/// Number of items skipped
	/// </summary>
	[JsonPropertyName("skip")]
	public int Skip { get; set; }

	/// <summary>
	/// Number of items to take
	/// </summary>
	[JsonPropertyName("take")]
	public int Take { get; set; }

	/// <summary>
	/// Total number of results
	/// </summary>
	[JsonPropertyName("totalResults")]
	public int TotalResults { get; set; }

	/// <summary>
	/// Items in this page
	/// </summary>
	[JsonPropertyName("items")]
	public List<T> Items { get; set; } = new();
}

/// <summary>
/// Link in Interests API response
/// </summary>
public class InterestLink
{
	/// <summary>
	/// Link relation type
	/// </summary>
	[JsonPropertyName("rel")]
	public string? Rel { get; set; }

	/// <summary>
	/// Link href
	/// </summary>
	[JsonPropertyName("href")]
	public string? Href { get; set; }

	/// <summary>
	/// HTTP method
	/// </summary>
	[JsonPropertyName("method")]
	public string? Method { get; set; }
}
