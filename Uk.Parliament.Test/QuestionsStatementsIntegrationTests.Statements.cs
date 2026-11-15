using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Written Statements and Daily Reports
/// </summary>
[Trait("Category", "Integration")]
public partial class QuestionsStatementsIntegrationTests
{
	private Task<PaginatedResponse<WrittenStatement>> GetStatementsAsync(GetWrittenStatementsRequest request)
		=> Client.QuestionsStatements.GetWrittenStatementsAsync(request, CancellationToken);

	/// <summary>Verifies that fetching written statements without filters returns a valid paginated result.</summary>
	[Fact]
	public async Task GetWrittenStatementsAsync_WithNoFilters_Succeeds()
		=> AssertValidPaginatedResponse(await GetStatementsAsync(new() { Take = 10 }));

	/// <summary>Verifies that filtering written statements by date range returns statements within that range.</summary>
	[Fact]
	public async Task GetWrittenStatementsAsync_FilterByDateRange_ReturnsStatements()
	{
		var (from, to) = LastMonth();

		AssertValidPaginatedResponse(
			await GetStatementsAsync(new() { MadeWhenFrom = from, MadeWhenTo = to, Take = 10 }),
			item => _ = item.Value.DateMade.Should().BeOnOrAfter(from).And.BeOnOrBefore(to));
	}

	/// <summary>Verifies that fetching a written statement by a valid ID returns that specific statement.</summary>
	[Fact]
	public async Task GetWrittenStatementByIdAsync_WithValidId_ReturnsStatement()
	{
		if (FirstValue(await GetStatementsAsync(new() { Take = 1 })) is not { } first)
		{
			return;
		}

		var result = await Client.QuestionsStatements.GetWrittenStatementByIdAsync(first.Id, CancellationToken);

		_ = result.Value.Id.Should().Be(first.Id);
	}

	/// <summary>Verifies that streaming all written statements via async enumerable completes without error.</summary>
	[Fact]
	public async Task GetAllWrittenStatementsAsync_StreamsResults()
		=> _ = (await CollectStreamedItemsAsync(
			Client.GetAllAsync(new GetWrittenStatementsRequest { Take = 5 }, CancellationToken))).Should().NotBeNull();

	/// <summary>Verifies that fetching daily reports within a date range returns the reports collection.</summary>
	[Fact]
	public async Task GetDailyReportsAsync_WithDateRange_ReturnsReports()
	{
		var result = await Client.QuestionsStatements.GetDailyReportsAsync(
			new GetDailyReportsRequest { DateFrom = DateTime.Now.AddDays(-7), DateTo = DateTime.Now, Take = 10 },
			CancellationToken);

		_ = result.Items.Should().NotBeNull();
	}

	/// <summary>Verifies that streaming daily reports via async enumerable completes without error.</summary>
	[Fact]
	public async Task GetAllDailyReportsAsync_StreamsResults()
		=> _ = (await CollectStreamedItemsAsync(
			Client.GetAllAsync(new GetDailyReportsRequest { DateFrom = DateTime.Now.AddDays(-30), Take = 5 }, CancellationToken))).Should().NotBeNull();
}
