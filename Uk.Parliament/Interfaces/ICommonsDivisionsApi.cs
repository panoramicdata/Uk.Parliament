using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Commons Divisions (Voting) API client
/// </summary>
public interface ICommonsDivisionsApi
{
	/// <summary>
	/// List all Commons divisions (placeholder - API structure TBD)
	/// </summary>
	/// <param name="queryParameters">Optional query parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of divisions</returns>
	[Get("/data/divisions.json")]
	Task<object> GetDivisionsAsync(
		[Query] string? queryParameters = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific Commons division by ID (placeholder - API structure TBD)
	/// </summary>
	/// <param name="divisionId">Division ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Division details</returns>
	[Get("/data/division/{divisionId}.json")]
	Task<object> GetDivisionByIdAsync(
		int divisionId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get division results grouped by party (placeholder - API structure TBD)
	/// </summary>
	/// <param name="divisionId">Division ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Grouped votes by party</returns>
	[Get("/data/divisions.json/groupedbyparty/{divisionId}")]
	Task<object> GetDivisionGroupedByPartyAsync(
		int divisionId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search divisions (placeholder - API structure TBD)
	/// </summary>
	/// <param name="searchTerm">Search term</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of divisions matching search criteria</returns>
	[Get("/data/divisions.json/search")]
	Task<object> SearchDivisionsAsync(
		[Query] string searchTerm,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get member voting records (placeholder - API structure TBD)
	/// </summary>
	/// <param name="memberId">Member ID</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Member voting history</returns>
	[Get("/data/divisions.json/membervoting")]
	Task<object> GetMemberVotingAsync(
		[Query] int memberId,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);
}
