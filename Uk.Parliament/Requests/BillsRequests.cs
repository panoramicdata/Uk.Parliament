#pragma warning disable CS1591
using Refit;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving bills.
/// </summary>
public sealed record class GetBillsRequest : SkipTakeRequest
{
	/// <summary>
	/// Optional search term.
	/// </summary>
	[AliasAs("searchTerm")]
	public string? SearchTerm { get; init; }

	/// <summary>
	/// Optional session identifier.
	/// </summary>
	[AliasAs("session")]
	public int? Session { get; init; }

	/// <summary>
	/// Optional current house filter.
	/// </summary>
	[AliasAs("currentHouse")]
	public string? CurrentHouse { get; init; }
}
#pragma warning restore CS1591
