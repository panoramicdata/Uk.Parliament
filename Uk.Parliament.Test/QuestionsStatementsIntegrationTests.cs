using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Written Questions and Statements API
/// </summary>
[Trait("Category", "Integration")]
public partial class QuestionsStatementsIntegrationTests : IntegrationTestBase
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
}
