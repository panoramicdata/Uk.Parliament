using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Bills;

/// <summary>
/// Response wrapper for Bills API list requests
/// </summary>
/// <typeparam name="T">The type of items in the response</typeparam>
public class BillsListResponse<T>
{
	/// <summary>
	/// List of items
	/// </summary>
	[JsonPropertyName("items")]
	public List<T> Items { get; set; } = new();

	/// <summary>
	/// Total number of results available
	/// </summary>
	[JsonPropertyName("totalResults")]
	public int TotalResults { get; set; }

	/// <summary>
	/// Number of items per page
	/// </summary>
	[JsonPropertyName("itemsPerPage")]
	public int ItemsPerPage { get; set; }
}
