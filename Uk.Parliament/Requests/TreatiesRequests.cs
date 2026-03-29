#pragma warning disable CS1591
using Refit;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving treaties.
/// </summary>
public sealed record class GetTreatiesRequest : SkipTakeRequest
{
	[AliasAs("governmentOrganisationId")]
	public int? GovernmentOrganisationId { get; init; }

	[AliasAs("house")]
	public string? House { get; init; }

	[AliasAs("status")]
	public string? Status { get; init; }

	[AliasAs("dateLaidFrom")]
	public DateTime? DateLaidFrom { get; init; }

	[AliasAs("dateLaidTo")]
	public DateTime? DateLaidTo { get; init; }
}
#pragma warning restore CS1591
