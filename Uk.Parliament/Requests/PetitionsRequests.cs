using Refit;
using Uk.Parliament.Models;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving petitions.
/// </summary>
public sealed record class GetPetitionsRequest : PageRequest, IPaginatedRequest<Petition>
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