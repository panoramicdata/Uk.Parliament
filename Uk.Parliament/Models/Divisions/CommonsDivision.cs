namespace Uk.Parliament.Models.Divisions;

/// <summary>
/// Represents a House of Commons division (vote)
/// </summary>
public class CommonsDivision
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
	/// Date and time the publication was last updated
	/// </summary>
	[JsonPropertyName("publicationUpdated")]
	public DateTime? PublicationUpdated { get; set; }

	/// <summary>
	/// Division number
	/// </summary>
	[JsonPropertyName("number")]
	public int Number { get; set; }

	/// <summary>
	/// Whether the division was deferred
	/// </summary>
	[JsonPropertyName("isDeferred")]
	public bool IsDeferred { get; set; }

	/// <summary>
	/// English Votes for English Laws type
	/// </summary>
	[JsonPropertyName("evelType")]
	public string? EVELType { get; set; }

	/// <summary>
	/// English Votes for English Laws country
	/// </summary>
	[JsonPropertyName("evelCountry")]
	public string? EVELCountry { get; set; }

	/// <summary>
	/// Division title
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// Number of Aye votes
	/// </summary>
	[JsonPropertyName("ayeCount")]
	public int AyeCount { get; set; }

	/// <summary>
	/// Number of No votes
	/// </summary>
	[JsonPropertyName("noCount")]
	public int NoCount { get; set; }

	/// <summary>
	/// Double majority Aye count (EVEL)
	/// </summary>
	[JsonPropertyName("doubleMajorityAyeCount")]
	public int? DoubleMajorityAyeCount { get; set; }

	/// <summary>
	/// Double majority No count (EVEL)
	/// </summary>
	[JsonPropertyName("doubleMajorityNoCount")]
	public int? DoubleMajorityNoCount { get; set; }

	/// <summary>
	/// Tellers for the Aye lobby
	/// </summary>
	[JsonPropertyName("ayeTellers")]
	public List<DivisionVoter> AyeTellers { get; set; } = [];

	/// <summary>
	/// Tellers for the No lobby
	/// </summary>
	[JsonPropertyName("noTellers")]
	public List<DivisionVoter> NoTellers { get; set; } = [];

	/// <summary>
	/// Members who voted Aye
	/// </summary>
	[JsonPropertyName("ayes")]
	public List<DivisionVoter> Ayes { get; set; } = [];

	/// <summary>
	/// Members who voted No
	/// </summary>
	[JsonPropertyName("noes")]
	public List<DivisionVoter> Noes { get; set; } = [];

	/// <summary>
	/// Human-readable description of the division
	/// </summary>
	[JsonPropertyName("friendlyDescription")]
	public string? FriendlyDescription { get; set; }

	/// <summary>
	/// Human-readable title of the division
	/// </summary>
	[JsonPropertyName("friendlyTitle")]
	public string? FriendlyTitle { get; set; }

	/// <summary>
	/// Members with no vote recorded
	/// </summary>
	[JsonPropertyName("noVoteRecorded")]
	public List<DivisionVoter> NoVoteRecorded { get; set; } = [];

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
}
