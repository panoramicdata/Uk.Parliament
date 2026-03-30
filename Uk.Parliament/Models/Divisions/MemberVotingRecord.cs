namespace Uk.Parliament.Models.Divisions;

/// <summary>
/// Represents a member's voting record for a Commons division
/// </summary>
public class MemberVotingRecord
{
	/// <summary>
	/// Member identifier
	/// </summary>
	[JsonPropertyName("memberId")]
	public int MemberId { get; set; }

	/// <summary>
	/// Whether the member voted Aye
	/// </summary>
	[JsonPropertyName("memberVotedAye")]
	public bool MemberVotedAye { get; set; }

	/// <summary>
	/// Whether the member voted No
	/// </summary>
	[JsonPropertyName("memberVotedNo")]
	public bool MemberVotedNo { get; set; }

	/// <summary>
	/// Whether the member was a teller
	/// </summary>
	[JsonPropertyName("memberWasTeller")]
	public bool MemberWasTeller { get; set; }

	/// <summary>
	/// The division the member voted in
	/// </summary>
	[JsonPropertyName("publishedDivision")]
	public CommonsDivision? PublishedDivision { get; set; }
}
