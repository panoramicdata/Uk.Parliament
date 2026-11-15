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

	private static Task<List<Models.Divisions.LordsDivision>> SearchAsync(ParliamentClient client, SearchLordsDivisionsRequest request)
		=> client.LordsDivisions.SearchDivisionsAsync(request, cancellationToken: CancellationToken);

	/// <summary>Verifies that fetching Lords divisions without filters returns a non-empty list.</summary>
	[Fact]
	public Task GetDivisionsAsync_WithNoFilters_Succeeds()
		=> RunTolerating404Async(DivisionsApi, async client =>
			AssertDivisionsValid(
				await client.LordsDivisions.GetDivisionsAsync(new GetLordsDivisionsRequest { Take = 5 }, cancellationToken: CancellationToken),
				d => d.DivisionId,
				d => d.Title));

	/// <summary>Verifies that fetching a Lords division by a valid ID returns the division with a title.</summary>
	[Fact]
	public Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
		=> RunDivisionByIdAsync(
			DivisionsApi,
			client => SearchAsync(client, new SearchLordsDivisionsRequest { SearchTerm = "Amendment", Take = 1 }),
			(client, id) => client.LordsDivisions.GetDivisionByIdAsync(id, CancellationToken),
			d => d.DivisionId,
			d => d.Title);

	/// <summary>Verifies that searching Lords divisions by search term returns results.</summary>
	[Fact]
	public Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
		=> RunTolerating404Async(SearchApi, async client =>
			AssertDivisionsValid(
				await SearchAsync(client, new SearchLordsDivisionsRequest { SearchTerm = "Amendment" }),
				d => d.DivisionId,
				d => d.Title));

	/// <summary>Verifies that paginated Lords division retrieval returns a page within the take limit.</summary>
	[Fact]
	public Task GetDivisionsAsync_WithPagination_Succeeds()
		=> RunPageWithinTakeAsync(
			DivisionsApi,
			client => client.LordsDivisions.GetDivisionsAsync(new GetLordsDivisionsRequest { Skip = 0, Take = 10 }, cancellationToken: CancellationToken),
			10);
}
