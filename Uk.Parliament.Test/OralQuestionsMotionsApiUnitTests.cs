using Uk.Parliament.Models.OralQuestions;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Oral Questions & Motions API (mocking)
/// </summary>
public class OralQuestionsMotionsApiUnitTests : IntegrationTestBase
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
		var expectedResponse = CreateOralQuestionsResponse();
		SetupGetOralQuestionsAsyncMock(mockApi, expectedResponse);

		// Act
		var result = await mockApi.Object.GetOralQuestionsAsync(take: 10,
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.PagingInfo.Total.Should().Be(2);
		_ = result.Response.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetMotionsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IOralQuestionsMotionsApi>();
		var expectedResponse = CreateMotionsResponse();
		SetupGetMotionsAsyncMock(mockApi, expectedResponse);

		// Act
		var result = await mockApi.Object.GetMotionsAsync(isActive: true,
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.PagingInfo.Total.Should().Be(1);
		_ = result.Response.Should().ContainSingle();
		_ = result.Response[0].SignatureCount.Should().Be(50);
	}

	private static OralQuestionsResponse<OralQuestion> CreateOralQuestionsResponse() => new()
	{
		PagingInfo = new OralQuestionsPagingInfo { Total = 2, Skip = 0, Take = 10 },
		StatusCode = 200,
		Success = true,
		Response =
		[
			new OralQuestion
			{
				Id = 1,
				Uin = 123,
				QuestionText = "Test oral question 1",
				TabledWhen = DateTime.Now.AddDays(-1)
			},
			new OralQuestion
			{
				Id = 2,
				Uin = 124,
				QuestionText = "Test oral question 2",
				TabledWhen = DateTime.Now
			}
		]
	};

	private static OralQuestionsResponse<Motion> CreateMotionsResponse() => new()
	{
		PagingInfo = new OralQuestionsPagingInfo { Total = 1, Skip = 0, Take = 10 },
		StatusCode = 200,
		Success = true,
		Response =
		[
			new Motion
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
		]
	};

	// Note: This mock setup has apparent complexity due to the API's parameter count.
	private static void SetupGetOralQuestionsAsyncMock(
		Mock<IOralQuestionsMotionsApi> mockApi,
		OralQuestionsResponse<OralQuestion> response)
		=> mockApi
			.Setup(x => x.GetOralQuestionsAsync(
				It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(),
				It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<bool?>(),
				It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(response);

	// Note: This mock setup has apparent complexity due to the API's parameter count.
	private static void SetupGetMotionsAsyncMock(
		Mock<IOralQuestionsMotionsApi> mockApi,
		OralQuestionsResponse<Motion> response)
		=> mockApi
			.Setup(x => x.GetMotionsAsync(
				It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
				It.IsAny<string?>(), It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<int?>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(response);

	[Fact]
	public async Task GetMotionByIdAsync_WithMock_ReturnsMotion()
	{
		// Arrange
		var mockApi = new Mock<IOralQuestionsMotionsApi>();
		var expectedResponse = new OralQuestionsResponse<Motion>
		{
			PagingInfo = new OralQuestionsPagingInfo { Total = 1, Skip = 0, Take = 1 },
			StatusCode = 200,
			Success = true,
			Response =
			[
				new Motion
				{
					Id = 123,
					Reference = "EDM123",
					House = "Commons",
					Title = "Test Motion",
					MotionText = "This is a test motion",
					DateTabled = DateTime.Now.AddDays(-5),
					SignatureCount = 50,
					IsActive = true
				}
			]
		};

		_ = mockApi.Setup(x => x.GetMotionByIdAsync(123, It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetMotionByIdAsync(123,
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Success.Should().BeTrue();
		_ = result.Response.Should().ContainSingle();
		_ = result.Response[0].Id.Should().Be(123);
	}
}
