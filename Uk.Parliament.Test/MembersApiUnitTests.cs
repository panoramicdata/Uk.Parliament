using Uk.Parliament.Extensions;
using Uk.Parliament.Models.Members;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Members API (mocking)
/// </summary>
public class MembersApiUnitTests : IntegrationTestBase
{
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
			It.IsAny<string>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<bool?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedMembers);

		// Act
		var result = await mockApi.Object.SearchAsync(take: 2,
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Items.Should().HaveCount(2);
		_ = result.Items[0].Value.NameDisplayAs.Should().Be("Test Member 1");
	}

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
			It.IsAny<string>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedConstituencies);

		// Act
		var result = await mockApi.Object.SearchConstituenciesAsync(take: 1,
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Items.Should().ContainSingle();
		_ = result.Items[0].Value.Name.Should().Be("Test Constituency");
	}

	[Fact]
	public async Task GetAllAsync_WithMock_PaginatesCorrectly()
	{
		// Arrange
		var mockApi = new Mock<IMembersApi>();

		// Page 1
		_ = mockApi.Setup(x => x.SearchAsync(null, 0, 2, null, null, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new PaginatedResponse<Member>
			{
				TotalResults = 3,
				Skip = 0,
				Take = 2,
				Items =
				[
					new() { Value = new Member { Id = 1 } },
					new() { Value = new Member { Id = 2 } }
				]
			});

		// Page 2
		_ = mockApi.Setup(x => x.SearchAsync(null, 2, 2, null, null, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new PaginatedResponse<Member>
			{
				TotalResults = 3,
				Skip = 2,
				Take = 2,
				Items =
				[
					new() { Value = new Member { Id = 3 } }
				]
			});

		var allMembers = new List<Member>();

		// Act
		await foreach (var member in mockApi.Object.GetAllAsync(pageSize: 2,
			cancellationToken: CancellationToken))
		{
			allMembers.Add(member);
		}

		// Assert
		_ = allMembers.Should().HaveCount(3);
		_ = allMembers[0].Id.Should().Be(1);
		_ = allMembers[2].Id.Should().Be(3);
	}
}
