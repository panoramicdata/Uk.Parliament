using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Bills;

/// <summary>
/// Represents a sponsor of a bill
/// </summary>
public class Sponsor
{
	/// <summary>
	/// Member information
	/// </summary>
	[JsonPropertyName("member")]
	public SponsorMember? Member { get; set; }

	/// <summary>
	/// Organization information (for non-member sponsors)
	/// </summary>
	[JsonPropertyName("organisation")]
	public string? Organisation { get; set; }

	/// <summary>
	/// Sort order for display
	/// </summary>
	[JsonPropertyName("sortOrder")]
	public int SortOrder { get; set; }
}

/// <summary>
/// Represents a promoter of a bill
/// </summary>
public class Promoter
{
	/// <summary>
	/// Promoter organization information
	/// </summary>
	[JsonPropertyName("organisationName")]
	public string? OrganisationName { get; set; }

	/// <summary>
	/// Promoter URL
	/// </summary>
	[JsonPropertyName("organisationUrl")]
	public string? OrganisationUrl { get; set; }
}

/// <summary>
/// Represents a member who is sponsoring a bill
/// </summary>
public class SponsorMember
{
	/// <summary>
	/// Member identifier
	/// </summary>
	[JsonPropertyName("memberId")]
	public int MemberId { get; set; }

	/// <summary>
	/// Member name
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Party affiliation
	/// </summary>
	[JsonPropertyName("party")]
	public string? Party { get; set; }

	/// <summary>
	/// Party color (hex code)
	/// </summary>
	[JsonPropertyName("partyColour")]
	public string? PartyColour { get; set; }

	/// <summary>
	/// House (Commons or Lords)
	/// </summary>
	[JsonPropertyName("house")]
	public string? House { get; set; }

	/// <summary>
	/// URL to member photo
	/// </summary>
	[JsonPropertyName("memberPhoto")]
	public string? MemberPhoto { get; set; }

	/// <summary>
	/// URL to member page
	/// </summary>
	[JsonPropertyName("memberPage")]
	public string? MemberPage { get; set; }

	/// <summary>
	/// Constituency or area the member represents
	/// </summary>
	[JsonPropertyName("memberFrom")]
	public string? MemberFrom { get; set; }
}
