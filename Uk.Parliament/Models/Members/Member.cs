using System.Text.Json.Serialization;

namespace Uk.Parliament.Models.Members;

/// <summary>
/// Represents a member of Parliament (MP or Lord)
/// </summary>
public class Member
{
	/// <summary>
	/// The member's unique identifier
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// The member's name as it appears in lists
	/// </summary>
	[JsonPropertyName("nameListAs")]
	public string NameListAs { get; set; } = string.Empty;

	/// <summary>
	/// The member's display name
	/// </summary>
	[JsonPropertyName("nameDisplayAs")]
	public string NameDisplayAs { get; set; } = string.Empty;

	/// <summary>
	/// The member's full title (e.g., "Mr John Smith MP")
	/// </summary>
	[JsonPropertyName("nameFullTitle")]
	public string NameFullTitle { get; set; } = string.Empty;

	/// <summary>
	/// The member's addressee name
	/// </summary>
	[JsonPropertyName("nameAddressAs")]
	public string? NameAddressAs { get; set; }

	/// <summary>
	/// The member's gender (M/F/U)
	/// </summary>
	[JsonPropertyName("gender")]
	public string? Gender { get; set; }

	/// <summary>
	/// The party the member belongs to
	/// </summary>
	[JsonPropertyName("latestParty")]
	public Party? LatestParty { get; set; }

	/// <summary>
	/// The member's latest house membership (Commons/Lords)
	/// </summary>
	[JsonPropertyName("latestHouseMembership")]
	public HouseMembership? LatestHouseMembership { get; set; }

	/// <summary>
	/// The member's thumbnail URL
	/// </summary>
	[JsonPropertyName("thumbnailUrl")]
	public string? ThumbnailUrl { get; set; }
}
