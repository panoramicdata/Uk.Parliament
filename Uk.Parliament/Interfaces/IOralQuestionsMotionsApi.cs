using Refit;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Models.OralQuestions;
using Uk.Parliament.Requests;

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
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of oral questions</returns>
	[Get("/oralquestions/list")]
	Task<OralQuestionsResponse<OralQuestion>> GetOralQuestionsAsync(
		[Query] GetOralQuestionsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get Early Day Motions with optional filtering
	/// </summary>
	/// <param name="request">Request parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Paginated list of Early Day Motions</returns>
	[Get("/EarlyDayMotions/list")]
	Task<OralQuestionsResponse<Motion>> GetMotionsAsync(
		[Query] GetMotionsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a specific Early Day Motion by ID
	/// </summary>
	/// <param name="id">Motion identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Motion details wrapped in response</returns>
	[Get("/EarlyDayMotion/{id}")]
	Task<OralQuestionsResponse<Motion>> GetMotionByIdAsync(
		int id,
		CancellationToken cancellationToken = default);
}
