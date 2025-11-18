using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Commons Divisions API (requires live API)
/// </summary>
/// <remarks>
/// WARNING: As of January 2025, the Commons Divisions API is returning HTTP 500 errors.
/// These tests are skipped until Parliament resolves the server-side issues.
/// See 500_ERROR_ANALYSIS.md for full details and diagnostic logs.
/// </remarks>
public class CommonsDivisionsIntegrationTests
{
	private readonly ITestOutputHelper _output;

	public CommonsDivisionsIntegrationTests(ITestOutputHelper output)
	{
		_output = output;
	}

	private ParliamentClient CreateClient()
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

	[Fact(Skip = "Commons Divisions API returns 500 errors - Parliament API infrastructure issue")]
	public async Task GetDivisionsAsync_WithNoFilters_Succeeds()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var divisions = await client.CommonsDivisions.GetDivisionsAsync();

		// Assert
		_ = divisions.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact(Skip = "Commons Divisions API returns 500 errors - Parliament API infrastructure issue")]
	public async Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var division = await client.CommonsDivisions.GetDivisionByIdAsync(1);

		// Assert
		_ = division.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact(Skip = "Commons Divisions API returns 500 errors - Parliament API infrastructure issue")]
	public async Task GetDivisionGroupedByPartyAsync_WithValidId_ReturnsGroupedVotes()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var groupedVotes = await client.CommonsDivisions.GetDivisionGroupedByPartyAsync(1);

		// Assert
		_ = groupedVotes.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact(Skip = "Commons Divisions API returns 500 errors - Parliament API infrastructure issue")]
	public async Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var divisions = await client.CommonsDivisions.SearchDivisionsAsync("Budget");

		// Assert
		_ = divisions.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact(Skip = "Commons Divisions API returns 500 errors - Parliament API infrastructure issue")]
	public async Task GetMemberVotingAsync_WithMemberId_ReturnsVotingHistory()
	{
		// Arrange
		var client = CreateClient();

		// Act
		var votingHistory = await client.CommonsDivisions.GetMemberVotingAsync(172);

		// Assert
		_ = votingHistory.Should().NotBeNull();
		await Task.CompletedTask;
	}

	[Fact(Skip = "Commons Divisions API returns 500 errors - Parliament API infrastructure issue")]
	public async Task SearchDivisionsAsync_WithPagination_Succeeds()
	{
		// Arrange
		var client = CreateClient();

		// Act
		// var page1 = await client.CommonsDivisions.SearchDivisionsAsync("Budget", skip: 0, take: 10);
		// var page2 = await client.CommonsDivisions.SearchDivisionsAsync("Budget", skip: 10, take: 10);

		// Assert
		// page1.Should().NotBeNull();
		// page2.Should().NotBeNull();
		await Task.CompletedTask;
	}
}
