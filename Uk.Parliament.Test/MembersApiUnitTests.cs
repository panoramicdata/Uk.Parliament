using Uk.Parliament.Models.Members;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Members API (mocking)
/// </summary>
public class MembersApiUnitTests : IntegrationTestBase
{
	/// <summary>Verifies that <c>SearchAsync</c> returns the mocked paginated members list.</summary>
	[Fact]
	public async Task SearchAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IMembersApi>();
		var expectedMembers = new PaginatedResponse<Member>
		{
			TotalResults = 2,
			Skip = 0,
			Take = 2,
			Items =
			[
				new() { Value = new Member { Id = 1, NameDisplayAs = "Test Member 1" } },
				new() { Value = new Member { Id = 2, NameDisplayAs = "Test Member 2" } }
			]
		};

		_ = mockApi.Setup(x => x.SearchAsync(
			It.IsAny<SearchMembersRequest>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedMembers);

		// Act
		var result = await mockApi.Object.SearchAsync(new SearchMembersRequest { Take = 2 },
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Items.Should().HaveCount(2);
		_ = result.Items[0].Value.NameDisplayAs.Should().Be("Test Member 1");
	}

	/// <summary>Verifies that <see cref="IMembersApi.GetByIdAsync"/> returns the mocked member wrapper.</summary>
	[Fact]
	public async Task GetByIdAsync_WithMock_ReturnsMember()
	{
		// Arrange
		var mockApi = new Mock<IMembersApi>();
		var expectedMemberWrapper = new ValueWrapper<Member>
		{
			Value = new Member
			{
				Id = 123,
				NameDisplayAs = "Test Member",
				NameFullTitle = "Test Member MP"
			}
		};

		_ = mockApi.Setup(x => x.GetByIdAsync(123, It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedMemberWrapper);

		// Act
		var result = await mockApi.Object.GetByIdAsync(123,
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Value.Id.Should().Be(123);
		_ = result.Value.NameDisplayAs.Should().Be("Test Member");
	}

	/// <summary>Verifies that <c>SearchConstituenciesAsync</c> returns the mocked constituency list.</summary>
	[Fact]
	public async Task SearchConstituenciesAsync_WithMock_ReturnsConstituencies()
	{
		// Arrange
		var mockApi = new Mock<IMembersApi>();
		var expectedConstituencies = new PaginatedResponse<Constituency>
		{
			TotalResults = 1,
			Items =
			[
				new() { Value = new Constituency { Id = 1, Name = "Test Constituency" } }
			]
		};

		_ = mockApi.Setup(x => x.SearchConstituenciesAsync(
			It.IsAny<SearchConstituenciesRequest>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedConstituencies);

		// Act
		var result = await mockApi.Object.SearchConstituenciesAsync(new SearchConstituenciesRequest { Take = 1 },
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Items.Should().ContainSingle();
		_ = result.Items[0].Value.Name.Should().Be("Test Constituency");
	}
}
