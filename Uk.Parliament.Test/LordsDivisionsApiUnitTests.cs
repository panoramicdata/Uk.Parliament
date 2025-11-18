namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Lords Divisions API (mocking)
/// </summary>
public class LordsDivisionsApiUnitTests
{
	[Fact]
	public void LordsDivisionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ILordsDivisionsApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public void LordsDivisionsApi_InterfaceExists()
	{
		// This test ensures the interface is defined
		// Real tests will be added once models are implemented
		var interfaceType = typeof(ILordsDivisionsApi);
		_ = interfaceType.IsInterface.Should().BeTrue();
		_ = interfaceType.Name.Should().Be("ILordsDivisionsApi");
	}

	// TODO: Add actual tests once Division models are implemented
	// Expected tests:
	// - GetDivisionsAsync_WithMock_ReturnsExpectedData
	// - GetDivisionByIdAsync_WithMock_ReturnsDivision
	// - GetDivisionGroupedByPartyAsync_WithMock_ReturnsGroupedVotes
	// - SearchDivisionsAsync_WithMock_ReturnsResults
}
