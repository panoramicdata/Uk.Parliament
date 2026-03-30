using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models.Divisions;
using Uk.Parliament.Requests;

#pragma warning disable CS1572, CS1573
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
	/// List all Lords divisions.
	/// </summary>
	/// <remarks>
	/// Use <see cref="GetDivisionsAsync(GetLordsDivisionsRequest, CancellationToken)"/> instead.
	/// Example: <c>GetDivisionsAsync(new GetLordsDivisionsRequest { Skip = skip, Take = take }, cancellationToken)</c>.
	/// </remarks>
	[Obsolete("Use GetDivisionsAsync(GetLordsDivisionsRequest request, CancellationToken cancellationToken) instead. Example: GetDivisionsAsync(new GetLordsDivisionsRequest { Skip = skip, Take = take }, cancellationToken).", true)]
	[Get("/data/Divisions")]
	Task<List<LordsDivision>> GetDivisionsAsync(
		[Query] int? skip = null,
		[Query] int? take = null,
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

	/// <summary>
	/// Search divisions.
	/// </summary>
	/// <remarks>
	/// Use <see cref="SearchDivisionsAsync(SearchLordsDivisionsRequest, CancellationToken)"/> instead.
	/// Example: <c>SearchDivisionsAsync(new SearchLordsDivisionsRequest { SearchTerm = searchTerm, Skip = skip, Take = take }, cancellationToken)</c>.
	/// </remarks>
	[Obsolete("Use SearchDivisionsAsync(SearchLordsDivisionsRequest request, CancellationToken cancellationToken) instead. Example: SearchDivisionsAsync(new SearchLordsDivisionsRequest { SearchTerm = searchTerm, Skip = skip, Take = take }, cancellationToken).", true)]
	[Get("/data/Divisions/search")]
	Task<List<LordsDivision>> SearchDivisionsAsync(
		[Query] string searchTerm,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);
}
#pragma warning restore CS1572, CS1573
