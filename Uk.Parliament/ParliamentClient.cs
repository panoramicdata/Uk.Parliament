using Refit;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Extensions;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Bills;
using Uk.Parliament.Models.Committees;
using Uk.Parliament.Models.Interests;
using Uk.Parliament.Models.Members;
using Uk.Parliament.Models.OralQuestions;
using Uk.Parliament.Models.Questions;
using Uk.Parliament.Models.Treaties;
using Uk.Parliament.Requests;

namespace Uk.Parliament;

/// <summary>
/// Unified client for all UK Parliament APIs
/// </summary>
public class ParliamentClient : IDisposable
{
	private readonly HttpClient? _ownedHttpClient;
	private readonly bool _disposeHttpClient;
	private readonly ParliamentClientOptions? _options;

	/// <summary>
	/// Petitions API - Access public petitions to Parliament
	/// </summary>
	public IPetitionsApi Petitions { get; }

	/// <summary>
	/// Members API - Access information about MPs and Lords
	/// </summary>
	public IMembersApi Members { get; }

	/// <summary>
	/// Bills API - Access parliamentary legislation information
	/// </summary>
	public IBillsApi Bills { get; }

	/// <summary>
	/// Committees API - Access parliamentary committee information
	/// </summary>
	public ICommitteesApi Committees { get; }

	/// <summary>
	/// Commons Divisions API - Access House of Commons voting records
	/// </summary>
	/// <remarks>
	/// WARNING: This API is currently returning HTTP 500 errors from Parliament servers.
	/// See 500_ERROR_ANALYSIS.md for details. Interface is implemented but may not function until Parliament fixes their API.
	/// </remarks>
	public ICommonsDivisionsApi CommonsDivisions { get; }

	/// <summary>
	/// Lords Divisions API - Access House of Lords voting records
	/// </summary>
	/// <remarks>
	/// WARNING: This API is currently returning HTTP 500 errors from Parliament servers.
	/// See 500_ERROR_ANALYSIS.md for details. Interface is implemented but may not function until Parliament fixes their API.
	/// </remarks>
	public ILordsDivisionsApi LordsDivisions { get; }

	/// <summary>
	/// Member Interests API - Access the Register of Members' Financial Interests
	/// </summary>
	public IInterestsApi Interests { get; }

	/// <summary>
	/// Written Questions and Statements API - Access parliamentary questions and ministerial statements
	/// </summary>
	public IQuestionsStatementsApi QuestionsStatements { get; }

	/// <summary>
	/// Oral Questions and Motions API - Access oral questions and parliamentary motions
	/// </summary>
	public IOralQuestionsMotionsApi OralQuestionsMotions { get; }

	/// <summary>
	/// Treaties API - Access international treaties laid before Parliament
	/// </summary>
	public ITreatiesApi Treaties { get; }

	/// <summary>
	/// Erskine May API - Access the authoritative guide to parliamentary procedure
	/// </summary>
	public IErskineMayApi ErskineMay { get; }

	/// <summary>
	/// NOW (Annunciator) API - Real-time chamber status and business information
	/// </summary>
	public INowApi Now { get; }

	/// <summary>
	/// Constructor with options (creates own HttpClient)
	/// </summary>
	/// <param name="options">Configuration options</param>
	public ParliamentClient(ParliamentClientOptions? options = null)
	{
		options ??= new ParliamentClientOptions();
		_options = options;

		_ownedHttpClient = new HttpClient
		{
			Timeout = options.Timeout
		};
		_disposeHttpClient = true;

		ConfigureHttpClient(_ownedHttpClient, options);

		var refitSettings = GetRefitSettings(options);

		// Initialize implemented API interfaces
		Petitions = CreateApi<IPetitionsApi>(options.PetitionsBaseUrl, refitSettings);
		Members = CreateApi<IMembersApi>(options.MembersBaseUrl, refitSettings);
		Bills = CreateApi<IBillsApi>(options.BillsBaseUrl, refitSettings);
		Committees = CreateApi<ICommitteesApi>(options.CommitteesBaseUrl, refitSettings);

		// Divisions APIs - implemented but currently affected by Parliament API 500 errors
		CommonsDivisions = CreateApi<ICommonsDivisionsApi>(options.CommonsDivisionsBaseUrl, refitSettings);
		LordsDivisions = CreateApi<ILordsDivisionsApi>(options.LordsDivisionsBaseUrl, refitSettings);

		// Interests API
		Interests = CreateApi<IInterestsApi>(options.InterestsBaseUrl, refitSettings);

		// Questions & Statements API
		QuestionsStatements = CreateApi<IQuestionsStatementsApi>(options.QuestionsStatementsBaseUrl, refitSettings);

		// Oral Questions & Motions API
		OralQuestionsMotions = CreateApi<IOralQuestionsMotionsApi>(options.OralQuestionsMotionsBaseUrl, refitSettings);

		// Treaties API
		Treaties = CreateApi<ITreatiesApi>(options.TreatiesBaseUrl, refitSettings);

		// Erskine May API
		ErskineMay = CreateApi<IErskineMayApi>(options.ErskineMayBaseUrl, refitSettings);

		// NOW (Annunciator) API
		Now = CreateApi<INowApi>(options.NowBaseUrl, refitSettings);
	}

	/// <summary>
	/// Constructor with custom HttpClient (for dependency injection)
	/// </summary>
	/// <param name="httpClient">Pre-configured HttpClient</param>
	/// <param name="options">Configuration options</param>
	public ParliamentClient(HttpClient httpClient, ParliamentClientOptions? options = null)
	{
		ArgumentNullException.ThrowIfNull(httpClient);

		options ??= new ParliamentClientOptions();

		_disposeHttpClient = false;

		ConfigureHttpClient(httpClient, options);

		var refitSettings = GetRefitSettings(options);

		// Initialize implemented API interfaces
		Petitions = CreateApi<IPetitionsApi>(httpClient, options.PetitionsBaseUrl, refitSettings);
		Members = CreateApi<IMembersApi>(httpClient, options.MembersBaseUrl, refitSettings);
		Bills = CreateApi<IBillsApi>(httpClient, options.BillsBaseUrl, refitSettings);
		Committees = CreateApi<ICommitteesApi>(httpClient, options.CommitteesBaseUrl, refitSettings);

		// Divisions APIs - implemented but currently affected by Parliament API 500 errors
		CommonsDivisions = CreateApi<ICommonsDivisionsApi>(httpClient, options.CommonsDivisionsBaseUrl, refitSettings);
		LordsDivisions = CreateApi<ILordsDivisionsApi>(httpClient, options.LordsDivisionsBaseUrl, refitSettings);

		// Interests API
		Interests = CreateApi<IInterestsApi>(httpClient, options.InterestsBaseUrl, refitSettings);
		QuestionsStatements = CreateApi<IQuestionsStatementsApi>(httpClient, options.QuestionsStatementsBaseUrl, refitSettings);
		OralQuestionsMotions = CreateApi<IOralQuestionsMotionsApi>(httpClient, options.OralQuestionsMotionsBaseUrl, refitSettings);
		Treaties = CreateApi<ITreatiesApi>(httpClient, options.TreatiesBaseUrl, refitSettings);
		ErskineMay = CreateApi<IErskineMayApi>(httpClient, options.ErskineMayBaseUrl, refitSettings);
		Now = CreateApi<INowApi>(httpClient, options.NowBaseUrl, refitSettings);
	}

	private static void ConfigureHttpClient(HttpClient httpClient, ParliamentClientOptions options)
	{
		httpClient.DefaultRequestHeaders.UserAgent.Clear();
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
		httpClient.DefaultRequestHeaders.Accept.Clear();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	private T CreateApi<T>(string baseUrl, RefitSettings settings)
	{
		if (_ownedHttpClient is null)
		{
			throw new InvalidOperationException("Cannot create API without owned HttpClient");
		}

		// Create handler chain with logging
		HttpMessageHandler handler = new HttpClientHandler();
		if (_options?.Logger != null)
		{
			handler = new LoggingHttpMessageHandler(handler, _options.Logger, _options.EnableVerboseLogging);
		}

		var httpClient = new HttpClient(handler)
		{
			BaseAddress = new Uri(baseUrl),
			Timeout = _ownedHttpClient.Timeout
		};

		// Copy headers from owned client
		foreach (var header in _ownedHttpClient.DefaultRequestHeaders)
		{
			_ = httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
		}

		return RestService.For<T>(httpClient, settings);
	}

	private static T CreateApi<T>(HttpClient sourceHttpClient, string baseUrl, RefitSettings settings)
	{
		var httpClient = new HttpClient()
		{
			BaseAddress = new Uri(baseUrl),
			Timeout = sourceHttpClient.Timeout
		};

		// Copy headers from source client
		foreach (var header in sourceHttpClient.DefaultRequestHeaders)
		{
			_ = httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
		}

		return RestService.For<T>(httpClient, settings);
	}

	private static RefitSettings GetRefitSettings(ParliamentClientOptions options) => new()
	{
		ContentSerializer = new SystemTextJsonContentSerializer(
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
					DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
					UnmappedMemberHandling = options.EnableDebugValidation
						? JsonUnmappedMemberHandling.Disallow
						: JsonUnmappedMemberHandling.Skip
				})
	};

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
		CancellationToken cancellationToken) => request switch
		{
			GetBillsRequest r => (IAsyncEnumerable<TItem>)GetAllBillsCore(r, cancellationToken),
			GetPetitionsRequest r => (IAsyncEnumerable<TItem>)GetAllPetitionsCore(r, cancellationToken),
			SearchMembersRequest r => (IAsyncEnumerable<TItem>)GetAllMembersCore(r, cancellationToken),
			SearchConstituenciesRequest r => (IAsyncEnumerable<TItem>)GetAllConstituenciesCore(r, cancellationToken),
			GetCommitteesRequest r => (IAsyncEnumerable<TItem>)GetAllCommitteesCore(r, cancellationToken),
			SearchInterestsRequest r => (IAsyncEnumerable<TItem>)GetAllInterestsCore(r, cancellationToken),
			GetWrittenQuestionsRequest r => (IAsyncEnumerable<TItem>)GetAllWrittenQuestionsCore(r, cancellationToken),
			GetWrittenStatementsRequest r => (IAsyncEnumerable<TItem>)GetAllWrittenStatementsCore(r, cancellationToken),
			GetDailyReportsRequest r => (IAsyncEnumerable<TItem>)GetAllDailyReportsCore(r, cancellationToken),
			GetOralQuestionsRequest r => (IAsyncEnumerable<TItem>)GetAllOralQuestionsCore(r, cancellationToken),
			GetMotionsRequest r => (IAsyncEnumerable<TItem>)GetAllMotionsCore(r, cancellationToken),
			GetTreatiesRequest r => (IAsyncEnumerable<TItem>)GetAllTreatiesCore(r, cancellationToken),
			_ => throw new NotSupportedException($"Pagination is not supported for request type {request.GetType().Name}.")
		};

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

	#region Pagination core methods

	private IAsyncEnumerable<Bill> GetAllBillsCore(GetBillsRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => Bills.GetBillsAsync(r, ct),
			static resp => resp.Items,
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private IAsyncEnumerable<Petition> GetAllPetitionsCore(GetPetitionsRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.PageSize ?? 50;
		return PaginationHelper.GetAllPageAsync(
			request, pageSize,
			static (c, page, size) => c with { Page = page, PageSize = size },
			(r, ct) => Petitions.GetAsync(r, ct),
			static resp => resp.Data,
			cancellationToken);
	}

	private IAsyncEnumerable<Member> GetAllMembersCore(SearchMembersRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => Members.SearchAsync(r, ct),
			static resp => resp.Items?.ConvertAll(static i => i.Value)
				?? resp.Results?.ConvertAll(static i => i.Value),
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private IAsyncEnumerable<Constituency> GetAllConstituenciesCore(SearchConstituenciesRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => Members.SearchConstituenciesAsync(r, ct),
			static resp => resp.Items?.ConvertAll(static i => i.Value)
				?? resp.Results?.ConvertAll(static i => i.Value),
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private IAsyncEnumerable<Committee> GetAllCommitteesCore(GetCommitteesRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => Committees.GetCommitteesAsync(r, ct),
			static resp => resp.Items,
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private IAsyncEnumerable<Interest> GetAllInterestsCore(SearchInterestsRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => Interests.SearchInterestsAsync(r, ct),
			static resp => resp.Items,
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private IAsyncEnumerable<WrittenQuestion> GetAllWrittenQuestionsCore(GetWrittenQuestionsRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => QuestionsStatements.GetWrittenQuestionsAsync(r, ct),
			static resp => GetWrappedItems(resp),
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private IAsyncEnumerable<WrittenStatement> GetAllWrittenStatementsCore(GetWrittenStatementsRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => QuestionsStatements.GetWrittenStatementsAsync(r, ct),
			static resp => GetWrappedItems(resp),
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private IAsyncEnumerable<DailyReport> GetAllDailyReportsCore(GetDailyReportsRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => QuestionsStatements.GetDailyReportsAsync(r, ct),
			static resp => GetWrappedItems(resp),
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private IAsyncEnumerable<OralQuestion> GetAllOralQuestionsCore(GetOralQuestionsRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => OralQuestionsMotions.GetOralQuestionsAsync(r, ct),
			static resp => resp.Response,
			static resp => resp.PagingInfo.Total,
			cancellationToken);
	}

	private IAsyncEnumerable<Motion> GetAllMotionsCore(GetMotionsRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => OralQuestionsMotions.GetMotionsAsync(r, ct),
			static resp => resp.Response,
			static resp => resp.PagingInfo.Total,
			cancellationToken);
	}

	private IAsyncEnumerable<Treaty> GetAllTreatiesCore(GetTreatiesRequest request, CancellationToken cancellationToken)
	{
		var pageSize = request.Take ?? 20;
		return PaginationHelper.GetAllOffsetAsync(
			request, pageSize,
			static (c, skip, take) => c with { Skip = skip, Take = take },
			(r, ct) => Treaties.GetTreatiesAsync(r, ct),
			static resp => resp.Items?.ConvertAll(static i => i.Value)
				?? resp.Results?.ConvertAll(static i => i.Value),
			static resp => resp.TotalResults,
			cancellationToken);
	}

	private static List<T>? GetWrappedItems<T>(PaginatedResponse<T>? response)
		=> response?.Items?.ConvertAll(static item => item.Value)
			?? response?.Results?.ConvertAll(static item => item.Value);

	#endregion

	/// <summary>
	/// Dispose of resources
	/// </summary>
	public void Dispose()
	{
		if (_disposeHttpClient)
		{
			_ownedHttpClient?.Dispose();
		}

		GC.SuppressFinalize(this);
	}
}
