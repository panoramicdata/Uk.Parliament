namespace Uk.Parliament.Models.OralQuestions;

/// <summary>
/// Response wrapper for Oral Questions and Motions API requests
/// </summary>
/// <typeparam name="T">The type of items in the response</typeparam>
public class OralQuestionsResponse<T>
{
	/// <summary>
	/// Paging information
	/// </summary>
	[JsonPropertyName("PagingInfo")]
	public OralQuestionsPagingInfo PagingInfo { get; set; } = new();

	/// <summary>
	/// HTTP status code
	/// </summary>
	[JsonPropertyName("StatusCode")]
	public int StatusCode { get; set; }

	/// <summary>
	/// Whether the request was successful
	/// </summary>
	[JsonPropertyName("Success")]
	public bool Success { get; set; }

	/// <summary>
	/// List of errors (if any)
	/// </summary>
	[JsonPropertyName("Errors")]
	public List<string> Errors { get; set; } = [];

	/// <summary>
	/// The response data
	/// </summary>
	[JsonPropertyName("Response")]
	public List<T> Response { get; set; } = [];
}

/// <summary>
/// Paging information for Oral Questions and Motions API
/// </summary>
public class OralQuestionsPagingInfo
{
	/// <summary>
	/// Number of results skipped
	/// </summary>
	[JsonPropertyName("Skip")]
	public int Skip { get; set; }

	/// <summary>
	/// Number of results taken
	/// </summary>
	[JsonPropertyName("Take")]
	public int Take { get; set; }

	/// <summary>
	/// Total number of results available
	/// </summary>
	[JsonPropertyName("Total")]
	public int Total { get; set; }

	/// <summary>
	/// Global total (usually same as Total)
	/// </summary>
	[JsonPropertyName("GlobalTotal")]
	public int GlobalTotal { get; set; }

	/// <summary>
	/// Status counts (if applicable)
	/// </summary>
	[JsonPropertyName("StatusCounts")]
	public List<object> StatusCounts { get; set; } = [];

	/// <summary>
	/// Global status counts (if applicable)
	/// </summary>
	[JsonPropertyName("GlobalStatusCounts")]
	public List<object> GlobalStatusCounts { get; set; } = [];
}
