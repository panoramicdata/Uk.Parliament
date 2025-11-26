namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for NOW (Annunciator) API
/// </summary>
public class NowIntegrationTests : IntegrationTestBase
{
	[Fact]
	public async Task GetCurrentMessageAsync_ForCommons_ReturnsMessage()
	{
		// Act
		var result = await Client
			.Now
			.GetCurrentMessageAsync(
				"commons",
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().BePositive();
		_ = result.Slides.Should().NotBeNull();
	}

	[Fact]
	public async Task GetCurrentMessageAsync_ForLords_ReturnsMessage()
	{
		// Act
		var result = await Client
			.Now
			.GetCurrentMessageAsync(
				"lords",
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().BePositive();
		_ = result.Slides.Should().NotBeNull();
	}

	[Fact]
	public async Task GetMessageByDateAsync_ForCommonsToday_ReturnsMessage()
	{
		// Arrange
		var today = DateTime.Now.ToString("yyyy-MM-dd");

		// Act
		var result = await Client
			.Now
			.GetMessageByDateAsync(
				"commons",
				today,
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
	}
}
