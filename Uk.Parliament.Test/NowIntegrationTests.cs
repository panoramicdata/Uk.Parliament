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
	public async Task GetCommonsStatusAsync_ReturnsStatus()
	{
		// Act
		var result = await _client.Now.GetCommonsStatusAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.House.Should().Be("Commons");
	}

	[Fact]
	public async Task GetLordsStatusAsync_ReturnsStatus()
	{
		// Act
		var result = await _client.Now.GetLordsStatusAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.House.Should().Be("Lords");
	}

	[Fact]
	public async Task GetUpcomingBusinessAsync_ForCommons_ReturnsBusinessItems()
	{
		// Act
		var result = await _client.Now.GetUpcomingBusinessAsync("Commons");

		// Assert
		_ = result.Should().NotBeNull();
	}

	[Fact]
	public async Task GetCurrentBusinessAsync_ForCommons_ReturnsCurrentBusiness()
	{
		// Act
		var result = await _client.Now.GetCurrentBusinessAsync("Commons");

		// Assert - can be null if chamber not sitting
		// Just verify no exception thrown
	}

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
}
