using Refit;
using Uk.Parliament.Models.Treaties;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving treaties.
/// </summary>
public sealed record class GetTreatiesRequest : SkipTakeRequest, IPaginatedRequest<Treaty>
{
	/// <summary>
	/// Filter by government organisation.
	/// </summary>
	[AliasAs("governmentOrganisationId")]
	public int? GovernmentOrganisationId { get; init; }

	/// <summary>
	/// Filter by house (Commons/Lords/Both).
	/// </summary>
	[AliasAs("house")]
	public string? House { get; init; }

	/// <summary>
	/// Filter by treaty status.
	/// </summary>
	[AliasAs("status")]
	public string? Status { get; init; }

	/// <summary>
	/// Filter by laid date from.
	/// </summary>
	[AliasAs("dateLaidFrom")]
	public DateTime? DateLaidFrom { get; init; }

	/// <summary>
	/// Filter by laid date to.
	/// </summary>
	[AliasAs("dateLaidTo")]
	public DateTime? DateLaidTo { get; init; }
}
