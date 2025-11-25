using Uk.Parliament.Extensions;
using Uk.Parliament.Models.Interests;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Member Interests API
/// </summary>
public class InterestsIntegrationTests : IntegrationTestBase
{
	[Fact]
	public async Task GetCategoriesAsync_ReturnsCategories()
	{
		// Act
		var result = await Client.Interests.GetCategoriesAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().NotBeEmpty();
		_ = result.Should().AllSatisfy(category =>
		{
			_ = category.Id.Should().BePositive();
			_ = category.Name.Should().NotBeNullOrEmpty();
		});
	}

	[Fact]
	public async Task SearchInterestsAsync_WithNoFilters_ReturnsResults()
	{
		// Act
		var result = await Client.Interests.SearchInterestsAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeEmpty();
		_ = result.TotalResults.Should().BePositive();
	}

	[Fact]
	public async Task SearchInterestsAsync_WithSearchTerm_ReturnsFilteredResults()
	{
		// Arrange
		const string searchTerm = "employment";

		// Act
		var result = await Client.Interests.SearchInterestsAsync(searchTerm: searchTerm, take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task SearchInterestsAsync_WithCategoryFilter_ReturnsFilteredResults()
	{
		// Arrange
		const int categoryId = 1;

		// Act
		var result = await Client.Interests.SearchInterestsAsync(categoryId: categoryId, take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().AllSatisfy(interest => interest.Category.Id.Should().Be(categoryId));
	}

	[Fact]
	public async Task GetAllInterestsAsync_StreamsResults()
	{
		// Act
		var interests = await CollectStreamedItemsAsync(
			Client.Interests.GetAllInterestsAsync(pageSize: 5));

		// Assert
		AssertValidStreamedResults(interests);
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
		var expectedResponse = new InterestsResponse<Interest>
		{
			TotalResults = 2,
			Skip = 0,
			Take = 10,
			Items =
			[
				new()
				{
					Id = 1,
					Summary = "Test interest 1",
					Category = new InterestCategoryInfo { Id = 1, Name = "Employment" },
					Member = new InterestMemberInfo { Id = 172, NameDisplayAs = "Test Member" }
				},
				new()
				{
					Id = 2,
					Summary = "Test interest 2",
					Category = new InterestCategoryInfo { Id = 1, Name = "Employment" },
					Member = new InterestMemberInfo { Id = 172, NameDisplayAs = "Test Member" }
				}
			]
		};

		_ = mockApi.Setup(x => x.SearchInterestsAsync(
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<string?>(),
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
