using Uk.Parliament.Models.Questions;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Questions &amp; Statements API (mocking)
/// </summary>
public class QuestionsStatementsApiUnitTests : IntegrationTestBase
{
	/// <summary>Verifies that <see cref="IQuestionsStatementsApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void QuestionsStatementsApi_CanBeMocked()
		=> new Mock<IQuestionsStatementsApi>().Object.Should().NotBeNull();

	/// <summary>Verifies that a mocked <c>GetWrittenQuestionsAsync</c> returns the expected written questions data.</summary>
	[Fact]
	public async Task GetWrittenQuestionsAsync_WithMock_ReturnsExpectedData()
	{
		var mockApi = new Mock<IQuestionsStatementsApi>();
		_ = mockApi.Setup(x => x.GetWrittenQuestionsAsync(It.IsAny<GetWrittenQuestionsRequest>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Page(
				new WrittenQuestion { Id = 1, Uin = "12345", House = "Commons", QuestionText = "Test question 1" },
				new WrittenQuestion { Id = 2, Uin = "12346", House = "Commons", QuestionText = "Test question 2" }));

		var result = await mockApi.Object.GetWrittenQuestionsAsync(new GetWrittenQuestionsRequest { Take = 10 }, CancellationToken);

		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}

	/// <summary>Verifies that a mocked <c>GetWrittenStatementsAsync</c> returns the expected statements data.</summary>
	[Fact]
	public async Task GetWrittenStatementsAsync_WithMock_ReturnsExpectedData()
	{
		var mockApi = new Mock<IQuestionsStatementsApi>();
		_ = mockApi.Setup(x => x.GetWrittenStatementsAsync(It.IsAny<GetWrittenStatementsRequest>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Page(
				new WrittenStatement { Id = 1, Uin = "HCWS123", House = "Commons", Title = "Test Statement 1" },
				new WrittenStatement { Id = 2, Uin = "HCWS124", House = "Commons", Title = "Test Statement 2" }));

		var result = await mockApi.Object.GetWrittenStatementsAsync(new GetWrittenStatementsRequest { Take = 10 }, CancellationToken);

		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}

	/// <summary>Verifies that a mocked <c>GetDailyReportsAsync</c> returns the expected daily report data.</summary>
	[Fact]
	public async Task GetDailyReportsAsync_WithMock_ReturnsExpectedData()
	{
		var mockApi = new Mock<IQuestionsStatementsApi>();
		_ = mockApi.Setup(x => x.GetDailyReportsAsync(It.IsAny<GetDailyReportsRequest>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Page(new DailyReport { Id = 1, House = "Commons", Title = "Daily Report", QuestionCount = 50, StatementCount = 10, IsPublished = true }));

		var result = await mockApi.Object.GetDailyReportsAsync(new GetDailyReportsRequest(), CancellationToken);

		_ = result.Items.Should().ContainSingle();
		_ = result.Items[0].Value.QuestionCount.Should().Be(50);
		_ = result.Items[0].Value.StatementCount.Should().Be(10);
	}
}
