using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models.Divisions;
using Uk.Parliament.Requests;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Commons Divisions (Voting) API client
/// </summary>
public interface ICommonsDivisionsApi
{
	/// <summary>
	/// List all Commons divisions
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of divisions</returns>
	[Get("/data/divisions.json")]
	Task<List<CommonsDivision>> GetDivisionsAsync(
		[Query] GetCommonsDivisionsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific Commons division by ID
	/// </summary>
	/// <param name="divisionId">Division ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Division details</returns>
	[Get("/data/division/{divisionId}.json")]
	Task<CommonsDivision> GetDivisionByIdAsync(
		int divisionId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get division results grouped by party
	/// </summary>
	/// <param name="divisionId">Division ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Division details with votes grouped by party</returns>
	[Get("/data/divisions.json/groupedbyparty/{divisionId}")]
	Task<CommonsDivision> GetDivisionGroupedByPartyAsync(
		int divisionId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Search divisions
	/// </summary>
	/// <param name="request">Search request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of divisions matching search criteria</returns>
	[Get("/data/divisions.json/search")]
	Task<List<CommonsDivision>> SearchDivisionsAsync(
		[Query] SearchCommonsDivisionsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get member voting records
	/// </summary>
	/// <param name="request">Request parameters including member ID</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Member voting history</returns>
	[Get("/data/divisions.json/membervoting")]
	Task<List<MemberVotingRecord>> GetMemberVotingAsync(
		[Query] GetCommonsMemberVotingRequest request,
		CancellationToken cancellationToken = default);
}