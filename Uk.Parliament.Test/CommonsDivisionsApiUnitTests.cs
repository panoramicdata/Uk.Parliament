namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Commons Divisions API (mocking)
/// </summary>
public class CommonsDivisionsApiUnitTests
{
	[Fact]
	public void CommonsDivisionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ICommonsDivisionsApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public void CommonsDivisionsApi_InterfaceExists()
	{
		// This test ensures the interface is defined
		// Real tests will be added once models are implemented
		var interfaceType = typeof(ICommonsDivisionsApi);
		_ = interfaceType.IsInterface.Should().BeTrue();
		_ = interfaceType.Name.Should().Be("ICommonsDivisionsApi");
	}

	// TODO: Add actual tests once Division models are implemented
	// Expected tests:
	// - GetDivisionsAsync_WithMock_ReturnsExpectedData
	// - GetDivisionByIdAsync_WithMock_ReturnsDivision
	// - GetDivisionGroupedByPartyAsync_WithMock_ReturnsGroupedVotes
	// - SearchDivisionsAsync_WithMock_ReturnsResults
	// - GetMemberVotingAsync_WithMock_ReturnsMemberVotingHistory
}
