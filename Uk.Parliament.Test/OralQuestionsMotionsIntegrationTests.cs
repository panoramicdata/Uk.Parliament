using System;
using Xunit;
using Uk.Parliament.Extensions;
using Uk.Parliament.Models.OralQuestions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Oral Questions and Motions API
/// </summary>
public class OralQuestionsMotionsIntegrationTests : IDisposable
{
	private readonly ParliamentClient _client;

	public OralQuestionsMotionsIntegrationTests()
	{
		_client = new ParliamentClient();
	}

	#region Oral Questions Tests

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetOralQuestionsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await _client.OralQuestionsMotions.GetOralQuestionsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.TotalResults.Should().BeGreaterThan(0);
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetOralQuestionsAsync_FilterByMember_ReturnsQuestions()
	{
		// Arrange
		const int memberId = 172;

		// Act
		var result = await _client.OralQuestionsMotions.GetOralQuestionsAsync(
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
	public async Task GetOralQuestionByIdAsync_WithValidId_ReturnsQuestion()
	{
		// Arrange
		const int questionId = 1;

		// Act
		var result = await _client.OralQuestionsMotions.GetOralQuestionByIdAsync(questionId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(questionId);
		_ = result.QuestionText.Should().NotBeNullOrEmpty();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetAllOralQuestionsAsync_StreamsResults()
	{
		// Arrange
		var questions = new List<OralQuestion>();

		// Act
		await foreach (var question in _client.OralQuestionsMotions.GetAllOralQuestionsAsync(
			house: "Commons",
			pageSize: 5))
		{
			questions.Add(question);
			if (questions.Count >= 10) break;
		}

		// Assert
		_ = questions.Should().NotBeEmpty();
		_ = questions.Count.Should().BeGreaterThanOrEqualTo(5);
	}

	#endregion

	#region Motions Tests

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetMotionsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await _client.OralQuestionsMotions.GetMotionsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.TotalResults.Should().BeGreaterThan(0);
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetMotionsAsync_FilterByActive_ReturnsActiveMotions()
	{
		// Act
		var result = await _client.OralQuestionsMotions.GetMotionsAsync(
			isActive: true,
			take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.IsActive.Should().BeTrue();
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetMotionByIdAsync_WithValidId_ReturnsMotion()
	{
		// Arrange
		const int motionId = 1;

		// Act
		var result = await _client.OralQuestionsMotions.GetMotionByIdAsync(motionId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(motionId);
		_ = result.MotionText.Should().NotBeNullOrEmpty();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetAllMotionsAsync_StreamsResults()
	{
		// Arrange
		var motions = new List<Motion>();

		// Act
		await foreach (var motion in _client.OralQuestionsMotions.GetAllMotionsAsync(
			house: "Commons",
			pageSize: 5))
		{
			motions.Add(motion);
			if (motions.Count >= 10) break;
		}

		// Assert
		_ = motions.Should().NotBeEmpty();
		_ = motions.Count.Should().BeGreaterThanOrEqualTo(5);
	}

	#endregion

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
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
		_ = result.Items.Should().HaveCount(1);
		_ = result.Items[0].Value.SignatureCount.Should().Be(50);
	}
}
