namespace Uk.Parliament.Models.Treaties;

/// <summary>
/// Represents an international treaty laid before Parliament
/// </summary>
public class Treaty
{
	/// <summary>
	/// Treaty identifier
	/// </summary>
	[JsonPropertyName("id")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Treaty name (alternative to title)
	/// </summary>
	[JsonPropertyName("name")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Name { get; set; }

	/// <summary>
	/// Command paper number (e.g., "CP 123" or numeric value)
	/// </summary>
	[JsonPropertyName("commandPaperNumber")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? CommandPaperNumber { get; set; }

	/// <summary>
	/// Command paper prefix (e.g., "CP", "Cm")
	/// </summary>
	[JsonPropertyName("commandPaperPrefix")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? CommandPaperPrefix { get; set; }

	/// <summary>
	/// Treaty title
	/// </summary>
	[JsonPropertyName("title")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Title { get; set; }

	/// <summary>
	/// Treaty series reference
	/// </summary>
	[JsonPropertyName("treatySeries")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? TreatySeries { get; set; }

	/// <summary>
	/// Treaty series membership information
	/// </summary>
	[JsonPropertyName("treatySeriesMembership")]
	public object? TreatySeriesMembership { get; set; }

	/// <summary>
	/// Government organization responsible
	/// </summary>
	[JsonPropertyName("leadGovernmentOrganisationId")]
	public int? LeadGovernmentOrganisationId { get; set; }

	/// <summary>
	/// Name of lead government organization
	/// </summary>
	[JsonPropertyName("leadGovernmentOrganisation")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? LeadGovernmentOrganisation { get; set; }

	/// <summary>
	/// Lead department (alternative to leadGovernmentOrganisation)
	/// </summary>
	[JsonPropertyName("leadDepartment")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? LeadDepartment { get; set; }

	/// <summary>
	/// Date treaty was laid before Commons
	/// </summary>
	[JsonPropertyName("commonsLayingDate")]
	public DateTime? CommonsLayingDate { get; set; }

	/// <summary>
	/// Date treaty was laid before Lords
	/// </summary>
	[JsonPropertyName("lordsLayingDate")]
	public DateTime? LordsLayingDate { get; set; }

	/// <summary>
	/// Date treaty was laid before Parliament
	/// </summary>
	[JsonPropertyName("dateLaid")]
	public DateTime? DateLaid { get; set; }

	/// <summary>
	/// Date treaty came into force
	/// </summary>
	[JsonPropertyName("dateIntoForce")]
	public DateTime? DateIntoForce { get; set; }

	/// <summary>
	/// Date treaty was signed
	/// </summary>
	[JsonPropertyName("dateSigned")]
	public DateTime? DateSigned { get; set; }

	/// <summary>
	/// House where treaty was laid (Commons/Lords/Both)
	/// </summary>
	[JsonPropertyName("house")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? House { get; set; }

	/// <summary>
	/// Treaty status (e.g., "In Force", "Not Yet In Force")
	/// </summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Status { get; set; }

	/// <summary>
	/// Web link to treaty document
	/// </summary>
	[JsonPropertyName("webLink")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? WebLink { get; set; }

	/// <summary>
	/// Whether the treaty is multilateral
	/// </summary>
	[JsonPropertyName("isMultilateral")]
	public bool IsMultilateral { get; set; }

	/// <summary>
	/// Countries/parties involved
	/// </summary>
	[JsonPropertyName("countries")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Countries { get; set; }

	/// <summary>
	/// Subject matter of the treaty
	/// </summary>
	[JsonPropertyName("subject")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Subject { get; set; }

	/// <summary>
	/// URI/identifier for the treaty
	/// </summary>
	[JsonPropertyName("uri")]
	[JsonConverter(typeof(AnyValueToStringConverter))]
	public string? Uri { get; set; }

	/// <summary>
	/// Gets the effective status value from either status or webLink properties
	/// </summary>
	[JsonIgnore]
	public string? EffectiveStatus => Status ?? (IsMultilateral ? null : "Unknown");
}
