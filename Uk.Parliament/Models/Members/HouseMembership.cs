namespace Uk.Parliament.Models.Members;

/// <summary>
/// Represents a member's house membership (Commons or Lords)
/// </summary>
public class HouseMembership : MembershipBase;

/// <summary>
/// Membership status
/// </summary>
public class MembershipStatus
{
	/// <summary>
	/// Status ID
	/// </summary>
	[JsonPropertyName("statusId")]
	public int StatusId { get; set; }

	/// <summary>
	/// Status value (appears to duplicate statusId)
	/// </summary>
	[JsonPropertyName("status")]
	public int Status { get; set; }

	/// <summary>
	/// Status description
	/// </summary>
	[JsonPropertyName("statusDescription")]
	public string? StatusDescription { get; set; }

	/// <summary>
	/// Status notes
	/// </summary>
	[JsonPropertyName("statusNotes")]
	public string? StatusNotes { get; set; }

	/// <summary>
	/// Status start date
	/// </summary>
	[JsonPropertyName("statusStartDate")]
	public DateTime? StatusStartDate { get; set; }

	/// <summary>
	/// Whether the member is active
	/// </summary>
	[JsonPropertyName("statusIsActive")]
	public bool StatusIsActive { get; set; }
}
