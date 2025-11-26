namespace Uk.Parliament.Models.Committees;

/// <summary>
/// Response wrapper for Committees API list requests
/// </summary>
/// <typeparam name="T">The type of items in the response</typeparam>
public class CommitteesListResponse<T>
{
	/// <summary>
	/// List of items
	/// </summary>
	[JsonPropertyName("items")]
	public List<T> Items { get; set; } = [];

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

	/// <summary>
	/// Pagination links (typically empty array)
	/// </summary>
	[JsonPropertyName("links")]
	public List<string>? Links { get; set; }
}
