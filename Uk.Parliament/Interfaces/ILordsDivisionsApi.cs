#pragma warning disable CS1572, CS1573
namespace Uk.Parliament.Interfaces;

using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Requests;

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
		[Query] GetLordsDivisionsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// List all Lords divisions.
	/// </summary>
	/// <remarks>
	/// Use <see cref="GetDivisionsAsync(GetLordsDivisionsRequest, CancellationToken)"/> instead.
	/// Example: <c>GetDivisionsAsync(new GetLordsDivisionsRequest { Skip = skip, Take = take }, cancellationToken)</c>.
	/// This overload remains temporarily as a warning-only migration path.
	/// </remarks>
	[Obsolete("Use GetDivisionsAsync(GetLordsDivisionsRequest request, CancellationToken cancellationToken) instead. Example: GetDivisionsAsync(new GetLordsDivisionsRequest { Skip = skip, Take = take }, cancellationToken). This overload remains temporarily as a warning-only migration path.")]
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
		[Query] SearchLordsDivisionsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search divisions.
	/// </summary>
	/// <remarks>
	/// Use <see cref="SearchDivisionsAsync(SearchLordsDivisionsRequest, CancellationToken)"/> instead.
	/// Example: <c>SearchDivisionsAsync(new SearchLordsDivisionsRequest { SearchTerm = searchTerm, Skip = skip, Take = take }, cancellationToken)</c>.
	/// This overload remains temporarily as a warning-only migration path.
	/// </remarks>
	[Obsolete("Use SearchDivisionsAsync(SearchLordsDivisionsRequest request, CancellationToken cancellationToken) instead. Example: SearchDivisionsAsync(new SearchLordsDivisionsRequest { SearchTerm = searchTerm, Skip = skip, Take = take }, cancellationToken). This overload remains temporarily as a warning-only migration path.")]
	[Get("/data/Divisions/search")]
	Task<object> SearchDivisionsAsync(
		[Query] string searchTerm,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);
}
#pragma warning restore CS1572, CS1573
