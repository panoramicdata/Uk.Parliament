using Uk.Parliament.Extensions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Written Questions and Statements API
/// </summary>
public class QuestionsStatementsIntegrationTests : IntegrationTestBase
{
	#region Written Questions Tests

	[Fact]
	public async Task GetWrittenQuestionsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await Client
			.QuestionsStatements
			.GetWrittenQuestionsAsync(
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		AssertValidPaginatedResponse(result);
	}

	[Fact]
	public async Task GetWrittenQuestionsAsync_FilterByMember_ReturnsQuestions()
	{
		// Arrange
		const int memberId = 172; // Example member ID

		// Act
		var result = await Client
			.QuestionsStatements
			.GetWrittenQuestionsAsync(
				askingMemberId: memberId,
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		AssertValidPaginatedResponse(result, item => _ = item.Value.AskingMemberId.Should().Be(memberId));
	}

	[Fact]
	public async Task GetWrittenQuestionsAsync_FilterByDateRange_ReturnsQuestions()
	{
		// Arrange
		var fromDate = DateTime.Now.AddMonths(-1);
		var toDate = DateTime.Now;

		// Act
		var result = await Client
			.QuestionsStatements
			.GetWrittenQuestionsAsync(
				tabledWhenFrom: fromDate,
				tabledWhenTo: toDate,
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		AssertValidPaginatedResponse(result, item =>
		{
			_ = item.Value.DateTabled.Should().BeOnOrAfter(fromDate);
			_ = item.Value.DateTabled.Should().BeOnOrBefore(toDate);
		});
	}

	[Fact]
	public async Task GetWrittenQuestionsAsync_FilterByAnswered_ReturnsAnsweredQuestions()
	{
		// Act
		var result = await Client
			.QuestionsStatements
			.GetWrittenQuestionsAsync(
				isAnswered: true,
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		AssertValidPaginatedResponse(result, item =>
		{
			_ = item.Value.IsAnswered.Should().BeTrue();
			_ = item.Value.AnswerText.Should().NotBeNullOrEmpty();
		});
	}

	[Fact]
	public async Task GetWrittenQuestionByIdAsync_WithValidId_ReturnsQuestion()
	{
		// Arrange - First get a valid question ID from the list
		var listResult = await Client
			.QuestionsStatements
			.GetWrittenQuestionsAsync(
				take: 1,
				cancellationToken: CancellationToken);

		// Check if using Results or Items property
		var hasResults = listResult.Results != null && listResult.Results.Count > 0;
		var hasItems = listResult.Items != null && listResult.Items.Count > 0;

		if (!hasResults && !hasItems)
		{
			// Skip test if no questions available
			return;
		}

		var validQuestionId = hasResults
			? listResult.Results![0].Value.Id
			: listResult.Items[0].Value.Id;

		// Act
		var result = await Client
			.QuestionsStatements
			.GetWrittenQuestionByIdAsync(
				validQuestionId,
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Value.Should().NotBeNull();
		_ = result.Value.Id.Should().Be(validQuestionId);
		_ = result.Value.QuestionText.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllWrittenQuestionsAsync_StreamsResults()
	{
		// Act
		var questions = await CollectStreamedItemsAsync(
			Client.QuestionsStatements.GetAllWrittenQuestionsAsync(
				house: "Commons",
				pageSize: 5,
				cancellationToken: CancellationToken));

		// Assert
		AssertValidStreamedResults(questions);
	}

	#endregion

	#region Written Statements Tests

	[Fact]
	public async Task GetWrittenStatementsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await Client
			.QuestionsStatements
			.GetWrittenStatementsAsync(
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		AssertValidPaginatedResponse(result);
	}

	[Fact]
	public async Task GetWrittenStatementsAsync_FilterByDateRange_ReturnsStatements()
	{
		// Arrange
		var fromDate = DateTime.Now.AddMonths(-1);
		var toDate = DateTime.Now;

		// Act
		var result = await Client
			.QuestionsStatements
			.GetWrittenStatementsAsync(
				madeWhenFrom: fromDate,
				madeWhenTo: toDate,
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		AssertValidPaginatedResponse(result, item =>
		{
			_ = item.Value.DateMade.Should().BeOnOrAfter(fromDate);
			_ = item.Value.DateMade.Should().BeOnOrBefore(toDate);
		});
	}

	[Fact]
	public async Task GetWrittenStatementByIdAsync_WithValidId_ReturnsStatement()
	{
		// Arrange - First get a valid statement ID from the list
		var listResult = await Client
			.QuestionsStatements
			.GetWrittenStatementsAsync(
				take: 1,
				cancellationToken: CancellationToken);

		// Check if using Results or Items property
		var hasResults = listResult.Results != null && listResult.Results.Count > 0;
		var hasItems = listResult.Items != null && listResult.Items.Count > 0;

		if (!hasResults && !hasItems)
		{
			// Skip test if no statements available
			return;
		}

		var validStatementId = hasResults
			? listResult.Results![0].Value.Id
			: listResult.Items[0].Value.Id;

		// Act
		var result = await Client
			.QuestionsStatements
			.GetWrittenStatementByIdAsync(
				validStatementId,
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Value.Should().NotBeNull();
		_ = result.Value.Id.Should().Be(validStatementId);
		_ = result.Value.StatementText.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllWrittenStatementsAsync_StreamsResults()
	{
		// Act
		var statements = await CollectStreamedItemsAsync(
			Client.QuestionsStatements.GetAllWrittenStatementsAsync(
				house: "Commons",
				pageSize: 5,
				cancellationToken: CancellationToken));

		// Assert
		AssertValidStreamedResults(statements);
	}

	#endregion

	#region Daily Reports Tests

	[Fact]
	public async Task GetDailyReportsAsync_WithDateRange_ReturnsReports()
	{
		// Arrange
		var fromDate = DateTime.Now.AddDays(-7);
		var toDate = DateTime.Now;

		// Act
		var result = await Client
			.QuestionsStatements
			.GetDailyReportsAsync(
				dateFrom: fromDate,
				dateTo: toDate,
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllDailyReportsAsync_StreamsResults()
	{
		// Arrange
		var fromDate = DateTime.Now.AddDays(-30);

		// Act
		var reports = await CollectStreamedItemsAsync(
			Client.QuestionsStatements.GetAllDailyReportsAsync(
				dateFrom: fromDate,
				pageSize: 5,
				cancellationToken: CancellationToken));

		// Assert
		_ = reports.Should().NotBeEmpty();
	}

	#endregion
}
