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

	private static Task<List<CommonsDivision>> SearchAsync(ParliamentClient client, SearchCommonsDivisionsRequest request)
		=> client.CommonsDivisions.SearchDivisionsAsync(request, CancellationToken);

	private Task AssertMemberVotingAsync(GetCommonsMemberVotingRequest request, Action<MemberVotingRecord>? recordCheck = null)
		=> RunTolerating404Async(MemberVotingApi, async client =>
		{
			var votingHistory = await client.CommonsDivisions.GetMemberVotingAsync(request, CancellationToken);

			_ = votingHistory.Should().NotBeEmpty();
			_ = votingHistory.Should().AllSatisfy(r =>
			{
				_ = r.MemberId.Should().Be(TestMemberId);
				_ = r.PublishedDivision.Should().NotBeNull();
				recordCheck?.Invoke(r);
			});
		});

	/// <summary>Verifies that fetching Commons divisions without filters returns a non-empty list.</summary>
	[Fact]
	public Task GetDivisionsAsync_WithNoFilters_Succeeds()
		=> RunTolerating404Async(DivisionsApi, async client =>
			AssertDivisionsValid(
				await client.CommonsDivisions.GetDivisionsAsync(new GetCommonsDivisionsRequest { Take = 5 }, cancellationToken: CancellationToken),
				d => d.DivisionId,
				d => d.Title));

	/// <summary>Verifies that fetching a Commons division by a valid ID returns the division with a title.</summary>
	[Fact]
	public Task GetDivisionByIdAsync_WithValidId_ReturnsDivision()
		=> RunDivisionByIdAsync(
			DivisionsApi,
			client => SearchAsync(client, new SearchCommonsDivisionsRequest { SearchTerm = "Budget", Take = 1 }),
			(client, id) => client.CommonsDivisions.GetDivisionByIdAsync(id, CancellationToken),
			d => d.DivisionId,
			d => d.Title);

	/// <summary>Verifies that searching Commons divisions by search term returns results.</summary>
	[Fact]
	public Task SearchDivisionsAsync_WithSearchTerm_ReturnsResults()
		=> RunTolerating404Async(SearchApi, async client =>
			AssertDivisionsValid(
				await SearchAsync(client, new SearchCommonsDivisionsRequest { SearchTerm = "Budget" }),
				d => d.DivisionId,
				d => d.Title));

	/// <summary>Verifies that fetching a Commons member's voting history returns populated records.</summary>
	[Fact]
	public Task GetMemberVotingAsync_WithMemberId_ReturnsVotingHistory()
		=> AssertMemberVotingAsync(new GetCommonsMemberVotingRequest { MemberId = TestMemberId, Take = 5 });

	/// <summary>Verifies that filtering a Commons member's voting history by division number returns results matching the division number.</summary>
	[Fact]
	public Task GetMemberVotingAsync_WithDivisionNumberFilter_Succeeds()
		=> AssertMemberVotingAsync(
			new GetCommonsMemberVotingRequest { MemberId = TestMemberId, DivisionNumber = 512, Take = 5 },
			r => _ = r.PublishedDivision!.Number.Should().Be(512));

	/// <summary>Verifies that paginated Commons division search returns a page of results within the take limit.</summary>
	[Fact]
	public Task SearchDivisionsAsync_WithPagination_Succeeds()
		=> RunPageWithinTakeAsync(
			DivisionsApi,
			client => SearchAsync(client, new SearchCommonsDivisionsRequest { SearchTerm = "Budget", Skip = 0, Take = 10 }),
			10);
}
