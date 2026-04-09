namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for NOW (Annunciator) API
/// </summary>
public class NowIntegrationTests : IntegrationTestBase
{
	/// <summary>Verifies that fetching the current Commons annunciator message returns a valid response.</summary>
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

	/// <summary>Verifies that fetching the current Lords annunciator message returns a valid response.</summary>
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

	/// <summary>Verifies that fetching the Commons annunciator message for today's date returns a valid response.</summary>
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
