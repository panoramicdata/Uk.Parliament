namespace Uk.Parliament.Models.Now;

/// <summary>
/// Represents an upcoming business item in the chamber
/// </summary>
public class BusinessItem
{
	/// <summary>
	/// Business item identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// House (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public required string House { get; set; }

	/// <summary>
	/// Scheduled time
	/// </summary>
	[JsonPropertyName("scheduledTime")]
	public DateTime? ScheduledTime { get; set; }

	/// <summary>
	/// Business description
	/// </summary>
	[JsonPropertyName("description")]
	public required string Description { get; set; }

	/// <summary>
	/// Type of business (e.g., "Question Time", "Debate", "Statement")
	/// </summary>
	[JsonPropertyName("businessType")]
	public string? BusinessType { get; set; }

	/// <summary>
	/// Member presenting/leading (if applicable)
	/// </summary>
	[JsonPropertyName("leadMember")]
	public string? LeadMember { get; set; }

	/// <summary>
	/// Order in the day's business
	/// </summary>
	[JsonPropertyName("orderNumber")]
	public int OrderNumber { get; set; }

	/// <summary>
	/// Whether this item is currently active
	/// </summary>
	[JsonPropertyName("isActive")]
	public bool IsActive { get; set; }
}
