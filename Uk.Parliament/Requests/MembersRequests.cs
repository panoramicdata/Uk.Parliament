#pragma warning disable CS1591
using Refit;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for searching members.
/// </summary>
public sealed record class SearchMembersRequest : SkipTakeRequest
{
	/// <summary>
	/// Optional member name.
	/// </summary>
	[AliasAs("name")]
	public string? Name { get; init; }

	/// <summary>
	/// Optional house filter.
	/// </summary>
	[AliasAs("house")]
	public int? House { get; init; }

	/// <summary>
	/// Optional current-member filter.
	/// </summary>
	[AliasAs("isCurrentMember")]
	public bool? IsCurrentMember { get; init; }
}

/// <summary>
/// Request for searching constituencies.
/// </summary>
public sealed record class SearchConstituenciesRequest : SkipTakeRequest
{
	/// <summary>
	/// Optional search text.
	/// </summary>
	[AliasAs("searchText")]
	public string? SearchText { get; init; }
}
#pragma warning restore CS1591
