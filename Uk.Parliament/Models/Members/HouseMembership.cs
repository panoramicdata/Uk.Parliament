using System;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Members;

/// <summary>
/// Represents a member's house membership (Commons or Lords)
/// </summary>
public class HouseMembership
{
	/// <summary>
	/// The membership type ID
	/// </summary>
	[JsonPropertyName("membershipFrom")]
	public string? MembershipFrom { get; set; }

	/// <summary>
	/// The constituency or membership from ID
	/// </summary>
	[JsonPropertyName("membershipFromId")]
	public int? MembershipFromId { get; set; }

	/// <summary>
	/// The membership start date
	/// </summary>
	[JsonPropertyName("membershipStartDate")]
	public DateTime? MembershipStartDate { get; set; }

	/// <summary>
	/// The membership end date (null if current)
	/// </summary>
	[JsonPropertyName("membershipEndDate")]
	public DateTime? MembershipEndDate { get; set; }

	/// <summary>
	/// The membership end reason
	/// </summary>
	[JsonPropertyName("membershipEndReason")]
	public string? MembershipEndReason { get; set; }

	/// <summary>
	/// Additional notes about the membership end reason
	/// </summary>
	[JsonPropertyName("membershipEndReasonNotes")]
	public string? MembershipEndReasonNotes { get; set; }

	/// <summary>
	/// The membership end reason ID
	/// </summary>
	[JsonPropertyName("membershipEndReasonId")]
	public int? MembershipEndReasonId { get; set; }

	/// <summary>
	/// The house (1 = Commons, 2 = Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public int House { get; set; }

	/// <summary>
	/// Whether this is the current membership
	/// </summary>
	[JsonPropertyName("membershipStatus")]
	public MembershipStatus? MembershipStatus { get; set; }
}

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
