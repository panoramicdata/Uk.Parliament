namespace Uk.Parliament.Models.Bills;

/// <summary>
/// Represents a parliamentary bill
/// </summary>
public class Bill
{
	/// <summary>
	/// The bill's unique identifier
	/// </summary>
	[JsonPropertyName("billId")]
	public int BillId { get; set; }

	/// <summary>
	/// Short title of the bill
	/// </summary>
	[JsonPropertyName("shortTitle")]
	public string ShortTitle { get; set; } = string.Empty;

	/// <summary>
	/// Former short title (if renamed)
	/// </summary>
	[JsonPropertyName("formerShortTitle")]
	public string? FormerShortTitle { get; set; }

	/// <summary>
	/// Long title of the bill
	/// </summary>
	[JsonPropertyName("longTitle")]
	public string? LongTitle { get; set; }

	/// <summary>
	/// Summary of the bill
	/// </summary>
	[JsonPropertyName("summary")]
	public string? Summary { get; set; }

	/// <summary>
	/// Current house (Commons or Lords)
	/// </summary>
	[JsonPropertyName("currentHouse")]
	public string? CurrentHouse { get; set; }

	/// <summary>
	/// Originating house
	/// </summary>
	[JsonPropertyName("originatingHouse")]
	public string? OriginatingHouse { get; set; }

	/// <summary>
	/// Last update timestamp
	/// </summary>
	[JsonPropertyName("lastUpdate")]
	public DateTime LastUpdate { get; set; }

	/// <summary>
	/// Date the bill was withdrawn (if applicable)
	/// </summary>
	[JsonPropertyName("billWithdrawn")]
	public DateTime? BillWithdrawn { get; set; }

	/// <summary>
	/// Whether the bill was defeated
	/// </summary>
	[JsonPropertyName("isDefeated")]
	public bool IsDefeated { get; set; }

	/// <summary>
	/// Bill type identifier
	/// </summary>
	[JsonPropertyName("billTypeId")]
	public int BillTypeId { get; set; }

	/// <summary>
	/// Session ID when introduced
	/// </summary>
	[JsonPropertyName("introducedSessionId")]
	public int IntroducedSessionId { get; set; }

	/// <summary>
	/// List of session IDs the bill has been in
	/// </summary>
	[JsonPropertyName("includedSessionIds")]
	public List<int> IncludedSessionIds { get; set; } = [];

	/// <summary>
	/// Whether the bill has become an Act
	/// </summary>
	[JsonPropertyName("isAct")]
	public bool IsAct { get; set; }

	/// <summary>
	/// Current stage of the bill
	/// </summary>
	[JsonPropertyName("currentStage")]
	public BillStage? CurrentStage { get; set; }

	/// <summary>
	/// List of sponsors
	/// </summary>
	[JsonPropertyName("sponsors")]
	public List<Sponsor>? Sponsors { get; set; }

	/// <summary>
	/// List of promoters
	/// </summary>
	[JsonPropertyName("promoters")]
	public List<Promoter>? Promoters { get; set; }

	/// <summary>
	/// Petitioning period information
	/// </summary>
	[JsonPropertyName("petitioningPeriod")]
	public string? PetitioningPeriod { get; set; }

	/// <summary>
	/// Petition information
	/// </summary>
	[JsonPropertyName("petitionInformation")]
	public string? PetitionInformation { get; set; }

	/// <summary>
	/// Agent information
	/// </summary>
	[JsonPropertyName("agent")]
	public string? Agent { get; set; }
}
