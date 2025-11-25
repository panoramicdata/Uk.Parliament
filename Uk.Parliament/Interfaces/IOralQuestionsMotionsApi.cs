using System;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Uk.Parliament.Models.OralQuestions;

namespace Uk.Parliament.Interfaces;

/// <summary>
/// UK Parliament Oral Questions and Motions API client using Refit
/// </summary>
/// <remarks>
/// Provides access to oral questions and Early Day Motions
/// </remarks>
public interface IOralQuestionsMotionsApi
{
	/// <summary>
	/// Get oral questions with optional filtering
	/// </summary>
	/// <param name="askingMemberId">Filter by member who asked</param>
	/// <param name="answeringDepartment">Filter by answering department</param>
	/// <param name="house">Filter by house (Commons/Lords)</param>
	/// <param name="dateFrom">Filter by date from</param>
	/// <param name="dateTo">Filter by date to</param>
	/// <param name="isAnswered">Filter by answered status</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of oral questions</returns>
	[Get("/oralquestions/list")]
	Task<OralQuestionsResponse<OralQuestion>> GetOralQuestionsAsync(
		[Query] int? askingMemberId = null,
		[Query] string? answeringDepartment = null,
		[Query] string? house = null,
		[Query] DateTime? dateFrom = null,
		[Query] DateTime? dateTo = null,
		[Query] bool? isAnswered = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get Early Day Motions with optional filtering
	/// </summary>
	/// <param name="proposingMemberId">Filter by member who proposed</param>
	/// <param name="house">Filter by house (Commons/Lords)</param>
	/// <param name="dateFrom">Filter by tabled date from</param>
	/// <param name="dateTo">Filter by tabled date to</param>
	/// <param name="motionType">Filter by motion type</param>
	/// <param name="isActive">Filter by active status</param>
	/// <param name="skip">Number of results to skip</param>
	/// <param name="take">Number of results to take</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of Early Day Motions</returns>
	[Get("/EarlyDayMotions/list")]
	Task<OralQuestionsResponse<Motion>> GetMotionsAsync(
		[Query] int? proposingMemberId = null,
		[Query] string? house = null,
		[Query] DateTime? dateFrom = null,
		[Query] DateTime? dateTo = null,
		[Query] string? motionType = null,
		[Query] bool? isActive = null,
		[Query] int? skip = null,
		[Query] int? take = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific Early Day Motion by ID
	/// </summary>
	/// <param name="id">Motion identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Motion details</returns>
	[Get("/EarlyDayMotion/{id}")]
	Task<Motion> GetMotionByIdAsync(
		int id,
		CancellationToken cancellationToken = default);
}
