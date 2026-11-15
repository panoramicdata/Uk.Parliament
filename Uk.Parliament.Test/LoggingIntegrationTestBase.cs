using Microsoft.Extensions.Logging;
using System.Net;

namespace Uk.Parliament.Test;

/// <summary>
/// Base class for integration tests that log their HTTP traffic to the test output and tolerate
/// the 404s that some Parliament endpoints return intermittently.
/// </summary>
/// <remarks>
/// The divisions APIs in particular go missing for stretches at a time, so every one of their tests
/// needs the same logging client and the same 404 tolerance. Keeping both here means a test body is
/// just its act and assert steps.
/// </remarks>
public abstract class LoggingIntegrationTestBase(ITestOutputHelper output) : IntegrationTestBase
{
	/// <summary>Test output for the running test.</summary>
	protected ITestOutputHelper Output { get; } = output;

	/// <summary>
	/// Creates a client that writes verbose request and response logging to the test output.
	/// </summary>
	protected ParliamentClient CreateClientWithLogging()
	{
		var loggerFactory = new XUnitLoggerFactory(Output, LogLevel.Debug);
		var logger = loggerFactory.CreateLogger("ParliamentClient");

		return new ParliamentClient(new ParliamentClientOptions
		{
			Logger = logger,
			EnableVerboseLogging = true,
			EnableDebugValidation = false
		});
	}

	/// <summary>
	/// Runs a test body against a live endpoint, reporting a 404 as an unavailable endpoint rather
	/// than failing the test.
	/// </summary>
	/// <param name="apiName">Name used in the message written to the test output on a 404.</param>
	/// <param name="testBody">The test body, given a client that logs to the test output.</param>
	protected async Task RunTolerating404Async(string apiName, Func<ParliamentClient, Task> testBody)
	{
		using var client = CreateClientWithLogging();

		try
		{
			await testBody(client);
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			Output.WriteLine($"{apiName} returned 404 - endpoint may not be available");
		}
	}

	/// <summary>
	/// Finds one division with <paramref name="findOne"/>, fetches it by identifier and checks the round trip.
	/// </summary>
	/// <typeparam name="T">The division type.</typeparam>
	/// <param name="apiName">Name used in the message written to the test output on a 404.</param>
	/// <param name="findOne">Searches for a single division.</param>
	/// <param name="getById">Fetches a division by identifier.</param>
	/// <param name="divisionId">Reads the identifier from a division.</param>
	/// <param name="title">Reads the title from a division.</param>
	protected Task RunDivisionByIdAsync<T>(
		string apiName,
		Func<ParliamentClient, Task<List<T>>> findOne,
		Func<ParliamentClient, int, Task<T>> getById,
		Func<T, int> divisionId,
		Func<T, string?> title)
		=> RunTolerating404Async(apiName, async client =>
		{
			var found = await findOne(client);

			if (found.Count == 0)
			{
				Output.WriteLine("No divisions found to test GetDivisionByIdAsync");
				return;
			}

			var id = divisionId(found[0]);
			var result = await getById(client, id);

			_ = divisionId(result).Should().Be(id);
			_ = title(result).Should().NotBeNullOrEmpty();
		});

	/// <summary>
	/// Fetches a page and checks that it did not exceed the requested take.
	/// </summary>
	/// <typeparam name="T">The division type.</typeparam>
	/// <param name="apiName">Name used in the message written to the test output on a 404.</param>
	/// <param name="fetch">Fetches the page.</param>
	/// <param name="take">The take that was requested.</param>
	protected Task RunPageWithinTakeAsync<T>(string apiName, Func<ParliamentClient, Task<List<T>>> fetch, int take)
		=> RunTolerating404Async(apiName, async client =>
		{
			var page = await fetch(client);

			_ = page.Should().NotBeNull();
			_ = page.Count.Should().BeLessThanOrEqualTo(take);
		});
	/// <summary>
	/// Asserts that a divisions list came back non-empty and that every entry has a positive
	/// identifier and a title.
	/// </summary>
	/// <typeparam name="T">The division type, which differs between the Commons and Lords APIs.</typeparam>
	/// <param name="divisions">The divisions returned by the API.</param>
	/// <param name="divisionId">Reads the identifier from a division.</param>
	/// <param name="title">Reads the title from a division.</param>
	protected static void AssertDivisionsValid<T>(
		IReadOnlyList<T> divisions,
		Func<T, int> divisionId,
		Func<T, string?> title)
	{
		_ = divisions.Should().NotBeNull();
		_ = divisions.Should().NotBeEmpty();
		_ = divisions.Should().AllSatisfy(division =>
		{
			_ = divisionId(division).Should().BePositive();
			_ = title(division).Should().NotBeNullOrEmpty();
		});
	}
}
