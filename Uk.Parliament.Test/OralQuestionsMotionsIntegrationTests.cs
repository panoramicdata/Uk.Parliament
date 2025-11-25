using Uk.Parliament.Extensions;
using Uk.Parliament.Models.OralQuestions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Oral Questions and Motions API
/// </summary>
public class OralQuestionsMotionsIntegrationTests : IntegrationTestBase
{

	#region Oral Questions Tests

	[Fact]
	public async Task GetOralQuestionsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await Client.OralQuestionsMotions.GetOralQuestionsAsync(take: 10);

		// Assert
		AssertValidPaginatedResponse(result);
	}

	[Fact]
	public async Task GetOralQuestionsAsync_FilterByMember_ReturnsQuestions()
	{
		// Arrange
		const int memberId = 172;

		// Act
		var result = await Client.OralQuestionsMotions.GetOralQuestionsAsync(
			askingMemberId: memberId,
			take: 10);

		// Assert
		AssertValidPaginatedResponse(result, item => _ = item.Value.AskingMemberId.Should().Be(memberId));
	}

	[Fact]
	public async Task GetOralQuestionByIdAsync_WithValidId_ReturnsQuestion()
	{
		// Arrange
		const int questionId = 1;

		// Act
		var result = await Client.OralQuestionsMotions.GetOralQuestionByIdAsync(questionId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(questionId);
		_ = result.QuestionText.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllOralQuestionsAsync_StreamsResults()
	{
		// Act
		var questions = await CollectStreamedItemsAsync(
			Client.OralQuestionsMotions.GetAllOralQuestionsAsync(
				house: "Commons",
				pageSize: 5));

		// Assert
		AssertValidStreamedResults(questions);
	}

	#endregion

	#region Motions Tests

	[Fact]
	public async Task GetMotionsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await Client.OralQuestionsMotions.GetMotionsAsync(take: 10);

		// Assert
		AssertValidPaginatedResponse(result);
	}

	[Fact]
	public async Task GetMotionsAsync_FilterByActive_ReturnsActiveMotions()
	{
		// Act
		var result = await Client.OralQuestionsMotions.GetMotionsAsync(
			isActive: true,
			take: 10);

		// Assert
		AssertValidPaginatedResponse(result, static item => _ = item.Value.IsActive.Should().BeTrue());
	}

	[Fact]
	public async Task GetMotionByIdAsync_WithValidId_ReturnsMotion()
	{
		// Arrange
		const int motionId = 1;

		// Act
		var result = await Client.OralQuestionsMotions.GetMotionByIdAsync(motionId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(motionId);
		_ = result.MotionText.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllMotionsAsync_StreamsResults()
	{
		// Act
		var motions = await CollectStreamedItemsAsync(
			Client.OralQuestionsMotions.GetAllMotionsAsync(
				house: "Commons",
				pageSize: 5));

		// Assert
		AssertValidStreamedResults(motions);
	}

	#endregion
}

/// <summary>
/// Unit tests for Oral Questions & Motions API (mocking)
/// </summary>
public class OralQuestionsMotionsApiUnitTests
{
	[Fact]
	public void OralQuestionsMotionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<IOralQuestionsMotionsApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public async Task GetOralQuestionsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IOralQuestionsMotionsApi>();
		var expectedResponse = new PaginatedResponse<OralQuestion>
		{
			TotalResults = 2,
			Items =
			[
				new ValueWrapper<OralQuestion>
				{
					Value = new OralQuestion
					{
						Id = 1,
						Uin = "OQ123",
						House = "Commons",
						QuestionText = "Test oral question 1",
						DateAsked = DateTime.Now.AddDays(-1)
					}
				},
				new ValueWrapper<OralQuestion>
				{
					Value = new OralQuestion
					{
						Id = 2,
						Uin = "OQ124",
						House = "Commons",
						QuestionText = "Test oral question 2",
						DateAsked = DateTime.Now
					}
				}
			]
		};

		_ = mockApi.Setup(x => x.GetOralQuestionsAsync(
			It.IsAny<int?>(),
			It.IsAny<string?>(),
			It.IsAny<string?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<bool?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetOralQuestionsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetMotionsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IOralQuestionsMotionsApi>();
		var expectedResponse = new PaginatedResponse<Motion>
		{
			TotalResults = 1,
			Items =
			[
				new ValueWrapper<Motion>
				{
					Value = new Motion
					{
						Id = 1,
						Reference = "EDM123",
						House = "Commons",
						Title = "Test Motion",
						MotionText = "This is a test motion",
						DateTabled = DateTime.Now.AddDays(-5),
						SignatureCount = 50,
						IsActive = true
					}
				}
			]
		};

		_ = mockApi.Setup(x => x.GetMotionsAsync(
			It.IsAny<int?>(),
			It.IsAny<string?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<string?>(),
			It.IsAny<bool?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetMotionsAsync(isActive: true);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(1);
		_ = result.Items.Should().ContainSingle();
		_ = result.Items[0].Value.SignatureCount.Should().Be(50);
	}
}
