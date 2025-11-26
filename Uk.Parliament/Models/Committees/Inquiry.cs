namespace Uk.Parliament.Models.Committees;

/// <summary>
/// Represents a committee inquiry
/// </summary>
public class Inquiry
{
	/// <summary>
	/// Inquiry identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Inquiry title
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Committee identifier
	/// </summary>
	[JsonPropertyName("committeeId")]
	public int CommitteeId { get; set; }

	/// <summary>
	/// Inquiry start date
	/// </summary>
	[JsonPropertyName("startDate")]
	public DateTime? StartDate { get; set; }

	/// <summary>
	/// Inquiry end date
	/// </summary>
	[JsonPropertyName("endDate")]
	public DateTime? EndDate { get; set; }

	/// <summary>
	/// Inquiry status
	/// </summary>
	[JsonPropertyName("status")]
	public string? Status { get; set; }

	/// <summary>
	/// Inquiry description
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }
}

/// <summary>
/// Represents a publication related to an inquiry
/// </summary>
public class InquiryPublication
{
	/// <summary>
	/// Publication identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Publication title
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Publication type
	/// </summary>
	[JsonPropertyName("publicationType")]
	public string? PublicationType { get; set; }

	/// <summary>
	/// Publication date
	/// </summary>
	[JsonPropertyName("publicationDate")]
	public DateTime? PublicationDate { get; set; }

	/// <summary>
	/// Publication URL
	/// </summary>
	[JsonPropertyName("url")]
	public string? Url { get; set; }
}

/// <summary>
/// Represents a submission to an inquiry
/// </summary>
public class Submission
{
	/// <summary>
	/// Submission identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Submitter name
	/// </summary>
	[JsonPropertyName("submitter")]
	public string Submitter { get; set; } = string.Empty;

	/// <summary>
	/// Submission date
	/// </summary>
	[JsonPropertyName("submissionDate")]
	public DateTime? SubmissionDate { get; set; }

	/// <summary>
	/// Submission title
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// Document URL
	/// </summary>
	[JsonPropertyName("documentUrl")]
	public string? DocumentUrl { get; set; }
}

/// <summary>
/// Represents a contribution to an inquiry (oral evidence, etc.)
/// </summary>
public class Contribution
{
	/// <summary>
	/// Contribution identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Contributor name
	/// </summary>
	[JsonPropertyName("contributor")]
	public string Contributor { get; set; } = string.Empty;

	/// <summary>
	/// Contribution date
	/// </summary>
	[JsonPropertyName("contributionDate")]
	public DateTime? ContributionDate { get; set; }

	/// <summary>
	/// Contribution type (e.g., "Oral Evidence")
	/// </summary>
	[JsonPropertyName("contributionType")]
	public string? ContributionType { get; set; }

	/// <summary>
	/// Transcript URL
	/// </summary>
	[JsonPropertyName("transcriptUrl")]
	public string? TranscriptUrl { get; set; }
}
