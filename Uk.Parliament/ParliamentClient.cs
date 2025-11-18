using Refit;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Uk.Parliament.Interfaces;

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
