using Refit;
using Uk.Parliament.Models.Bills;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving bills.
/// </summary>
public sealed record class GetBillsRequest : SkipTakeRequest, IPaginatedRequest<Bill>
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