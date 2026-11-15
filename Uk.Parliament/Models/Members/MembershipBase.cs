namespace Uk.Parliament.Models.Members;

/// <summary>
/// The fields shared by every description of a membership of a house or constituency
/// </summary>
public abstract class MembershipBase
{
	/// <summary>
	/// The constituency or area name
	/// </summary>
	[JsonPropertyName("membershipFrom")]
	public string? MembershipFrom { get; set; }

	/// <summary>
	/// The constituency or area ID
	/// </summary>
	[JsonPropertyName("membershipFromId")]
	public int? MembershipFromId { get; set; }

	/// <summary>
	/// The house (1 = Commons, 2 = Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public int House { get; set; }

	/// <summary>
	/// When the membership started
	/// </summary>
	[JsonPropertyName("membershipStartDate")]
	public DateTime? MembershipStartDate { get; set; }

	/// <summary>
	/// When the membership ended (null if current)
	/// </summary>
	[JsonPropertyName("membershipEndDate")]
	public DateTime? MembershipEndDate { get; set; }

	/// <summary>
	/// Reason for the membership ending
	/// </summary>
	[JsonPropertyName("membershipEndReason")]
	public string? MembershipEndReason { get; set; }

	/// <summary>
	/// Additional notes about the end reason
	/// </summary>
	[JsonPropertyName("membershipEndReasonNotes")]
	public string? MembershipEndReasonNotes { get; set; }

	/// <summary>
	/// ID of the end reason
	/// </summary>
	[JsonPropertyName("membershipEndReasonId")]
	public int? MembershipEndReasonId { get; set; }

	/// <summary>
	/// Membership status information
	/// </summary>
	[JsonPropertyName("membershipStatus")]
	public MembershipStatus? MembershipStatus { get; set; }
}
