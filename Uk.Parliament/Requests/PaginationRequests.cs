#pragma warning disable CS1591
using Refit;

namespace Uk.Parliament.Requests;

/// <summary>
/// Base request for APIs that support skip/take pagination.
/// </summary>
public abstract record class SkipTakeRequest
{
	/// <summary>
	/// Number of results to skip.
	/// </summary>
	[AliasAs("skip")]
	public int? Skip { get; init; }

	/// <summary>
	/// Number of results to take.
	/// </summary>
	[AliasAs("take")]
	public int? Take { get; init; }
}

/// <summary>
/// Base request for APIs that support page/page_size pagination.
/// </summary>
public abstract record class PageRequest
{
	/// <summary>
	/// One-based page number.
	/// </summary>
	[AliasAs("page")]
	public int? Page { get; init; }

	/// <summary>
	/// Number of results per page.
	/// </summary>
	[AliasAs("page_size")]
	public int? PageSize { get; init; }
}
#pragma warning restore CS1591
