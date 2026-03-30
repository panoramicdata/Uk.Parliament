#pragma warning disable CS1591
using Uk.Parliament.Models.Committees;

namespace Uk.Parliament.Requests;

/// <summary>
/// Request for retrieving committees.
/// </summary>
public sealed record class GetCommitteesRequest : SkipTakeRequest, IPaginatedRequest<Committee>;
#pragma warning restore CS1591
