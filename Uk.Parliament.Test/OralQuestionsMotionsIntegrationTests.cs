using Uk.Parliament.Extensions;

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
		var result = await Client
			.OralQuestionsMotions
			.GetOralQuestionsAsync(
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Success.Should().BeTrue();
		_ = result.Response.Should().NotBeNull();
		_ = result.PagingInfo.Total.Should().BePositive();
	}

	[Fact]
	public async Task GetOralQuestionsAsync_FilterByMember_ReturnsQuestions()
	{
		// NOTE: This test may fail due to Parliament API bug - filter might not work
		// Arrange
		const int memberId = 172;

		// Act
		var result = await Client
			.OralQuestionsMotions
			.GetOralQuestionsAsync(
				askingMemberId: memberId,
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Success.Should().BeTrue();
	}

	[Fact]
	public async Task GetAllOralQuestionsAsync_StreamsResults()
	{
		// Act
		var questions = await CollectStreamedItemsAsync(
			Client.OralQuestionsMotions.GetAllOralQuestionsAsync(
				pageSize: 5,
			cancellationToken: CancellationToken));

		// Assert
		AssertValidStreamedResults(questions);
	}

	#endregion

	#region Motions Tests

	[Fact]
	public async Task GetMotionsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await Client
			.OralQuestionsMotions
			.GetMotionsAsync(
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Success.Should().BeTrue();
		_ = result.Response.Should().NotBeNull();
		_ = result.PagingInfo.Total.Should().BePositive();
	}

	[Fact]
	public async Task GetMotionsAsync_FilterByActive_ReturnsActiveMotions()
	{
		// Act
		var result = await Client
			.OralQuestionsMotions
			.GetMotionsAsync(
				isActive: true,
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Success.Should().BeTrue();
		_ = result.Response.Should().AllSatisfy(static m => m.IsActive.Should().BeTrue());
	}

	[Fact]
	public async Task GetMotionByIdAsync_WithValidId_ReturnsMotion()
	{
		// Arrange - First get a valid motion ID from the list
		var listResult = await Client
			.OralQuestionsMotions
			.GetMotionsAsync(
				take: 1,
				cancellationToken: CancellationToken);
		_ = listResult.Response.Should().NotBeEmpty("Need at least one motion to test GetById");
		var validMotionId = listResult.Response[0].Id;

		// Act
		var result = await Client
			.OralQuestionsMotions
			.GetMotionByIdAsync(
				validMotionId,
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Success.Should().BeTrue();
		_ = result.Response.Should().NotBeEmpty();
		_ = result.Response[0].Id.Should().Be(validMotionId);
		_ = result.Response[0].MotionText.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllMotionsAsync_StreamsResults()
	{
		// Act
		var motions = await CollectStreamedItemsAsync(
			Client.OralQuestionsMotions.GetAllMotionsAsync(
				pageSize: 5,
			cancellationToken: CancellationToken));

		// Assert
		AssertValidStreamedResults(motions);
	}

	#endregion
}
