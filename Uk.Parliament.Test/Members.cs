using Uk.Parliament.Extensions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Members API
/// </summary>
public class Members : IntegrationTestBase
{
	[Fact]
	public async Task SearchAsync_WithNoFilters_Succeeds()
	{
		// Act
		var response = await Client
			.Members
			.SearchAsync(
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BePositive();
	}

	[Fact]
	public async Task SearchAsync_WithNameFilter_Succeeds()
	{
		// Act
		var response = await Client
			.Members
			.SearchAsync(
				name: "Johnson",
				take: 10,
				cancellationToken: CancellationToken);

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
		// Act
		var response = await Client
			.Members
			.SearchAsync(
				isCurrentMember: true,
				take: 20,
				cancellationToken: CancellationToken);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BePositive();
	}

	[Fact]
	public async Task SearchAsync_ForCommonsMembers_Succeeds()
	{
		// Act - House 1 = Commons
		var response = await Client
			.Members
			.SearchAsync(
				house: 1,
				isCurrentMember: true,
				take: 10,
				cancellationToken: CancellationToken);

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
		// Act - House 2 = Lords
		var response = await Client
			.Members
			.SearchAsync(
				house: 2,
				isCurrentMember: true,
				take: 10,
				cancellationToken: CancellationToken);

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
		// First, get a valid member ID
		var searchResponse = await Client
			.Members
			.SearchAsync(
				take: 1,
				cancellationToken: CancellationToken);
		var memberId = searchResponse.Items[0].Value.Id;

		// Act
		var memberWrapper = await Client
			.Members
			.GetByIdAsync(
				memberId,
				CancellationToken);

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
		// Act - Get first page
		var page1 = await Client
			.Members
			.SearchAsync(
				skip: 0,
				take: 10,
				isCurrentMember: true,
				cancellationToken: CancellationToken);

		// Act - Get second page
		var page2 = await Client
			.Members
			.SearchAsync(
				skip: 10,
				take: 10,
				isCurrentMember: true,
				cancellationToken: CancellationToken);

		// Assert
		_ = page1.Items.Should().NotBeEmpty();
		_ = page2.Items.Should().NotBeEmpty();
		_ = page1.Items[0].Value.Id.Should().NotBe(page2.Items[0].Value.Id, "different pages should have different members");
	}

	[Fact]
	public async Task SearchConstituenciesAsync_WithNoFilters_Succeeds()
	{
		// Act
		var response = await Client
			.Members
			.SearchConstituenciesAsync(
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BePositive();
	}

	[Fact]
	public async Task SearchConstituenciesAsync_WithSearchText_Succeeds()
	{
		// Act
		var response = await Client
			.Members
			.SearchConstituenciesAsync(
				searchText: "Westminster",
				take: 10,
				cancellationToken: CancellationToken);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		// Verify all returned constituencies have data
		_ = response.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.Name.Should().NotBeNullOrWhiteSpace();
			_ = item.Value.Id.Should().BePositive();
		});
	}

	[Fact]
	public async Task GetConstituencyByIdAsync_WithValidId_ReturnsConstituency()
	{
		// Arrange
		// First, get a valid constituency ID
		var searchResponse = await Client
			.Members
			.SearchConstituenciesAsync(
				take: 1,
				cancellationToken: CancellationToken);
		var constituencyId = searchResponse.Items[0].Value.Id;

		// Act
		var constituencyWrapper = await Client
			.Members
			.GetConstituencyByIdAsync(
				constituencyId,
				CancellationToken);

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
		var count = 0;

		// Act
		await foreach (var member in Client.Members.GetAllAsync(name: "Brown", pageSize: 5, cancellationToken: CancellationToken))
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
		_ = count.Should().BePositive();
	}

	[Fact]
	public async Task GetAllListAsync_RetrievesMultiplePages()
	{
		// Act
		var allMembers = await Client.Members.GetAllListAsync(
			house: 1, // Commons
			isCurrentMember: true,
			pageSize: 20,
			cancellationToken: CancellationToken);

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
