namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Lords Divisions API (requires live API)
/// </summary>
/// <remarks>
/// WARNING: As of January 2025, the Lords Divisions API endpoints may return 404 errors.
/// These tests handle these errors gracefully.
/// </remarks>
public class LordsDivisionsIntegrationTests(ITestOutputHelper output) : LoggingIntegrationTestBase(output)
{
	private const string DivisionsApi = "Lords Divisions API";
	private const string SearchApi = "Lords Divisions Search API";

	/// <summary>Verifies that fetching Lords divisions without filters returns a non-empty list.</summary>
	[Fact]
	public Task GetDivisionsAsync_WithNoFilters_Succeeds()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			// Act
			var divisions = await client
				.LordsDivisions
				.GetDivisionsAsync(
					new GetLordsDivisionsRequest { Take = 5 },
					cancellationToken: CancellationToken);

			// Assert
			AssertDivisionsValid(divisions, d => d.DivisionId, d => d.Title);
		});

	/// <summary>Verifies that fetching a Lords division by a valid ID returns the division with a title.</summary>
	[Fact]
	public Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			// Arrange - first get a valid division ID
			var divisions = await client
				.LordsDivisions
				.SearchDivisionsAsync(
					new SearchLordsDivisionsRequest { SearchTerm = "Amendment", Take = 1 },
					cancellationToken: CancellationToken);

			if (divisions.Count == 0)
			{
				Output.WriteLine("No divisions found to test GetDivisionByIdAsync");
				return;
			}

			var divisionId = divisions[0].DivisionId;

			// Act
			var result = await client
				.LordsDivisions
				.GetDivisionByIdAsync(divisionId, CancellationToken);

			// Assert
			_ = result.Should().NotBeNull();
			_ = result.DivisionId.Should().Be(divisionId);
			_ = result.Title.Should().NotBeNullOrEmpty();
		});

	/// <summary>Verifies that searching Lords divisions by search term returns results.</summary>
	[Fact]
	public Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
		=> RunTolerating404Async(SearchApi, async client =>
		{
			// Act
			var divisions = await client
				.LordsDivisions
				.SearchDivisionsAsync(
					new SearchLordsDivisionsRequest { SearchTerm = "Amendment" },
					cancellationToken: CancellationToken);

			// Assert
			_ = divisions.Should().NotBeNull();
			_ = divisions.Should().NotBeEmpty();
			_ = divisions[0].DivisionId.Should().BePositive();
		});

	/// <summary>Verifies that paginated Lords division retrieval returns a page within the take limit.</summary>
	[Fact]
	public Task GetDivisionsAsync_WithPagination_Succeeds()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			// Act
			var page1 = await client
				.LordsDivisions
				.GetDivisionsAsync(
					new GetLordsDivisionsRequest { Skip = 0, Take = 10 },
					cancellationToken: CancellationToken);

			// Assert
			_ = page1.Should().NotBeNull();
			_ = page1.Count.Should().BeLessThanOrEqualTo(10);
		});
}
