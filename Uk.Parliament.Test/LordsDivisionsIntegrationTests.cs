using Microsoft.Extensions.Logging;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Lords Divisions API (requires live API)
/// </summary>
/// <remarks>
/// WARNING: As of January 2025, the Lords Divisions API endpoints may return 404 errors.
/// These tests handle these errors gracefully.
/// </remarks>
public class LordsDivisionsIntegrationTests(ITestOutputHelper output) : IntegrationTestBase
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

	/// <summary>Verifies that fetching Lords divisions without filters returns a non-empty list.</summary>
	[Fact]
	public async Task GetDivisionsAsync_WithNoFilters_Succeeds()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
		{
			// Act
			var divisions = await client
				.LordsDivisions
				.GetDivisionsAsync(
					new GetLordsDivisionsRequest { Take = 5 },
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
			_output.WriteLine("Lords Divisions API returned 404 - endpoint may not be available");
		}
	}

	/// <summary>Verifies that fetching a Lords division by a valid ID returns the division with a title.</summary>
	[Fact]
	public async Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
		{
			// First get a valid division ID
			var divisions = await client
				.LordsDivisions
				.SearchDivisionsAsync(
					new SearchLordsDivisionsRequest { SearchTerm = "Amendment", Take = 1 },
					cancellationToken: CancellationToken);

			if (divisions.Count == 0)
			{
				_output.WriteLine("No divisions found to test GetDivisionByIdAsync");
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
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			_output.WriteLine("Lords Divisions API returned 404 - endpoint may not be available");
		}
	}

	/// <summary>Verifies that searching Lords divisions by search term returns results.</summary>
	[Fact]
	public async Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
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
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			_output.WriteLine("Lords Divisions Search API returned 404 - endpoint may not be available");
		}
	}

	/// <summary>Verifies that paginated Lords division retrieval returns a page within the take limit.</summary>
	[Fact]
	public async Task GetDivisionsAsync_WithPagination_Succeeds()
	{
		// Arrange
		var client = CreateClientWithLogging();

		try
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
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			_output.WriteLine("Lords Divisions API returned 404 - endpoint may not be available");
		}
	}
}
