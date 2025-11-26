namespace Uk.Parliament.Models.ErskineMay;

/// <summary>
/// Search response from Erskine May API
/// </summary>
public class ErskineMaySearchResponse
{
	/// <summary>
	/// Search results
	/// </summary>
	[JsonPropertyName("searchResults")]
	public List<ErskineMaySearchResult> SearchResults { get; set; } = [];

	/// <summary>
	/// Search term used
	/// </summary>
	[JsonPropertyName("searchTerm")]
	public string? SearchTerm { get; set; }

	/// <summary>
	/// Search terms
	/// </summary>
	[JsonPropertyName("searchTerms")]
	public List<string>? SearchTerms { get; set; }

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
	/// Total results available
	/// </summary>
	[JsonPropertyName("totalResults")]
	public int TotalResults { get; set; }

	/// <summary>
	/// Suggested search term
	/// </summary>
	[JsonPropertyName("suggestedSearch")]
	public string? SuggestedSearch { get; set; }
}
