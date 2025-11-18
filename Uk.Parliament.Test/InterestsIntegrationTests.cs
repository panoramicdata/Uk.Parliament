using Xunit;
using Uk.Parliament.Extensions;
using Uk.Parliament.Models.Interests;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Member Interests API
/// </summary>
public class InterestsIntegrationTests : IDisposable
{
	private readonly ParliamentClient _client;

	public InterestsIntegrationTests()
	{
		_client = new ParliamentClient();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetMemberInterestsAsync_WithValidMemberId_ReturnsInterests()
	{
		// Arrange
		const int memberId = 172; // Example member ID

		// Act
		var result = await _client.Interests.GetMemberInterestsAsync(memberId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.MemberId.Should().Be(memberId);
		_ = result.Categories.Should().NotBeNull();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetMemberInterestsAsync_WithInvalidMemberId_ThrowsApiException()
	{
		// Arrange
		const int invalidMemberId = -1;

		// Act
		var act = async () => await _client.Interests.GetMemberInterestsAsync(invalidMemberId);

		// Assert
		_ = await act.Should().ThrowAsync<Refit.ApiException>();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetCategoriesAsync_ReturnsCategories()
	{
		// Act
		var result = await _client.Interests.GetCategoriesAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().NotBeEmpty();
		_ = result.Should().AllSatisfy(category =>
		{
			_ = category.Id.Should().BeGreaterThan(0);
			_ = category.Name.Should().NotBeNullOrEmpty();
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task SearchInterestsAsync_WithNoFilters_ReturnsResults()
	{
		// Act
		var result = await _client.Interests.SearchInterestsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.TotalResults.Should().BeGreaterThan(0);
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task SearchInterestsAsync_WithSearchTerm_ReturnsFilteredResults()
	{
		// Arrange
		const string searchTerm = "employment";

		// Act
		var result = await _client.Interests.SearchInterestsAsync(searchTerm: searchTerm, take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task SearchInterestsAsync_WithCategoryFilter_ReturnsFilteredResults()
	{
		// Arrange
		const int categoryId = 1;

		// Act
		var result = await _client.Interests.SearchInterestsAsync(categoryId: categoryId, take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.CategoryId.Should().Be(categoryId);
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetAllInterestsAsync_StreamsResults()
	{
		// Arrange
		var interests = new List<Interest>();

		// Act
		await foreach (var interest in _client.Interests.GetAllInterestsAsync(pageSize: 5))
		{
			interests.Add(interest);
			if (interests.Count >= 10) break; // Limit for test
		}

		// Assert
		_ = interests.Should().NotBeEmpty();
		_ = interests.Count.Should().BeGreaterThanOrEqualTo(5);
	}

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
}

/// <summary>
/// Unit tests for Interests API (mocking)
/// </summary>
public class InterestsApiUnitTests
{
	[Fact]
	public void InterestsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<IInterestsApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public async Task GetMemberInterestsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IInterestsApi>();
		var expectedInterests = new MemberInterests
		{
			MemberId = 172,
			MemberName = "Test Member",
			Categories =
			[
				new InterestCategoryWithInterests
				{
					Category = new InterestCategory { Id = 1, Name = "Employment", Description = "Paid employment" },
					Interests =
					[
						new Interest { Id = 1, MemberId = 172, CategoryId = 1, InterestDetails = "Test employment" }
					]
				}
			]
		};

		_ = mockApi.Setup(x => x.GetMemberInterestsAsync(172, It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedInterests);

		// Act
		var result = await mockApi.Object.GetMemberInterestsAsync(172);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.MemberId.Should().Be(172);
		_ = result.MemberName.Should().Be("Test Member");
		_ = result.Categories.Should().HaveCount(1);
	}

	[Fact]
	public async Task GetCategoriesAsync_WithMock_ReturnsCategories()
	{
		// Arrange
		var mockApi = new Mock<IInterestsApi>();
		var expectedCategories = new List<InterestCategory>
		{
			new() { Id = 1, Name = "Employment", Description = "Paid employment" },
			new() { Id = 2, Name = "Donations", Description = "Donations and gifts" }
		};

		_ = mockApi.Setup(x => x.GetCategoriesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedCategories);

		// Act
		var result = await mockApi.Object.GetCategoriesAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().HaveCount(2);
		_ = result[0].Name.Should().Be("Employment");
		_ = result[1].Name.Should().Be("Donations");
	}

	[Fact]
	public async Task SearchInterestsAsync_WithMock_ReturnsResults()
	{
		// Arrange
		var mockApi = new Mock<IInterestsApi>();
		var expectedResponse = new PaginatedResponse<Interest>
		{
			TotalResults = 2,
			Items =
			[
				new ValueWrapper<Interest>
				{
					Value = new Interest { Id = 1, MemberId = 172, CategoryId = 1, InterestDetails = "Test interest 1" }
				},
				new ValueWrapper<Interest>
				{
					Value = new Interest { Id = 2, MemberId = 172, CategoryId = 1, InterestDetails = "Test interest 2" }
				}
			]
		};

		_ = mockApi.Setup(x => x.SearchInterestsAsync(
			It.IsAny<string>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.SearchInterestsAsync(searchTerm: "test");

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}
}
