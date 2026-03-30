namespace Uk.Parliament.Models.Divisions;

/// <summary>
/// Represents a House of Lords division (vote)
/// </summary>
public class LordsDivision
{
	/// <summary>
	/// Division identifier
	/// </summary>
	[JsonPropertyName("divisionId")]
	public int DivisionId { get; set; }

	/// <summary>
	/// Date and time of the division
	/// </summary>
	[JsonPropertyName("date")]
	public DateTime? Date { get; set; }

	/// <summary>
	/// Division number
	/// </summary>
	[JsonPropertyName("number")]
	public int Number { get; set; }

	/// <summary>
	/// Notes about the division
	/// </summary>
	[JsonPropertyName("notes")]
	public string? Notes { get; set; }

	/// <summary>
	/// Division title
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// Whether a whip was issued
	/// </summary>
	[JsonPropertyName("isWhipped")]
	public bool IsWhipped { get; set; }

	/// <summary>
	/// Whether the government voted Content
	/// </summary>
	[JsonPropertyName("isGovernmentContent")]
	public bool IsGovernmentContent { get; set; }

	/// <summary>
	/// Authoritative Content count
	/// </summary>
	[JsonPropertyName("authoritativeContentCount")]
	public int AuthoritativeContentCount { get; set; }

	/// <summary>
	/// Authoritative Not-Content count
	/// </summary>
	[JsonPropertyName("authoritativeNotContentCount")]
	public int AuthoritativeNotContentCount { get; set; }

	/// <summary>
	/// Whether the division had tellers
	/// </summary>
	[JsonPropertyName("divisionHadTellers")]
	public bool DivisionHadTellers { get; set; }

	/// <summary>
	/// Teller Content count
	/// </summary>
	[JsonPropertyName("tellerContentCount")]
	public int TellerContentCount { get; set; }

	/// <summary>
	/// Teller Not-Content count
	/// </summary>
	[JsonPropertyName("tellerNotContentCount")]
	public int TellerNotContentCount { get; set; }

	/// <summary>
	/// Member Content count
	/// </summary>
	[JsonPropertyName("memberContentCount")]
	public int MemberContentCount { get; set; }

	/// <summary>
	/// Member Not-Content count
	/// </summary>
	[JsonPropertyName("memberNotContentCount")]
	public int MemberNotContentCount { get; set; }

	/// <summary>
	/// Member ID of the sponsoring member
	/// </summary>
	[JsonPropertyName("sponsoringMemberId")]
	public int? SponsoringMemberId { get; set; }

	/// <summary>
	/// Whether this was a House division (as opposed to committee)
	/// </summary>
	[JsonPropertyName("isHouse")]
	public bool IsHouse { get; set; }

	/// <summary>
	/// Whether the division was inquorate
	/// </summary>
	[JsonPropertyName("isInquorate")]
	public bool IsInquorate { get; set; }

	/// <summary>
	/// Notes about the amendment motion (HTML)
	/// </summary>
	[JsonPropertyName("amendmentMotionNotes")]
	public string? AmendmentMotionNotes { get; set; }

	/// <summary>
	/// Whether the government won
	/// </summary>
	[JsonPropertyName("isGovernmentWin")]
	public bool IsGovernmentWin { get; set; }

	/// <summary>
	/// Start time of remote voting (if applicable)
	/// </summary>
	[JsonPropertyName("remoteVotingStart")]
	public DateTime? RemoteVotingStart { get; set; }

	/// <summary>
	/// End time of remote voting (if applicable)
	/// </summary>
	[JsonPropertyName("remoteVotingEnd")]
	public DateTime? RemoteVotingEnd { get; set; }

	/// <summary>
	/// Whether the division was exclusively remote
	/// </summary>
	[JsonPropertyName("divisionWasExclusivelyRemote")]
	public bool DivisionWasExclusivelyRemote { get; set; }

	/// <summary>
	/// Members who voted Content
	/// </summary>
	[JsonPropertyName("contents")]
	public List<DivisionVoter> Contents { get; set; } = [];

	/// <summary>
	/// Members who voted Not-Content
	/// </summary>
	[JsonPropertyName("notContents")]
	public List<DivisionVoter> NotContents { get; set; } = [];

	/// <summary>
	/// Tellers for the Content lobby
	/// </summary>
	[JsonPropertyName("contentTellers")]
	public List<DivisionVoter> ContentTellers { get; set; } = [];

	/// <summary>
	/// Tellers for the Not-Content lobby
	/// </summary>
	[JsonPropertyName("notContentTellers")]
	public List<DivisionVoter> NotContentTellers { get; set; } = [];
}
