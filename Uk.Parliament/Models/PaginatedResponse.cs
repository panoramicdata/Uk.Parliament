using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models;

/// <summary>
/// Generic paginated response wrapper for Members API and other APIs
/// </summary>
/// <typeparam name="T">The type of items in the response</typeparam>
public class PaginatedResponse<T>
{
	/// <summary>
	/// Total number of results available
	/// </summary>
	[JsonPropertyName("totalResults")]
	public int TotalResults { get; set; }

	/// <summary>
	/// Result context information
	/// </summary>
	[JsonPropertyName("resultContext")]
	public string? ResultContext { get; set; }

	/// <summary>
	/// Number of results skipped
	/// </summary>
	[JsonPropertyName("skip")]
	public int Skip { get; set; }

	/// <summary>
	/// Number of results taken
	/// </summary>
	[JsonPropertyName("take")]
	public int Take { get; set; }

	/// <summary>
	/// Paging information (alternative to skip/take in some APIs)
	/// </summary>
	[JsonPropertyName("PagingInfo")]
	public PagingInfo? PagingInfo { get; set; }

	/// <summary>
	/// The list of items with their values
	/// </summary>
	[JsonPropertyName("items")]
	public List<ValueWrapper<T>> Items { get; set; } = new();

	/// <summary>
	/// The list of results (used by Questions/Statements API - also uses ValueWrapper)
	/// </summary>
	[JsonPropertyName("results")]
	public List<ValueWrapper<T>>? Results { get; set; }

	/// <summary>
	/// Links for pagination
	/// </summary>
	[JsonPropertyName("links")]
	public List<Link>? Links { get; set; }

	/// <summary>
	/// Result type information
	/// </summary>
	[JsonPropertyName("resultType")]
	public string? ResultType { get; set; }
}

/// <summary>
/// Paging information for paginated responses
/// </summary>
public class PagingInfo
{
	/// <summary>
	/// Number of results skipped
	/// </summary>
	[JsonPropertyName("skip")]
	public int Skip { get; set; }

	/// <summary>
	/// Number of results taken
	/// </summary>
	[JsonPropertyName("take")]
	public int Take { get; set; }
}

/// <summary>
/// Wrapper for value objects in Members API responses
/// </summary>
/// <typeparam name="T">The type of the value</typeparam>
public class ValueWrapper<T>
{
	/// <summary>
	/// The actual value
	/// </summary>
	[JsonPropertyName("value")]
	public T Value { get; set; } = default!;

	/// <summary>
	/// Links related to this item
	/// </summary>
	[JsonPropertyName("links")]
	public List<Link>? Links { get; set; }
}

/// <summary>
/// Represents a hyperlink
/// </summary>
public class Link
{
	/// <summary>
	/// Link relationship type
	/// </summary>
	[JsonPropertyName("rel")]
	public string? Rel { get; set; }

	/// <summary>
	/// Link URL
	/// </summary>
	[JsonPropertyName("href")]
	public string? Href { get; set; }

	/// <summary>
	/// Link method (GET, POST, etc.)
	/// </summary>
	[JsonPropertyName("method")]
	public string? Method { get; set; }
}
