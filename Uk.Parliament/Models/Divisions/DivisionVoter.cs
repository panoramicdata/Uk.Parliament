namespace Uk.Parliament.Models.Divisions;

/// <summary>
/// Represents a member who voted or acted as teller in a division
/// </summary>
public class DivisionVoter
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
	public string? Name { get; set; }

	/// <summary>
	/// Display name for sorting (e.g. "Surname, Forename")
	/// </summary>
	[JsonPropertyName("listAs")]
	public string? ListAs { get; set; }

	/// <summary>
	/// Member's constituency or peer type
	/// </summary>
	[JsonPropertyName("memberFrom")]
	public string? MemberFrom { get; set; }

	/// <summary>
	/// Party name
	/// </summary>
	[JsonPropertyName("party")]
	public string? Party { get; set; }

	/// <summary>
	/// Sub-party grouping (if any)
	/// </summary>
	[JsonPropertyName("subParty")]
	public string? SubParty { get; set; }

	/// <summary>
	/// Party colour (hex, without '#')
	/// </summary>
	[JsonPropertyName("partyColour")]
	public string? PartyColour { get; set; }

	/// <summary>
	/// Party abbreviation (e.g. "Lab", "Con")
	/// </summary>
	[JsonPropertyName("partyAbbreviation")]
	public string? PartyAbbreviation { get; set; }

	/// <summary>
	/// Whether the party is the main party (Lords only)
	/// </summary>
	[JsonPropertyName("partyIsMainParty")]
	public bool? PartyIsMainParty { get; set; }

	/// <summary>
	/// Name of proxy voter (if voting by proxy)
	/// </summary>
	[JsonPropertyName("proxyName")]
	public string? ProxyName { get; set; }
}
