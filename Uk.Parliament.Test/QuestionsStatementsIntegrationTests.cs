using Uk.Parliament.Extensions;
using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Written Questions and Statements API
/// </summary>
public class QuestionsStatementsIntegrationTests : IDisposable
{
	private readonly ParliamentClient _client;

	public QuestionsStatementsIntegrationTests()
	{
		_client = new ParliamentClient();
	}

	#region Written Questions Tests

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetWrittenQuestionsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await _client.QuestionsStatements.GetWrittenQuestionsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.TotalResults.Should().BePositive();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetWrittenQuestionsAsync_FilterByMember_ReturnsQuestions()
	{
		// Arrange
		const int memberId = 172; // Example member ID

		// Act
		var result = await _client.QuestionsStatements.GetWrittenQuestionsAsync(
			askingMemberId: memberId,
			take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.AskingMemberId.Should().Be(memberId);
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetWrittenQuestionsAsync_FilterByDateRange_ReturnsQuestions()
	{
		// Arrange
		var fromDate = DateTime.Now.AddMonths(-1);
		var toDate = DateTime.Now;

		// Act
		var result = await _client.QuestionsStatements.GetWrittenQuestionsAsync(
			tabledWhenFrom: fromDate,
			tabledWhenTo: toDate,
			take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.DateTabled.Should().BeOnOrAfter(fromDate);
			_ = item.Value.DateTabled.Should().BeOnOrBefore(toDate);
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetWrittenQuestionsAsync_FilterByAnswered_ReturnsAnsweredQuestions()
	{
		// Act
		var result = await _client.QuestionsStatements.GetWrittenQuestionsAsync(
			isAnswered: true,
			take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.IsAnswered.Should().BeTrue();
			_ = item.Value.AnswerText.Should().NotBeNullOrEmpty();
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetWrittenQuestionByIdAsync_WithValidId_ReturnsQuestion()
	{
		// Arrange
		const int questionId = 1; // Example question ID

		// Act
		var result = await _client.QuestionsStatements.GetWrittenQuestionByIdAsync(questionId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(questionId);
		_ = result.QuestionText.Should().NotBeNullOrEmpty();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetAllWrittenQuestionsAsync_StreamsResults()
	{
		// Arrange
		var questions = new List<WrittenQuestion>();

		// Act
		await foreach (var question in _client.QuestionsStatements.GetAllWrittenQuestionsAsync(
			house: "Commons",
			pageSize: 5))
		{
			questions.Add(question);
			if (questions.Count >= 10)
			{
				break; // Limit for test
			}
		}

		// Assert
		_ = questions.Should().NotBeEmpty();
		_ = questions.Should().HaveCountGreaterThanOrEqualTo(5);
	}

	#endregion

	#region Written Statements Tests

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetWrittenStatementsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await _client.QuestionsStatements.GetWrittenStatementsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.TotalResults.Should().BePositive();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetWrittenStatementsAsync_FilterByDateRange_ReturnsStatements()
	{
		// Arrange
		var fromDate = DateTime.Now.AddMonths(-1);
		var toDate = DateTime.Now;

		// Act
		var result = await _client.QuestionsStatements.GetWrittenStatementsAsync(
			madeWhenFrom: fromDate,
			madeWhenTo: toDate,
			take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.DateMade.Should().BeOnOrAfter(fromDate);
			_ = item.Value.DateMade.Should().BeOnOrBefore(toDate);
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetWrittenStatementByIdAsync_WithValidId_ReturnsStatement()
	{
		// Arrange
		const int statementId = 1; // Example statement ID

		// Act
		var result = await _client.QuestionsStatements.GetWrittenStatementByIdAsync(statementId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(statementId);
		_ = result.StatementText.Should().NotBeNullOrEmpty();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetAllWrittenStatementsAsync_StreamsResults()
	{
		// Arrange
		var statements = new List<WrittenStatement>();

		// Act
		await foreach (var statement in _client.QuestionsStatements.GetAllWrittenStatementsAsync(
			house: "Commons",
			pageSize: 5))
		{
			statements.Add(statement);
			if (statements.Count >= 10)
			{
				break; // Limit for test
			}
		}

		// Assert
		_ = statements.Should().NotBeEmpty();
		_ = statements.Should().HaveCountGreaterThanOrEqualTo(5);
	}

	#endregion

	#region Daily Reports Tests

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetDailyReportsAsync_WithDateRange_ReturnsReports()
	{
		// Arrange
		var fromDate = DateTime.Now.AddDays(-7);
		var toDate = DateTime.Now;

		// Act
		var result = await _client.QuestionsStatements.GetDailyReportsAsync(
			dateFrom: fromDate,
			dateTo: toDate,
			take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetAllDailyReportsAsync_StreamsResults()
	{
		// Arrange
		var reports = new List<DailyReport>();
		var fromDate = DateTime.Now.AddDays(-30);

		// Act
		await foreach (var report in _client.QuestionsStatements.GetAllDailyReportsAsync(
			dateFrom: fromDate,
			pageSize: 5))
		{
			reports.Add(report);
			if (reports.Count >= 10)
			{
				break; // Limit for test
			}
		}

		// Assert
		_ = reports.Should().NotBeEmpty();
	}

	#endregion

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
}

/// <summary>
/// Unit tests for Questions & Statements API (mocking)
/// </summary>
public class QuestionsStatementsApiUnitTests
{
	[Fact]
	public void QuestionsStatementsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<IQuestionsStatementsApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public async Task GetWrittenQuestionsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IQuestionsStatementsApi>();
		var expectedResponse = new PaginatedResponse<WrittenQuestion>
		{
			TotalResults = 2,
			Items =
			[
				new ValueWrapper<WrittenQuestion>
				{
					Value = new WrittenQuestion
					{
						Id = 1,
						Uin = "12345",
						House = "Commons",
						QuestionText = "Test question 1",
						DateTabled = DateTime.Now.AddDays(-1)
					}
				},
				new ValueWrapper<WrittenQuestion>
				{
					Value = new WrittenQuestion
					{
						Id = 2,
						Uin = "12346",
						House = "Commons",
						QuestionText = "Test question 2",
						DateTabled = DateTime.Now
					}
				}
			]
		};

		_ = mockApi.Setup(x => x.GetWrittenQuestionsAsync(
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<string?>(),
			It.IsAny<string?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<bool?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetWrittenQuestionsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetWrittenStatementsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IQuestionsStatementsApi>();
		var expectedResponse = new PaginatedResponse<WrittenStatement>
		{
			TotalResults = 2,
			Items =
			[
				new ValueWrapper<WrittenStatement>
				{
					Value = new WrittenStatement
					{
						Id = 1,
						Uin = "HCWS123",
						House = "Commons",
						Title = "Test Statement 1",
						StatementText = "This is a test statement",
						DateMade = DateTime.Now.AddDays(-1)
					}
				},
				new ValueWrapper<WrittenStatement>
				{
					Value = new WrittenStatement
					{
						Id = 2,
						Uin = "HCWS124",
						House = "Commons",
						Title = "Test Statement 2",
						StatementText = "This is another test statement",
						DateMade = DateTime.Now
					}
				}
			]
		};

		_ = mockApi.Setup(x => x.GetWrittenStatementsAsync(
			It.IsAny<int?>(),
			It.IsAny<string?>(),
			It.IsAny<string?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetWrittenStatementsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetDailyReportsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IQuestionsStatementsApi>();
		var expectedResponse = new PaginatedResponse<DailyReport>
		{
			TotalResults = 1,
			Items =
			[
				new ValueWrapper<DailyReport>
				{
					Value = new DailyReport
					{
						Id = 1,
						Date = DateTime.Now.AddDays(-1),
						House = "Commons",
						Title = "Daily Report",
						QuestionCount = 50,
						StatementCount = 10,
						IsPublished = true
					}
				}
			]
		};

		_ = mockApi.Setup(x => x.GetDailyReportsAsync(
			It.IsAny<DateTime?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<string?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetDailyReportsAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(1);
		_ = result.Items.Should().ContainSingle();
		_ = result.Items[0].Value.QuestionCount.Should().Be(50);
		_ = result.Items[0].Value.StatementCount.Should().Be(10);
	}
}
