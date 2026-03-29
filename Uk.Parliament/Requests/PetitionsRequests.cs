#pragma warning disable CS1591
using Refit;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving petitions.
/// </summary>
public sealed record class GetPetitionsRequest : PageRequest
{
	/// <summary>
	/// Optional search term.
	/// </summary>
	[AliasAs("search")]
	public string? Search { get; init; }

	/// <summary>
	/// Optional petition state.
	/// </summary>
	[AliasAs("state")]
	public string? State { get; init; }
}
#pragma warning restore CS1591
