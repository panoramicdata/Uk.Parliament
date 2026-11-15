using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Written Questions and Statements API
/// </summary>
[Trait("Category", "Integration")]
public class QuestionsStatementsIntegrationTests : IntegrationTestBase
{
	private const int ExampleMemberId = 172;

	private static (DateTime From, DateTime To) LastMonth() => (DateTime.Now.AddMonths(-1), DateTime.Now);

	/// <summary>Reads the first item of a list response, which the API returns as either Results or Items.</summary>
	private static T? FirstValue<T>(PaginatedResponse<T> page)
		=> page.Results is { Count: > 0 } results ? results[0].Value
			: page.Items is { Count: > 0 } items ? items[0].Value
			: default;

	private Task<PaginatedResponse<WrittenQuestion>> GetQuestionsAsync(GetWrittenQuestionsRequest request)
		=> Client.QuestionsStatements.GetWrittenQuestionsAsync(request, CancellationToken);

	private Task<PaginatedResponse<WrittenStatement>> GetStatementsAsync(GetWrittenStatementsRequest request)
		=> Client.QuestionsStatements.GetWrittenStatementsAsync(request, CancellationToken);

	/// <summary>Verifies that fetching written questions without filters returns a valid paginated result.</summary>
	[Fact]
	public async Task GetWrittenQuestionsAsync_WithNoFilters_Succeeds()
		=> AssertValidPaginatedResponse(await GetQuestionsAsync(new() { Take = 10 }));

	/// <summary>Verifies that filtering written questions by member ID returns questions from that member.</summary>
	[Fact]
	public async Task GetWrittenQuestionsAsync_FilterByMember_ReturnsQuestions()
		=> AssertValidPaginatedResponse(
			await GetQuestionsAsync(new() { AskingMemberId = ExampleMemberId, Take = 10 }),
			item => _ = item.Value.AskingMemberId.Should().Be(ExampleMemberId));

	/// <summary>Verifies that filtering written questions by a date range returns questions within that range.</summary>
	[Fact]
	public async Task GetWrittenQuestionsAsync_FilterByDateRange_ReturnsQuestions()
	{
		var (from, to) = LastMonth();

		AssertValidPaginatedResponse(
			await GetQuestionsAsync(new() { TabledWhenFrom = from, TabledWhenTo = to, Take = 10 }),
			item => _ = item.Value.DateTabled.Should().BeOnOrAfter(from).And.BeOnOrBefore(to));
	}

	/// <summary>Verifies that filtering written questions to answered-only returns questions with answer text.</summary>
	[Fact]
	public async Task GetWrittenQuestionsAsync_FilterByAnswered_ReturnsAnsweredQuestions()
		=> AssertValidPaginatedResponse(
			await GetQuestionsAsync(new() { IsAnswered = true, Take = 10 }),
			item =>
			{
				_ = item.Value.IsAnswered.Should().BeTrue();
				_ = item.Value.AnswerText.Should().NotBeNullOrEmpty();
			});

	/// <summary>Verifies that fetching a written question by a valid ID returns that specific question.</summary>
	[Fact]
	public async Task GetWrittenQuestionByIdAsync_WithValidId_ReturnsQuestion()
	{
		if (FirstValue(await GetQuestionsAsync(new() { Take = 1 })) is not { } first)
		{
			return;
		}

		var result = await Client.QuestionsStatements.GetWrittenQuestionByIdAsync(first.Id, CancellationToken);

		_ = result.Value.Id.Should().Be(first.Id);
		_ = result.Value.QuestionText.Should().NotBeNullOrEmpty();
	}

	/// <summary>Verifies that streaming all written questions via async enumerable completes without error.</summary>
	[Fact]
	public async Task GetAllWrittenQuestionsAsync_StreamsResults()
		=> _ = (await CollectStreamedItemsAsync(
			Client.GetAllAsync(new GetWrittenQuestionsRequest { House = "Commons", Take = 5 }, CancellationToken))).Should().NotBeNull();

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
