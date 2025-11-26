using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Questions & Statements API (mocking)
/// </summary>
public class QuestionsStatementsApiUnitTests : IntegrationTestBase
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
		var result = await mockApi.Object.GetWrittenQuestionsAsync(take: 10,
			cancellationToken: CancellationToken);

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
		var result = await mockApi.Object.GetWrittenStatementsAsync(take: 10,
			cancellationToken: CancellationToken);

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
		var result = await mockApi.Object.GetDailyReportsAsync(
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(1);
		_ = result.Items.Should().ContainSingle();
		_ = result.Items[0].Value.QuestionCount.Should().Be(50);
		_ = result.Items[0].Value.StatementCount.Should().Be(10);
	}
}
