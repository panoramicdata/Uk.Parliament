using System;
using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Members;

/// <summary>
/// Represents a parliamentary constituency
/// </summary>
public class Constituency
{
	/// <summary>
	/// The constituency's unique identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// The constituency name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Start date of the constituency
	/// </summary>
	[JsonPropertyName("startDate")]
	public DateTime? StartDate { get; set; }

	/// <summary>
	/// End date of the constituency (null if current)
	/// </summary>
	[JsonPropertyName("endDate")]
	public DateTime? EndDate { get; set; }

	/// <summary>
	/// Current representation details
	/// </summary>
	[JsonPropertyName("currentRepresentation")]
	public CurrentRepresentation? CurrentRepresentation { get; set; }
}

/// <summary>
/// Represents current constituency representation with member and representation details
/// </summary>
public class CurrentRepresentation
{
	/// <summary>
	/// The representing member (wrapped in ValueWrapper)
	/// </summary>
	[JsonPropertyName("member")]
	public ValueWrapper<Member>? Member { get; set; }

	/// <summary>
	/// Representation details (membership information)
	/// </summary>
	[JsonPropertyName("representation")]
	public RepresentationDetails? Representation { get; set; }
}

/// <summary>
/// Represents the representation details for a constituency
/// </summary>
public class RepresentationDetails
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
	/// When the representation started
	/// </summary>
	[JsonPropertyName("membershipStartDate")]
	public DateTime? MembershipStartDate { get; set; }

	/// <summary>
	/// When the representation ended (null if current)
	/// </summary>
	[JsonPropertyName("membershipEndDate")]
	public DateTime? MembershipEndDate { get; set; }

	/// <summary>
	/// Reason for representation ending
	/// </summary>
	[JsonPropertyName("membershipEndReason")]
	public string? MembershipEndReason { get; set; }

	/// <summary>
	/// Additional notes about end reason
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
