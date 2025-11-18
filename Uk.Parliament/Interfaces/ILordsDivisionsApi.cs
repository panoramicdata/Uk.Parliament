namespace Uk.Parliament.Interfaces;

using Refit;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// UK Parliament Lords Divisions (Voting) API client
/// </summary>
public interface ILordsDivisionsApi
{
	/// <summary>
	/// List all Lords divisions (placeholder - API structure TBD)
	/// </summary>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of divisions</returns>
	[Get("/data/Divisions")]
	Task<object> GetDivisionsAsync(
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific Lords division by ID (placeholder - API structure TBD)
	/// </summary>
	/// <param name="divisionId">Division ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Division details</returns>
	[Get("/data/Divisions/{divisionId}")]
	Task<object> GetDivisionByIdAsync(
		int divisionId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get division results grouped by party (placeholder - API structure TBD)
	/// </summary>
	/// <param name="divisionId">Division ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Grouped votes by party</returns>
	[Get("/data/Divisions/groupedbyparty/{divisionId}")]
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
	[Get("/data/Divisions/search")]
	Task<object> SearchDivisionsAsync(
		[Query] string searchTerm,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);
}
