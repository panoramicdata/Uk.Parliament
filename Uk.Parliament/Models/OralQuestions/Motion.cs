namespace Uk.Parliament.Models.OralQuestions;

/// <summary>
/// Represents a parliamentary motion
/// </summary>
public class Motion
{
	/// <summary>
	/// Motion identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Reference number for the motion
	/// </summary>
	[JsonPropertyName("reference")]
	[JsonConverter(typeof(AnyToStringConverter))]
	public string? Reference { get; set; }

	/// <summary>
	/// Member who proposed the motion
	/// </summary>
	[JsonPropertyName("proposingMemberId")]
	public int ProposingMemberId { get; set; }

	/// <summary>
	/// Name of the member who proposed
	/// </summary>
	[JsonPropertyName("proposingMember")]
	[JsonConverter(typeof(AnyToStringConverter))]
	public string? ProposingMember { get; set; }

	/// <summary>
	/// House where motion was proposed (Commons/Lords)
	/// </summary>
	[JsonPropertyName("house")]
	[JsonConverter(typeof(AnyToStringConverter))]
	public string? House { get; set; }

	/// <summary>
	/// Date the motion was tabled
	/// </summary>
	[JsonPropertyName("dateTabled")]
	public DateTime DateTabled { get; set; }

	/// <summary>
	/// Motion title/subject
	/// </summary>
	[JsonPropertyName("title")]
	[JsonConverter(typeof(AnyToStringConverter))]
	public string? Title { get; set; }

	/// <summary>
	/// Motion text/content
	/// </summary>
	[JsonPropertyName("motionText")]
	[JsonConverter(typeof(AnyToStringConverter))]
	public string? MotionText { get; set; }

	/// <summary>
	/// Type of motion (e.g., "Early Day Motion", "Amendment")
	/// </summary>
	[JsonPropertyName("motionType")]
	[JsonConverter(typeof(AnyToStringConverter))]
	public string? MotionType { get; set; }

	/// <summary>
	/// Number of signatures/supporters
	/// </summary>
	[JsonPropertyName("signatureCount")]
	public int SignatureCount { get; set; }

	/// <summary>
	/// Whether the motion is still active
	/// </summary>
	[JsonPropertyName("isActive")]
	public bool IsActive { get; set; }

	/// <summary>
	/// Whether the motion was withdrawn
	/// </summary>
	[JsonPropertyName("isWithdrawn")]
	public bool IsWithdrawn { get; set; }

	/// <summary>
	/// Motion status (e.g., "Active", "Closed", "Withdrawn")
	/// </summary>
	[JsonPropertyName("Status")]
	[JsonConverter(typeof(AnyToStringConverter))]
	public string? Status { get; set; }

	/// <summary>
	/// Related document URL
	/// </summary>
	[JsonPropertyName("documentUrl")]
	[JsonConverter(typeof(AnyToStringConverter))]
	public string? DocumentUrl { get; set; }
}

/// <summary>
/// JSON converter that handles any scalar value type and converts to string
/// </summary>
internal sealed class AnyToStringConverter : LenientStringConverter
{
	protected override bool AllowBooleans => true;
}
