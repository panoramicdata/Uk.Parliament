namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for NOW (Annunciator) API
/// </summary>
public class NowIntegrationTests : IDisposable
{
	private readonly ParliamentClient _client;

	public NowIntegrationTests()
	{
		_client = new ParliamentClient();
	}

	[Fact]
	public async Task GetCurrentMessageAsync_ForCommons_ReturnsMessage()
	{
		// Act
		var result = await _client.Now.GetCurrentMessageAsync("commons");

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().BePositive();
		_ = result.Slides.Should().NotBeNull();
	}

	[Fact]
	public async Task GetCurrentMessageAsync_ForLords_ReturnsMessage()
	{
		// Act
		var result = await _client.Now.GetCurrentMessageAsync("lords");

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
		var result = await _client.Now.GetMessageByDateAsync("commons", today);

		// Assert
		_ = result.Should().NotBeNull();
	}

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
}
