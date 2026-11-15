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
public partial class ParliamentClient : IDisposable
{
	/// <summary>Page size used when a skip/take request does not specify one.</summary>
	private const int DefaultTakePageSize = 20;

	/// <summary>Page size used when a Petitions request does not specify one.</summary>
	private const int DefaultPetitionsPageSize = 50;

	private readonly HttpClient? _ownedHttpClient;
	private readonly ParliamentClientOptions _options;
	private readonly Func<HttpMessageHandler> _innerHandlerFactory;

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
	/// Constructor using the default options (creates own HttpClient)
	/// </summary>
	public ParliamentClient() : this(new ParliamentClientOptions())
	{
	}

	/// <summary>
	/// Constructor with options (creates own HttpClient)
	/// </summary>
	/// <param name="options">Configuration options</param>
	public ParliamentClient(ParliamentClientOptions options) : this(options, static () => new HttpClientHandler())
	{
	}

	internal ParliamentClient(HttpMessageHandler innerHandler, ParliamentClientOptions options) : this(options, () => innerHandler)
	{
	}

	private ParliamentClient(ParliamentClientOptions options, Func<HttpMessageHandler> innerHandlerFactory)
	{
		ArgumentNullException.ThrowIfNull(options);
		_options = options;
		_innerHandlerFactory = innerHandlerFactory;

		_ownedHttpClient = new HttpClient
		{
			Timeout = options.Timeout
		};
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
	/// Constructor with custom HttpClient and the default options (for dependency injection)
	/// </summary>
	/// <param name="httpClient">Pre-configured HttpClient</param>
	public ParliamentClient(HttpClient httpClient) : this(httpClient, new ParliamentClientOptions())
	{
	}

	/// <summary>
	/// Constructor with custom HttpClient (for dependency injection)
	/// </summary>
	/// <param name="httpClient">Pre-configured HttpClient</param>
	/// <param name="options">Configuration options</param>
	public ParliamentClient(HttpClient httpClient, ParliamentClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(httpClient);
		ArgumentNullException.ThrowIfNull(options);

		_options = options;
		_innerHandlerFactory = static () => new HttpClientHandler();

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
		var httpClient = new HttpClient(BuildHandler())
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

	private T CreateApi<T>(HttpClient sourceHttpClient, string baseUrl, RefitSettings settings)
	{
		var httpClient = new HttpClient(BuildHandler())
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

	private ReadOnlyHttpMessageHandler BuildHandler()
	{
		var handler = _innerHandlerFactory();
		if (_options.Logger != null)
		{
			handler = new LoggingHttpMessageHandler(handler, _options.Logger, _options.EnableVerboseLogging);
		}

		return new ReadOnlyHttpMessageHandler(handler, _options);
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
	/// Dispose of resources
	/// </summary>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Dispose of resources
	/// </summary>
	/// <param name="disposing">True when called from <see cref="Dispose()"/></param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_ownedHttpClient?.Dispose();
		}
	}
}