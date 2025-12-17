namespace Uk.Parliament.Models.Questions;

/// <summary>
/// Represents a daily report of parliamentary questions and statements
/// </summary>
public class DailyReport
{
	/// <summary>
	/// Report identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Date of the report
	/// </summary>
	[JsonPropertyName("date")]
	public DateTime Date { get; set; }

	/// <summary>
	/// House (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public string House { get; set; } = string.Empty;

	/// <summary>
	/// Report title
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// Number of questions in the report
	/// </summary>
	[JsonPropertyName("questionCount")]
	public int QuestionCount { get; set; }

	/// <summary>
	/// Number of statements in the report
	/// </summary>
	[JsonPropertyName("statementCount")]
	public int StatementCount { get; set; }

	/// <summary>
	/// URL to the full report document
	/// </summary>
	[JsonPropertyName("documentUrl")]
	public string? DocumentUrl { get; set; }

	/// <summary>
	/// URL to the HTML version
	/// </summary>
	[JsonPropertyName("htmlUrl")]
	public string? HtmlUrl { get; set; }

	/// <summary>
	/// URL to the report (alternative to documentUrl)
	/// </summary>
	[JsonPropertyName("url")]
	public string? Url { get; set; }

	/// <summary>
	/// Whether the report has been published
	/// </summary>
	[JsonPropertyName("isPublished")]
	public bool IsPublished { get; set; }

	/// <summary>
	/// File size in bytes (for downloadable reports)
	/// </summary>
	[JsonPropertyName("fileSizeBytes")]
	public long? FileSizeBytes { get; set; }
}
