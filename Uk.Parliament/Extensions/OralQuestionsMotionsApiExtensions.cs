using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.OralQuestions;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IOralQuestionsMotionsApi to provide additional functionality
/// </summary>
public static class OralQuestionsMotionsApiExtensions
{
	/// <summary>
	/// Get all oral questions by automatically paginating through all results
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="askingMemberId">Filter by member who asked</param>
	/// <param name="answeringDepartment">Filter by answering department</param>
	/// <param name="house">Filter by house</param>
	/// <param name="dateFrom">Filter by date from</param>
	/// <param name="dateTo">Filter by date to</param>
	/// <param name="isAnswered">Filter by answered status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all oral questions</returns>
	public static async IAsyncEnumerable<OralQuestion> GetAllOralQuestionsAsync(
		this IOralQuestionsMotionsApi api,
		int? askingMemberId = null,
		string? answeringDepartment = null,
		string? house = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		bool? isAnswered = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (true)
		{
			var response = await api.GetOralQuestionsAsync(
				askingMemberId,
				answeringDepartment,
				house,
				dateFrom,
				dateTo,
				isAnswered,
				skip,
				pageSize,
				cancellationToken);

			if (response?.Items is null || response.Items.Count == 0)
			{
				yield break;
			}

			foreach (var item in response.Items)
			{
				yield return item.Value;
			}

			// Stop if this was the last page
			if (response.Items.Count < pageSize || skip + pageSize >= response.TotalResults)
			{
				yield break;
			}

			skip += pageSize;
		}
	}

	/// <summary>
	/// Get all oral questions as a materialized list
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="askingMemberId">Filter by member who asked</param>
	/// <param name="answeringDepartment">Filter by answering department</param>
	/// <param name="house">Filter by house</param>
	/// <param name="dateFrom">Filter by date from</param>
	/// <param name="dateTo">Filter by date to</param>
	/// <param name="isAnswered">Filter by answered status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all oral questions</returns>
	public static async Task<List<OralQuestion>> GetAllOralQuestionsListAsync(
		this IOralQuestionsMotionsApi api,
		int? askingMemberId = null,
		string? answeringDepartment = null,
		string? house = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		bool? isAnswered = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allQuestions = new List<OralQuestion>();

		await foreach (var question in api.GetAllOralQuestionsAsync(
			askingMemberId,
			answeringDepartment,
			house,
			dateFrom,
			dateTo,
			isAnswered,
			pageSize,
			cancellationToken))
		{
			allQuestions.Add(question);
		}

		return allQuestions;
	}

	/// <summary>
	/// Get all motions by automatically paginating through all results
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="proposingMemberId">Filter by member who proposed</param>
	/// <param name="house">Filter by house</param>
	/// <param name="dateFrom">Filter by tabled date from</param>
	/// <param name="dateTo">Filter by tabled date to</param>
	/// <param name="motionType">Filter by motion type</param>
	/// <param name="isActive">Filter by active status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all motions</returns>
	public static async IAsyncEnumerable<Motion> GetAllMotionsAsync(
		this IOralQuestionsMotionsApi api,
		int? proposingMemberId = null,
		string? house = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		string? motionType = null,
		bool? isActive = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (true)
		{
			var response = await api.GetMotionsAsync(
				proposingMemberId,
				house,
				dateFrom,
				dateTo,
				motionType,
				isActive,
				skip,
				pageSize,
				cancellationToken);

			if (response?.Items is null || response.Items.Count == 0)
			{
				yield break;
			}

			foreach (var item in response.Items)
			{
				yield return item.Value;
			}

			// Stop if this was the last page
			if (response.Items.Count < pageSize || skip + pageSize >= response.TotalResults)
			{
				yield break;
			}

			skip += pageSize;
		}
	}

	/// <summary>
	/// Get all motions as a materialized list
	/// </summary>
	/// <param name="api">The oral questions/motions API</param>
	/// <param name="proposingMemberId">Filter by member who proposed</param>
	/// <param name="house">Filter by house</param>
	/// <param name="dateFrom">Filter by tabled date from</param>
	/// <param name="dateTo">Filter by tabled date to</param>
	/// <param name="motionType">Filter by motion type</param>
	/// <param name="isActive">Filter by active status</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all motions</returns>
	public static async Task<List<Motion>> GetAllMotionsListAsync(
		this IOralQuestionsMotionsApi api,
		int? proposingMemberId = null,
		string? house = null,
		DateTime? dateFrom = null,
		DateTime? dateTo = null,
		string? motionType = null,
		bool? isActive = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allMotions = new List<Motion>();

		await foreach (var motion in api.GetAllMotionsAsync(
			proposingMemberId,
			house,
			dateFrom,
			dateTo,
			motionType,
			isActive,
			pageSize,
			cancellationToken))
		{
			allMotions.Add(motion);
		}

		return allMotions;
	}
}
