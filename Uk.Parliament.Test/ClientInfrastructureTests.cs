using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Uk.Parliament.Extensions;

namespace Uk.Parliament.Test;

/// <summary>Offline tests for dependency injection, HTTP logging and the small model helpers.</summary>
public class ClientInfrastructureTests
{
	private sealed class ListLogger : ILogger
	{
		public List<(LogLevel Level, string Message)> Entries { get; } = [];

		public List<object> Scopes { get; } = [];

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		{
			Scopes.Add(state);
			return null;
		}

		public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
			=> Entries.Add((logLevel, $"{eventId.Id}{formatter(state, exception)}"));
	}

	/// <summary>Registering the client resolves a working instance.</summary>
	[Fact]
	public void AddUkParliamentClient_ResolvesClient()
	{
		using var provider = new ServiceCollection().AddUkParliamentClient().BuildServiceProvider();
		using var scope = provider.CreateScope();

		var client = scope.ServiceProvider.GetRequiredService<ParliamentClient>();

		_ = client.Petitions.Should().NotBeNull();
	}

	/// <summary>The configure callback reaches the registered options.</summary>
	[Fact]
	public void AddUkParliamentClient_AppliesConfigureCallback()
	{
		using var provider = new ServiceCollection()
			.AddUkParliamentClient(options => options.IsReadOnly = true)
			.BuildServiceProvider();

		var options = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<ParliamentClientOptions>>().Value;

		_ = options.IsReadOnly.Should().BeTrue();
	}

	/// <summary>The resilient registration wires up a named HttpClient that carries the configured user agent.</summary>
	[Fact]
	public void AddUkParliamentClientWithResilience_RegistersHttpClient()
	{
		using var provider = new ServiceCollection()
			.AddUkParliamentClientWithResilience(options => options.UserAgent = "Test/1.0")
			.BuildServiceProvider();

		var http = provider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(ParliamentClient));

		_ = http.DefaultRequestHeaders.UserAgent.ToString().Should().Contain("Test/1.0");
		_ = new ServiceCollection().AddUkParliamentClientWithResilience().Should().NotBeNull();
	}

	/// <summary>The HttpClient constructor applies the user agent and builds every API.</summary>
	[Fact]
	public void Constructor_WithHttpClient_BuildsEveryApi()
	{
		using var http = new HttpClient();
		using var client = new ParliamentClient(http);

		_ = client.Now.Should().NotBeNull();
		_ = http.DefaultRequestHeaders.UserAgent.ToString().Should().Contain("Uk.Parliament");
	}

	/// <summary>A null options argument is rejected.</summary>
	[Fact]
	public void Constructor_WithNullOptions_Throws()
	{
		var act = () => new ParliamentClient((ParliamentClientOptions)null!);

		_ = act.Should().Throw<ArgumentNullException>();
	}

	/// <summary>Verbose logging records the request and the response.</summary>
	[Fact]
	public async Task LoggingHandler_WhenVerbose_LogsRequestAndResponse()
	{
		var logger = new ListLogger();
		var stub = new StubHttpMessageHandler(_ => """{"items":[],"totalResults":0,"itemsPerPage":0}""");
		using var client = new ParliamentClient(stub, new ParliamentClientOptions { Logger = logger, EnableVerboseLogging = true });

		_ = await client.Bills.GetBillsAsync(new GetBillsRequest(), TestContext.Current.CancellationToken);

		_ = logger.Entries.Should().Contain(e => e.Message.Contains("HTTP REQUEST", StringComparison.Ordinal));
		_ = logger.Entries.Should().Contain(e => e.Message.Contains("totalResults", StringComparison.Ordinal));
	}

	/// <summary>Without verbose logging the response is still logged but its successful body is not.</summary>
	[Fact]
	public async Task LoggingHandler_WhenNotVerbose_OmitsRequestAndBody()
	{
		var logger = new ListLogger();
		var stub = new StubHttpMessageHandler(_ => """{"items":[],"totalResults":0,"itemsPerPage":0}""");
		using var client = new ParliamentClient(stub, new ParliamentClientOptions { Logger = logger, EnableVerboseLogging = false });

		_ = await client.Bills.GetBillsAsync(new GetBillsRequest(), TestContext.Current.CancellationToken);

		_ = logger.Entries.Should().ContainSingle().Which.Message.Should().Contain("HTTP RESPONSE").And.NotContain("totalResults");
	}

	/// <summary>A failing request is logged as an error and rethrown.</summary>
	[Fact]
	public async Task LoggingHandler_WhenRequestFails_LogsErrorAndRethrows()
	{
		var logger = new ListLogger();
		using var client = new ParliamentClient(new StubHttpMessageHandler(_ => throw new HttpRequestException("boom")), new ParliamentClientOptions { Logger = logger });

		var act = () => client.Bills.GetBillsAsync(new GetBillsRequest(), TestContext.Current.CancellationToken);

		_ = await act.Should().ThrowAsync<Refit.ApiRequestException>();
		_ = logger.Entries.Should().Contain(e => e.Level == LogLevel.Error && e.Message.Contains("Request failed", StringComparison.Ordinal));
	}

	/// <summary>The query renders every set filter and nothing when none is set.</summary>
	[Fact]
	public void Query_ToString_RendersSetFilters()
	{
		var full = new Query { Text = "rail", State = PetitionState.Open, PageSize = 5, PageNumber = 2 };
		var bare = new Query { Text = " " };

		_ = full.ToString().Should().Be("?search=rail&state=open&_pageSize=5&page=2");
		_ = bare.ToString().Should().BeEmpty();
	}

	/// <summary>A successful result exposes its data and a failed one its exception.</summary>
	[Fact]
	public void Result_ExposesDataOrException()
	{
		var ok = new Result<int>(7);
		var failed = new Result<int>(new InvalidOperationException("bad"));

		_ = ok.Ok.Should().BeTrue();
		_ = ok.ToString().Should().Be("7");
		_ = failed.Ok.Should().BeFalse();
		_ = failed.Exception.Should().NotBeNull();
		_ = failed.ToString().Should().Contain("bad");
	}
}
