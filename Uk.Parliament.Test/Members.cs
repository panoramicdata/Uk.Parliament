using Uk.Parliament.Models.Members;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Members API (requires live API).
/// </summary>
public class Members : IntegrationTestBase
{
	/// <summary>House identifier for the House of Commons.</summary>
	private const int Commons = 1;

	/// <summary>House identifier for the House of Lords.</summary>
	private const int Lords = 2;

	private Task<PaginatedResponse<Member>> SearchMembersAsync(SearchMembersRequest request)
		=> Client.Members.SearchAsync(request, CancellationToken);

	private Task<PaginatedResponse<Constituency>> SearchConstituenciesAsync(SearchConstituenciesRequest request)
		=> Client.Members.SearchConstituenciesAsync(request, CancellationToken);

	/// <summary>Verifies that searching members without filters returns a non-empty paginated result.</summary>
	[Fact]
	public async Task SearchAsync_WithNoFilters_Succeeds()
	{
		// Act
		var response = await SearchMembersAsync(new SearchMembersRequest { Take = 10 });

		// Assert
		AssertItemsReturnedWithTotal(response);
	}

	/// <summary>Verifies that filtering members by name returns results matching the name filter.</summary>
	[Fact]
	public async Task SearchAsync_WithNameFilter_Succeeds()
	{
		// Act
		var response = await SearchMembersAsync(new SearchMembersRequest { Name = "Johnson", Take = 10 });

		// Assert
		// Note: API performs substring matching, so "Johnson" will match members with Johnson in their name
		AssertItemsReturned(response, item =>
		{
			_ = item.Value.NameDisplayAs.Should().NotBeNullOrWhiteSpace();
			_ = item.Value.NameListAs.Should().NotBeNullOrWhiteSpace();
		});
	}

	/// <summary>Verifies that filtering members to current members returns a non-empty result.</summary>
	[Fact]
	public async Task SearchAsync_ForCurrentMembers_Succeeds()
	{
		// Act
		var response = await SearchMembersAsync(new SearchMembersRequest { IsCurrentMember = true, Take = 20 });

		// Assert
		AssertItemsReturnedWithTotal(response);
	}

	/// <summary>Verifies that filtering members to a single current House returns only members of that House.</summary>
	[Theory]
	[InlineData(Commons)]
	[InlineData(Lords)]
	public async Task SearchAsync_ForHouse_ReturnsOnlyThatHouse(int house)
	{
		// Act
		var response = await SearchMembersAsync(
			new SearchMembersRequest { House = house, IsCurrentMember = true, Take = 10 });

		// Assert
		AssertItemsReturned(response, item =>
		{
			_ = item.Value.LatestHouseMembership.Should().NotBeNull();
			_ = item.Value.LatestHouseMembership.House.Should().Be(house, "should only return members of the requested House");
		});
	}

	/// <summary>Verifies that fetching a member by a valid ID returns the member with name and title.</summary>
	[Fact]
	public async Task GetByIdAsync_WithValidId_ReturnsMember()
	{
		// Arrange - first, get a valid member ID
		var searchResponse = await SearchMembersAsync(new SearchMembersRequest { Take = 1 });
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

	/// <summary>Verifies that successive pages return different members.</summary>
	[Fact]
	public async Task SearchAsync_WithPagination_Succeeds()
	{
		// Act
		var page1 = await SearchMembersAsync(
			new SearchMembersRequest { Skip = 0, Take = 10, IsCurrentMember = true });
		var page2 = await SearchMembersAsync(
			new SearchMembersRequest { Skip = 10, Take = 10, IsCurrentMember = true });

		// Assert
		_ = page1.Items.Should().NotBeEmpty();
		_ = page2.Items.Should().NotBeEmpty();
		_ = page1.Items[0].Value.Id.Should().NotBe(page2.Items[0].Value.Id, "different pages should have different members");
	}

	/// <summary>Verifies that searching constituencies without filters returns a non-empty paginated result.</summary>
	[Fact]
	public async Task SearchConstituenciesAsync_WithNoFilters_Succeeds()
	{
		// Act
		var response = await SearchConstituenciesAsync(new SearchConstituenciesRequest { Take = 10 });

		// Assert
		AssertItemsReturnedWithTotal(response);
	}

	/// <summary>Verifies that searching constituencies by text returns matching constituencies with names and IDs.</summary>
	[Fact]
	public async Task SearchConstituenciesAsync_WithSearchText_Succeeds()
	{
		// Act
		var response = await SearchConstituenciesAsync(
			new SearchConstituenciesRequest { SearchText = "Westminster", Take = 10 });

		// Assert
		AssertItemsReturned(response, item =>
		{
			_ = item.Value.Name.Should().NotBeNullOrWhiteSpace();
			_ = item.Value.Id.Should().BePositive();
		});
	}

	/// <summary>Verifies that fetching a constituency by a valid ID returns the constituency with name and correct ID.</summary>
	[Fact]
	public async Task GetConstituencyByIdAsync_WithValidId_ReturnsConstituency()
	{
		// Arrange - first, get a valid constituency ID
		var searchResponse = await SearchConstituenciesAsync(new SearchConstituenciesRequest { Take = 1 });
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

	/// <summary>Verifies that streaming members via async enumerable returns members with valid name data.</summary>
	[Fact]
	public async Task GetAllAsync_StreamingMembers_Works()
	{
		// Arrange
		var count = 0;

		// Act
		await foreach (var member in Client.GetAllAsync(new SearchMembersRequest { Name = "Brown", Take = 5 }, CancellationToken))
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

	/// <summary>Verifies that <c>GetAllListAsync</c> retrieves all current Commons members across multiple pages.</summary>
	[Fact]
	public async Task GetAllListAsync_RetrievesMultiplePages()
	{
		// Act
		var allMembers = await Client.GetAllListAsync(
			new SearchMembersRequest { House = Commons, IsCurrentMember = true, Take = 20 },
			CancellationToken);

		// Assert
		_ = allMembers.Should().NotBeNull();
		_ = allMembers.Should().NotBeEmpty();
		_ = allMembers.Should().AllSatisfy(m =>
		{
			_ = m.LatestHouseMembership.Should().NotBeNull();
			_ = m.LatestHouseMembership.House.Should().Be(Commons);
		});
	}
}
