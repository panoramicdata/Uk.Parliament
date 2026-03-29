#pragma warning disable CS1591
using Refit;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving Commons divisions.
/// </summary>
public sealed record class GetCommonsDivisionsRequest : SkipTakeRequest;

/// <summary>
/// Request for searching Commons divisions.
/// </summary>
public sealed record class SearchCommonsDivisionsRequest : SkipTakeRequest
{
	/// <summary>
	/// Search term.
	/// </summary>
	[AliasAs("searchTerm")]
	public required string SearchTerm { get; init; }
}
#pragma warning restore CS1591

/// <summary>
/// Request for retrieving Commons member voting.
/// </summary>
public sealed record class GetCommonsMemberVotingRequest : SkipTakeRequest
{
	/// <summary>
	/// Member identifier.
	/// </summary>
	[AliasAs("memberId")]
	public required int MemberId { get; init; }
}

/// <summary>
/// Request for retrieving Lords divisions.
/// </summary>
public sealed record class GetLordsDivisionsRequest : SkipTakeRequest;

/// <summary>
/// Request for searching Lords divisions.
/// </summary>
public sealed record class SearchLordsDivisionsRequest : SkipTakeRequest
{
	/// <summary>
	/// Search term.
	/// </summary>
	[AliasAs("searchTerm")]
	public required string SearchTerm { get; init; }
}
