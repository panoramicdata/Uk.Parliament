using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models.Divisions;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Lords Divisions (Voting) API client
/// </summary>
public interface ILordsDivisionsApi
{
	/// <summary>
	/// List all Lords divisions
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of divisions</returns>
	[Get("/data/Divisions")]
	Task<List<LordsDivision>> GetDivisionsAsync(
		[Query] GetLordsDivisionsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific Lords division by ID
	/// </summary>
	/// <param name="divisionId">Division ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Division details</returns>
	[Get("/data/Divisions/{divisionId}")]
	Task<LordsDivision> GetDivisionByIdAsync(
		int divisionId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get division results grouped by party
	/// </summary>
	/// <param name="divisionId">Division ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Division details with votes grouped by party</returns>
	[Get("/data/Divisions/groupedbyparty/{divisionId}")]
	Task<LordsDivision> GetDivisionGroupedByPartyAsync(
		int divisionId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search divisions
	/// </summary>
	/// <param name="request">Search request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of divisions matching search criteria</returns>
	[Get("/data/Divisions/search")]
	Task<List<LordsDivision>> SearchDivisionsAsync(
		[Query] SearchLordsDivisionsRequest request,
		CancellationToken cancellationToken = default);
}