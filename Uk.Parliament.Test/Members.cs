using Uk.Parliament.Extensions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Members API
/// </summary>
public class Members
{
	[Fact]
	public async Task SearchAsync_WithNoFilters_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var response = await client.Members.SearchAsync(take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task SearchAsync_WithNameFilter_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var response = await client.Members.SearchAsync(name: "Johnson", take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		// Note: API performs substring matching, so "Johnson" will match members with Johnson in their name
		_ = response.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.NameDisplayAs.Should().NotBeNullOrWhiteSpace();
			_ = item.Value.NameListAs.Should().NotBeNullOrWhiteSpace();
		});
	}

	[Fact]
	public async Task SearchAsync_ForCurrentMembers_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var response = await client.Members.SearchAsync(isCurrentMember: true, take: 20);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task SearchAsync_ForCommonsMembers_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act - House 1 = Commons
		var response = await client.Members.SearchAsync(house: 1, isCurrentMember: true, take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.LatestHouseMembership.Should().NotBeNull();
			_ = item.Value.LatestHouseMembership!.House.Should().Be(1, "should only return Commons members");
		});
	}

	[Fact]
	public async Task SearchAsync_ForLordsMembers_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act - House 2 = Lords
		var response = await client.Members.SearchAsync(house: 2, isCurrentMember: true, take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.LatestHouseMembership.Should().NotBeNull();
			_ = item.Value.LatestHouseMembership!.House.Should().Be(2, "should only return Lords members");
		});
	}

	[Fact]
	public async Task GetByIdAsync_WithValidId_ReturnsMember()
	{
		// Arrange
		var client = new ParliamentClient();
		// First, get a valid member ID
		var searchResponse = await client.Members.SearchAsync(take: 1);
		var memberId = searchResponse.Items[0].Value.Id;

		// Act
		var memberWrapper = await client.Members.GetByIdAsync(memberId);

		// Assert
		_ = memberWrapper.Should().NotBeNull();
		_ = memberWrapper.Value.Should().NotBeNull();
		_ = memberWrapper.Value.Id.Should().Be(memberId);
		_ = memberWrapper.Value.NameDisplayAs.Should().NotBeNullOrWhiteSpace();
		_ = memberWrapper.Value.NameFullTitle.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task SearchAsync_WithPagination_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act - Get first page
		var page1 = await client.Members.SearchAsync(skip: 0, take: 10, isCurrentMember: true);

		// Act - Get second page
		var page2 = await client.Members.SearchAsync(skip: 10, take: 10, isCurrentMember: true);

		// Assert
		_ = page1.Items.Should().NotBeEmpty();
		_ = page2.Items.Should().NotBeEmpty();
		_ = page1.Items[0].Value.Id.Should().NotBe(page2.Items[0].Value.Id, "different pages should have different members");
	}

	[Fact]
	public async Task SearchConstituenciesAsync_WithNoFilters_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var response = await client.Members.SearchConstituenciesAsync(take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task SearchConstituenciesAsync_WithSearchText_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var response = await client.Members.SearchConstituenciesAsync(searchText: "Westminster", take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		// Verify all returned constituencies have data
		_ = response.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.Name.Should().NotBeNullOrWhiteSpace();
			_ = item.Value.Id.Should().BeGreaterThan(0);
		});
	}

	[Fact]
	public async Task GetConstituencyByIdAsync_WithValidId_ReturnsConstituency()
	{
		// Arrange
		var client = new ParliamentClient();
		// First, get a valid constituency ID
		var searchResponse = await client.Members.SearchConstituenciesAsync(take: 1);
		var constituencyId = searchResponse.Items[0].Value.Id;

		// Act
		var constituencyWrapper = await client.Members.GetConstituencyByIdAsync(constituencyId);

		// Assert
		_ = constituencyWrapper.Should().NotBeNull();
		_ = constituencyWrapper.Value.Should().NotBeNull();
		_ = constituencyWrapper.Value.Id.Should().Be(constituencyId);
		_ = constituencyWrapper.Value.Name.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task GetAllAsync_StreamingMembers_Works()
	{
		// Arrange
		var client = new ParliamentClient();
		var count = 0;

		// Act
		await foreach (var member in client.Members.GetAllAsync(name: "Brown", pageSize: 5))
		{
			_ = member.Should().NotBeNull();
			_ = member.NameDisplayAs.Should().NotBeNullOrWhiteSpace();
			count++;

			if (count >= 10)
			{
				break; // Test pagination by getting at least 2 pages
			}
		}

		// Assert
		_ = count.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task GetAllListAsync_RetrievesMultiplePages()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var allMembers = await client.Members.GetAllListAsync(
			house: 1, // Commons
			isCurrentMember: true,
			pageSize: 20);

		// Assert
		_ = allMembers.Should().NotBeNull();
		_ = allMembers.Should().NotBeEmpty();
		_ = allMembers.Should().AllSatisfy(m =>
		{
			_ = m.LatestHouseMembership.Should().NotBeNull();
			_ = m.LatestHouseMembership!.House.Should().Be(1);
		});
	}
}
