using Refit;
using Uk.Parliament.Models.Interests;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for searching interests.
/// </summary>
public sealed record class SearchInterestsRequest : SkipTakeRequest, IPaginatedRequest<Interest>
{
	/// <summary>
	/// Optional member identifier.
	/// </summary>
	[AliasAs("memberId")]
	public int? MemberId { get; init; }

	/// <summary>
	/// Optional category identifier.
	/// </summary>
	[AliasAs("categoryId")]
	public int? CategoryId { get; init; }

	/// <summary>
	/// Optional search term.
	/// </summary>
	[AliasAs("searchTerm")]
	public string? SearchTerm { get; init; }
}