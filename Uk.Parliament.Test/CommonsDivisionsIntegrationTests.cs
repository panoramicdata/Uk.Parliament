using Uk.Parliament.Models.Divisions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Commons Divisions API (requires live API)
/// </summary>
/// <remarks>
/// WARNING: The Commons Divisions API endpoints may return 404 errors.
/// These tests handle these errors gracefully.
/// </remarks>
[Trait("Category", "Integration")]
public class CommonsDivisionsIntegrationTests(ITestOutputHelper output) : LoggingIntegrationTestBase(output)
{
	private const string DivisionsApi = "Commons Divisions API";
	private const string SearchApi = "Commons Divisions Search API";
	private const string MemberVotingApi = "Commons Divisions Member Voting API";

	/// <summary>A member with enough voting history for the member voting tests.</summary>
	private const int TestMemberId = 172;

	/// <summary>Verifies that fetching Commons divisions without filters returns a non-empty list.</summary>
	[Fact]
	public Task GetDivisionsAsync_WithNoFilters_Succeeds()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			// Act
			var divisions = await client
				.CommonsDivisions
				.GetDivisionsAsync(
					new GetCommonsDivisionsRequest { Take = 5 },
					cancellationToken: CancellationToken);

			// Assert
			AssertDivisionsValid(divisions, d => d.DivisionId, d => d.Title);
		});

	/// <summary>Verifies that fetching a Commons division by a valid ID returns the division with a title.</summary>
	[Fact]
	public Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			// Arrange - first get a valid division ID
			var divisions = await client
				.CommonsDivisions
				.SearchDivisionsAsync(
					new SearchCommonsDivisionsRequest { SearchTerm = "Budget", Take = 1 },
					CancellationToken);

			if (divisions.Count == 0)
			{
				Output.WriteLine("No divisions found to test GetDivisionByIdAsync");
				return;
			}

			// Act
			var divisionId = divisions[0].DivisionId;
			var result = await client.CommonsDivisions.GetDivisionByIdAsync(divisionId, CancellationToken);

			// Assert
			_ = result.Should().NotBeNull();
			_ = result.DivisionId.Should().Be(divisionId);
			_ = result.Title.Should().NotBeNullOrEmpty();
		});

	/// <summary>Verifies that searching Commons divisions by search term returns results.</summary>
	[Fact]
	public Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
		=> RunTolerating404Async(SearchApi, async client =>
		{
			// Act
			var divisions = await SearchCommonsDivisionsAsync(client, new SearchCommonsDivisionsRequest { SearchTerm = "Budget" });

			// Assert
			AssertDivisionsValid(divisions, d => d.DivisionId, d => d.Title);
		});

	/// <summary>Verifies that fetching a Commons member's voting history returns populated records.</summary>
	[Fact]
	public Task GetMemberVotingAsync_WithMemberId_ReturnsVotingHistory()
		=> RunTolerating404Async(MemberVotingApi, async client =>
		{
			// Act
			var votingHistory = await GetMemberVotingAsync(client, new GetCommonsMemberVotingRequest { MemberId = TestMemberId, Take = 5 });

			// Assert
			AssertMemberVotingHistory(votingHistory, null);
		});

	/// <summary>Verifies that filtering a Commons member's voting history by division number returns results matching the division number.</summary>
	[Fact]
	public Task GetMemberVotingAsync_WithDivisionNumberFilter_Succeeds()
		=> RunTolerating404Async(MemberVotingApi, async client =>
		{
			const int divisionNumber = 512;

			// Act
			var votingHistory = await GetMemberVotingAsync(client,
				new GetCommonsMemberVotingRequest { MemberId = TestMemberId, DivisionNumber = divisionNumber, Take = 5 });

			// Assert
			AssertMemberVotingHistory(votingHistory, r =>
			{
				_ = r.PublishedDivision!.Number.Should().Be(divisionNumber);
			});
		});

	/// <summary>Verifies that paginated Commons division search returns a page of results within the take limit.</summary>
	[Fact]
	public Task SearchDivisionsAsync_WithPagination_Succeeds()
		=> RunTolerating404Async(DivisionsApi, async client =>
		{
			// Act
			var page1 = await SearchCommonsDivisionsAsync(client,
				new SearchCommonsDivisionsRequest { SearchTerm = "Budget", Skip = 0, Take = 10 });

			// Assert
			_ = page1.Should().NotBeNull();
			_ = page1.Count.Should().BeLessThanOrEqualTo(10);
		});

	/// <summary>Performs a Commons divisions search with the supplied request.</summary>
	private static async Task<List<CommonsDivision>> SearchCommonsDivisionsAsync(
		ParliamentClient client,
		SearchCommonsDivisionsRequest request)
		=> await client.CommonsDivisions.SearchDivisionsAsync(request, CancellationToken);

	/// <summary>Performs a Commons member voting fetch with the supplied request.</summary>
	private static async Task<List<MemberVotingRecord>> GetMemberVotingAsync(
		ParliamentClient client,
		GetCommonsMemberVotingRequest request)
		=> await client.CommonsDivisions.GetMemberVotingAsync(request, CancellationToken);

	/// <summary>
	/// Asserts that a member voting history came back non-empty, that every record belongs to the
	/// member under test with a published division, and runs an optional record-specific check.
	/// </summary>
	/// <param name="votingHistory">The voting history returned by the API.</param>
	/// <param name="recordCheck">Optional extra assertion for each record.</param>
	private static void AssertMemberVotingHistory(
		IReadOnlyList<MemberVotingRecord> votingHistory,
		Action<MemberVotingRecord>? recordCheck = null)
	{
		_ = votingHistory.Should().NotBeNull();
		_ = votingHistory.Should().NotBeEmpty();
		_ = votingHistory.Should().AllSatisfy(r =>
		{
			_ = r.MemberId.Should().Be(TestMemberId);
			_ = r.PublishedDivision.Should().NotBeNull();
			recordCheck?.Invoke(r);
		});
	}
}
