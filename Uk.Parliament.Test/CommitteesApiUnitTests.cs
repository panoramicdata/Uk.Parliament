namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Committees API (mocking)
/// </summary>
public class CommitteesApiUnitTests
{
	/// <summary>Verifies that <see cref="ICommitteesApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void CommitteesApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ICommitteesApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	/// <summary>Verifies that the <see cref="ICommitteesApi"/> interface is defined and accessible.</summary>
	[Fact]
	public void CommitteesApi_InterfaceExists()
	{
		// This test ensures the interface is defined
		// Real tests will be added once models are implemented
		var interfaceType = typeof(ICommitteesApi);
		_ = interfaceType.IsInterface.Should().BeTrue();
		_ = interfaceType.Name.Should().Be("ICommitteesApi");
	}

	// TODO: Add actual tests once Committee models are implemented
	// Expected tests:
	// - GetCommitteesAsync_WithMock_ReturnsExpectedData
	// - GetCommitteeByIdAsync_WithMock_ReturnsCommittee
	// - GetCommitteeMembersAsync_WithMock_ReturnsMembers
	// - GetCommitteeInquiriesAsync_WithMock_ReturnsInquiries
	// - GetInquiryByIdAsync_WithMock_ReturnsInquiry
	// - GetInquiryContributionsAsync_WithMock_ReturnsContributions
	// - GetInquirySubmissionsAsync_WithMock_ReturnsSubmissions
	// - GetInquiryPublicationsAsync_WithMock_ReturnsPublications
}
