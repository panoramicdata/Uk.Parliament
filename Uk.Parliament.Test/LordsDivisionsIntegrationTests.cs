using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Lords Divisions API (requires live API)
/// </summary>
/// <remarks>
/// WARNING: As of January 2025, the Lords Divisions API is returning HTTP 500 errors.
/// These tests are skipped until Parliament resolves the server-side issues.
/// See 500_ERROR_ANALYSIS.md for full details and diagnostic logs.
/// </remarks>
public class LordsDivisionsIntegrationTests(ITestOutputHelper output)
{
	private ParliamentClient CreateClient()
	{
		var loggerFactory = new XUnitLoggerFactory(output, LogLevel.Debug);
		var logger = loggerFactory.CreateLogger("ParliamentClient");

		return new ParliamentClient(new ParliamentClientOptions
		{
			Logger = logger,
			EnableVerboseLogging = true,
			EnableDebugValidation = false
		});
	}

	[Fact]
	public async Task GetDivisionsAsync_WithNoFilters_Succeeds()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var divisions = await client.LordsDivisions.GetDivisionsAsync();

		// Assert
		_ = divisions.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact]
	public async Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var division = await client.LordsDivisions.GetDivisionByIdAsync(1);

		// Assert
		_ = division.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact]
	public async Task GetDivisionGroupedByPartyAsync_WithValidId_ReturnsGroupedVotes()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var groupedVotes = await client.LordsDivisions.GetDivisionGroupedByPartyAsync(1);

		// Assert
		_ = groupedVotes.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact]
	public async Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var divisions = await client.LordsDivisions.SearchDivisionsAsync("Amendment");

		// Assert
		_ = divisions.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact]
	public async Task GetDivisionsAsync_WithPagination_Succeeds()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var page1 = await client.LordsDivisions.GetDivisionsAsync(skip: 0, take: 10);
		var page2 = await client.LordsDivisions.GetDivisionsAsync(skip: 10, take: 10);

		// Assert
		_ = page1.Should().NotBeNull();
		_ = page2.Should().NotBeNull();
		await Task.CompletedTask;
	}
}
