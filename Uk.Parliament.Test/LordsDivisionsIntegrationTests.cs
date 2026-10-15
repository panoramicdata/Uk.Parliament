namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Lords Divisions API (requires live API)
/// </summary>
/// <remarks>
/// WARNING: As of January 2025, the Lords Divisions API endpoints may return 404 errors.
/// These tests handle these errors gracefully.
/// </remarks>
[Trait("Category", "Integration")]
public class LordsDivisionsIntegrationTests(ITestOutputHelper output) : LoggingIntegrationTestBase(output)
{
	private const string DivisionsApi = "Lords Divisions API";
	private const string SearchApi = "Lords Divisions Search API";

	/// <summary>Executes an ILordsDivisionsApi call with the test cancellation token.</summary>
	private static Task<T> CallAsync<T>(ParliamentClient client, Func<ILordsDivisionsApi, Task<T>> call)
		=> call(client.LordsDivisions);

	/// <summary>Verifies that fetching Lords divisions without filters returns a non-empty list.</summary>
	[Fact]
	public Task GetDivisionsAsync_WithNoFilters_Succeeds()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			var divisions = await CallAsync(client, api => api.GetDivisionsAsync(
				new GetLordsDivisionsRequest { Take = 5 },
				cancellationToken: CancellationToken));

			AssertDivisionsValid(divisions, d => d.DivisionId, d => d.Title);
		});

	/// <summary>Verifies that fetching a Lords division by a valid ID returns the division with a title.</summary>
	[Fact]
	public Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			var divisions = await CallAsync(client, api => api.SearchDivisionsAsync(
				new SearchLordsDivisionsRequest { SearchTerm = "Amendment", Take = 1 },
				cancellationToken: CancellationToken));

			if (divisions.Count == 0)
			{
				Output.WriteLine("No divisions found to test GetDivisionByIdAsync");
				return;
			}

			var divisionId = divisions[0].DivisionId;
			var result = await CallAsync(client, api => api.GetDivisionByIdAsync(divisionId, CancellationToken));

			_ = result.Should().NotBeNull();
			_ = result.DivisionId.Should().Be(divisionId);
			_ = result.Title.Should().NotBeNullOrEmpty();
		});

	/// <summary>Verifies that searching Lords divisions by search term returns results.</summary>
	[Fact]
	public Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
		=> RunTolerating404Async(SearchApi, async client =>
		{
			var divisions = await CallAsync(client, api => api.SearchDivisionsAsync(
				new SearchLordsDivisionsRequest { SearchTerm = "Amendment" },
				cancellationToken: CancellationToken));

			_ = divisions.Should().NotBeNull();
			_ = divisions.Should().NotBeEmpty();
			_ = divisions[0].DivisionId.Should().BePositive();
		});

	/// <summary>Verifies that paginated Lords division retrieval returns a page within the take limit.</summary>
	[Fact]
	public Task GetDivisionsAsync_WithPagination_Succeeds()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			var page1 = await CallAsync(client, api => api.GetDivisionsAsync(
				new GetLordsDivisionsRequest { Skip = 0, Take = 10 },
				cancellationToken: CancellationToken));

			_ = page1.Should().NotBeNull();
			_ = page1.Count.Should().BeLessThanOrEqualTo(10);
		});
}
