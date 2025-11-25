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
/// Provides access to oral questions and parliamentary motions
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
	/// Get a specific oral question by ID
	/// </summary>
	/// <param name="id">Question identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Oral question details</returns>
	[Get("/oralquestions/{id}")]
	Task<OralQuestion> GetOralQuestionByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get motions with optional filtering
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
	/// <returns>Paginated list of motions</returns>
	[Get("/motions/list")]
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
	/// Get a specific motion by ID
	/// </summary>
	/// <param name="id">Motion identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Motion details</returns>
	[Get("/motions/{id}")]
	Task<Motion> GetMotionByIdAsync(
		int id,
		CancellationToken cancellationToken = default);
}
