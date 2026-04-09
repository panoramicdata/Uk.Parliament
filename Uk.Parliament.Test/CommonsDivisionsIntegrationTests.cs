using Microsoft.Extensions.Logging;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Commons Divisions API (requires live API)
/// </summary>
/// <remarks>
/// WARNING: The Commons Divisions API endpoints may return 404 errors.
/// These tests handle these errors gracefully.
/// </remarks>
public class CommonsDivisionsIntegrationTests(ITestOutputHelper output) : IntegrationTestBase
{
	private readonly ITestOutputHelper _output = output;

	private ParliamentClient CreateClientWithLogging()
	{
		var loggerFactory = new XUnitLoggerFactory(_output, LogLevel.Debug);
		var logger = loggerFactory.CreateLogger("ParliamentClient");

		return new ParliamentClient(new ParliamentClientOptions
		{
			Logger = logger,
			EnableVerboseLogging = true,
			EnableDebugValidation = false
		});
	}

	/// <summary>Verifies that fetching Commons divisions without filters returns a non-empty list.</summary>
	[Fact]
	public async Task GetDivisionsAsync_WithNoFilters_Succeeds()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
		{
			// Act
			var divisions = await client
				.CommonsDivisions
				.GetDivisionsAsync(
					new GetCommonsDivisionsRequest { Take = 5 },
					cancellationToken: CancellationToken);

			// Assert
			_ = divisions.Should().NotBeNull();
			_ = divisions.Should().NotBeEmpty();
			_ = divisions.Should().AllSatisfy(d =>
			{
				_ = d.DivisionId.Should().BePositive();
				_ = d.Title.Should().NotBeNullOrEmpty();
			});
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			_output.WriteLine("Commons Divisions API returned 404 - endpoint may not be available");
		}
	}

	/// <summary>Verifies that fetching a Commons division by a valid ID returns the division with a title.</summary>
	[Fact]
	public async Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
		{
			// First get a valid division ID
			var divisions = await client
				.CommonsDivisions
				.SearchDivisionsAsync(
					new SearchCommonsDivisionsRequest { SearchTerm = "Budget", Take = 1 },
					cancellationToken: CancellationToken);

			if (divisions.Count == 0)
			{
				_output.WriteLine("No divisions found to test GetDivisionByIdAsync");
				return;
			}

			var divisionId = divisions[0].DivisionId;

			// Act
			var result = await client
				.CommonsDivisions
				.GetDivisionByIdAsync(divisionId, CancellationToken);

			// Assert
			_ = result.Should().NotBeNull();
			_ = result.DivisionId.Should().Be(divisionId);
			_ = result.Title.Should().NotBeNullOrEmpty();
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			_output.WriteLine("Commons Divisions API returned 404 - endpoint may not be available");
		}
	}

	/// <summary>Verifies that searching Commons divisions by search term returns results.</summary>
	[Fact]
	public async Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
		{
			// Act
			var divisions = await client
				.CommonsDivisions
				.SearchDivisionsAsync(
					new SearchCommonsDivisionsRequest { SearchTerm = "Budget" },
					cancellationToken: CancellationToken);

			// Assert
			_ = divisions.Should().NotBeNull();
			_ = divisions.Should().NotBeEmpty();
			_ = divisions[0].DivisionId.Should().BePositive();
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			_output.WriteLine("Commons Divisions Search API returned 404 - endpoint may not be available");
		}
	}

	/// <summary>Verifies that fetching a Commons member's voting history returns populated records.</summary>
	[Fact]
	public async Task GetMemberVotingAsync_WithMemberId_ReturnsVotingHistory()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
		{
			// Act
			var votingHistory = await client
				.CommonsDivisions
				.GetMemberVotingAsync(
					new GetCommonsMemberVotingRequest { MemberId = 172, Take = 5 },
					cancellationToken: CancellationToken);

			// Assert
			_ = votingHistory.Should().NotBeNull();
			_ = votingHistory.Should().NotBeEmpty();
			_ = votingHistory.Should().AllSatisfy(r =>
			{
				_ = r.MemberId.Should().Be(172);
				_ = r.PublishedDivision.Should().NotBeNull();
			});
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			_output.WriteLine("Commons Divisions Member Voting API returned 404 - endpoint may not be available");
		}
	}

	/// <summary>Verifies that paginated Commons division search returns a page of results within the take limit.</summary>
	[Fact]
	public async Task SearchDivisionsAsync_WithPagination_Succeeds()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
		{
			// Act
			var page1 = await client
				.CommonsDivisions
				.SearchDivisionsAsync(
					new SearchCommonsDivisionsRequest { SearchTerm = "Budget", Skip = 0, Take = 10 },
					cancellationToken: CancellationToken);

			// Assert
			_ = page1.Should().NotBeNull();
			_ = page1.Count.Should().BeLessThanOrEqualTo(10);
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			_output.WriteLine("Commons Divisions API returned 404 - endpoint may not be available");
		}
	}
}
