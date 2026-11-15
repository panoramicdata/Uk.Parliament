using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Extensions;
using Uk.Parliament.Requests;

namespace Uk.Parliament;

public partial class ParliamentClient
{
	/// <summary>
	/// Automatically paginate through all results for the given request.
	/// </summary>
	/// <typeparam name="TItem">The item type returned by the API.</typeparam>
	/// <param name="request">A request that implements <see cref="IPaginatedRequest{TItem}"/>.</param>
	/// <param name="cancellationToken">Cancellation token (required).</param>
	/// <returns>An async enumerable that streams all pages.</returns>
	/// <exception cref="NotSupportedException">Thrown when the request type does not have a registered pagination strategy.</exception>
	public IAsyncEnumerable<TItem> GetAllAsync<TItem>(
		IPaginatedRequest<TItem> request,
		CancellationToken cancellationToken)
	{
		// Each dispatcher returns null for a request it does not own.
		var pager = GetLegislationPager(request, cancellationToken)
			?? GetPeoplePager(request, cancellationToken)
			?? GetQuestionsPager(request, cancellationToken)
			?? throw new NotSupportedException($"Pagination is not supported for request type {request.GetType().Name}.");

		return (IAsyncEnumerable<TItem>)pager;
	}

	/// <summary>
	/// Automatically paginate through all results and return them as a materialized list.
	/// </summary>
	/// <typeparam name="TItem">The item type returned by the API.</typeparam>
	/// <param name="request">A request that implements <see cref="IPaginatedRequest{TItem}"/>.</param>
	/// <param name="cancellationToken">Cancellation token (required).</param>
	/// <returns>A list containing every item from all pages.</returns>
	public Task<List<TItem>> GetAllListAsync<TItem>(
		IPaginatedRequest<TItem> request,
		CancellationToken cancellationToken)
		=> PaginationHelper.ToListAsync(GetAllAsync(request, cancellationToken), cancellationToken);

	private object? GetLegislationPager(object request, CancellationToken ct) => request switch
	{
		GetBillsRequest r => Offset(r, Bills.GetBillsAsync, x => x.Items, x => x.TotalResults, ct),
		GetCommitteesRequest r => Offset(r, Committees.GetCommitteesAsync, x => x.Items, x => x.TotalResults, ct),
		GetTreatiesRequest r => Offset(r, Treaties.GetTreatiesAsync, x => GetWrappedItems(x), x => x.TotalResults, ct),
		_ => null
	};

	private object? GetPeoplePager(object request, CancellationToken ct) => request switch
	{
		SearchMembersRequest r => Offset(r, Members.SearchAsync, x => GetWrappedItems(x), x => x.TotalResults, ct),
		SearchConstituenciesRequest r => Offset(r, Members.SearchConstituenciesAsync, x => GetWrappedItems(x), x => x.TotalResults, ct),
		SearchInterestsRequest r => Offset(r, Interests.SearchInterestsAsync, x => x.Items, x => x.TotalResults, ct),
		GetPetitionsRequest r => PetitionPages(r, ct),
		_ => null
	};

	private object? GetQuestionsPager(object request, CancellationToken ct) => request switch
	{
		GetWrittenQuestionsRequest r => Offset(r, QuestionsStatements.GetWrittenQuestionsAsync, x => GetWrappedItems(x), x => x.TotalResults, ct),
		GetWrittenStatementsRequest r => Offset(r, QuestionsStatements.GetWrittenStatementsAsync, x => GetWrappedItems(x), x => x.TotalResults, ct),
		GetDailyReportsRequest r => Offset(r, QuestionsStatements.GetDailyReportsAsync, x => GetWrappedItems(x), x => x.TotalResults, ct),
		GetOralQuestionsRequest r => Offset(r, OralQuestionsMotions.GetOralQuestionsAsync, x => x.Response, x => x.PagingInfo.Total, ct),
		GetMotionsRequest r => Offset(r, OralQuestionsMotions.GetMotionsAsync, x => x.Response, x => x.PagingInfo.Total, ct),
		_ => null
	};

	private static IAsyncEnumerable<TItem> Offset<TRequest, TResponse, TItem>(
		TRequest request,
		Func<TRequest, CancellationToken, Task<TResponse>> fetchPage,
		Func<TResponse, IReadOnlyList<TItem>?> getItems,
		Func<TResponse, int> getTotalResults,
		CancellationToken cancellationToken)
		where TRequest : SkipTakeRequest
		=> PaginationHelper.GetAllOffsetAsync(
			request,
			request.Take ?? DefaultTakePageSize,
			static (r, skip, take) => (TRequest)r.WithSkipTake(skip, take),
			fetchPage,
			getItems,
			getTotalResults,
			cancellationToken);

	// Petitions pages by page number and reports no total, so a short page ends the walk.
	private IAsyncEnumerable<Models.Petition> PetitionPages(GetPetitionsRequest request, CancellationToken cancellationToken)
		=> PaginationHelper.GetAllOffsetAsync(
			request,
			request.PageSize ?? DefaultPetitionsPageSize,
			static (r, skip, size) => r with { Page = (skip / size) + 1, PageSize = size },
			(r, ct) => Petitions.GetAsync(r, ct),
			resp => resp.Data,
			static _ => int.MaxValue,
			cancellationToken);

	private static List<T>? GetWrappedItems<T>(Models.PaginatedResponse<T>? response)
		=> response?.Items?.ConvertAll(static item => item.Value)
			?? response?.Results?.ConvertAll(static item => item.Value);
}
