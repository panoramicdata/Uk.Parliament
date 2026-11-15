using System.Globalization;
using System.Net.Http;
using System.Web;

namespace Uk.Parliament.Test;

/// <summary>Offline tests that drive every GetAllAsync strategy through a stub server.</summary>
public class ParliamentClientPaginationTests
{
	private const int Total = 3;
	private const int PageSize = 2;

	private static readonly Dictionary<string, string> ItemTemplates = new()
	{
		["bills-api"] = """{"billId":{id}}""",
		["committees-api"] = """{"id":{id}}""",
		["treaties-api"] = """{"value":{"id":"T{id}"}}""",
		["members-api"] = """{"value":{"id":{id}}}""",
		["interests-api"] = """{"id":{id}}""",
		["questions-statements-api"] = """{"value":{"id":{id}}}""",
		["oralquestionsandmotions-api"] = """{"id":{id}}""",
		["petition"] = """{"id":{id},"type":"petition","attributes":{"action":"A{id}","background":"b","additional_details":"d","state":"open","signature_count":1,"created_at":"2024-01-01T00:00:00Z"}}"""
	};

	private static readonly Dictionary<string, Func<ParliamentClient, CancellationToken, Task<int>>> Cases = new()
	{
		["bills"] = (c, ct) => CountAsync(c, new GetBillsRequest { Take = PageSize }, ct),
		["committees"] = (c, ct) => CountAsync(c, new GetCommitteesRequest { Take = PageSize }, ct),
		["treaties"] = (c, ct) => CountAsync(c, new GetTreatiesRequest { Take = PageSize }, ct),
		["members"] = (c, ct) => CountAsync(c, new SearchMembersRequest { Take = PageSize }, ct),
		["constituencies"] = (c, ct) => CountAsync(c, new SearchConstituenciesRequest { Take = PageSize }, ct),
		["interests"] = (c, ct) => CountAsync(c, new SearchInterestsRequest { Take = PageSize }, ct),
		["written-questions"] = (c, ct) => CountAsync(c, new GetWrittenQuestionsRequest { Take = PageSize }, ct),
		["written-statements"] = (c, ct) => CountAsync(c, new GetWrittenStatementsRequest { Take = PageSize }, ct),
		["daily-reports"] = (c, ct) => CountAsync(c, new GetDailyReportsRequest { Take = PageSize }, ct),
		["oral-questions"] = (c, ct) => CountAsync(c, new GetOralQuestionsRequest { Take = PageSize }, ct),
		["motions"] = (c, ct) => CountAsync(c, new GetMotionsRequest { Take = PageSize }, ct),
		["petitions"] = (c, ct) => CountAsync(c, new GetPetitionsRequest { PageSize = PageSize }, ct)
	};

	private sealed record UnsupportedRequest : IPaginatedRequest<string>;

	private static async Task<int> CountAsync<TItem>(ParliamentClient client, IPaginatedRequest<TItem> request, CancellationToken ct)
		=> (await client.GetAllListAsync(request, ct)).Count;

	private static int ParseInt(string? value, int fallback)
		=> value is null ? fallback : int.Parse(value, CultureInfo.InvariantCulture);

	private static string Respond(HttpRequestMessage request, int total)
	{
		var uri = request.RequestUri!;
		var query = HttpUtility.ParseQueryString(uri.Query);
		var host = uri.Host.Split('.')[0];
		var size = ParseInt(query["take"] ?? query["page_size"], PageSize);
		var skip = query["skip"] is null
			? (ParseInt(query["page"], 1) - 1) * size
			: ParseInt(query["skip"], 0);
		var items = string.Join(",", Enumerable.Range(skip, Math.Max(0, Math.Min(size, total - skip)))
			.Select(i => ItemTemplates[host].Replace("{id}", (i + 1).ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal)));

		return host switch
		{
			"bills-api" or "committees-api" => $$"""{"items":[{{items}}],"totalResults":{{total}},"itemsPerPage":{{size}}}""",
			"oralquestionsandmotions-api" => $$$"""{"Response":[{{{items}}}],"PagingInfo":{"Skip":{{{skip}}},"Take":{{{size}}},"Total":{{{total}}}}}""",
			"petition" => $$"""{"links":{"self":"x"},"data":[{{items}}]}""",
			_ => $$"""{"items":[{{items}}],"totalResults":{{total}},"skip":{{skip}},"take":{{size}}}"""
		};
	}

	private static (ParliamentClient Client, StubHttpMessageHandler Stub) CreateClient(int total)
	{
		var stub = new StubHttpMessageHandler(request => Respond(request, total));
		return (new ParliamentClient(stub, new ParliamentClientOptions()), stub);
	}

	private static List<string?> QueryValues(StubHttpMessageHandler stub, string key)
		=> [.. stub.Requests.Select(r => HttpUtility.ParseQueryString(r.RequestUri!.Query)[key])];

	/// <summary>Every strategy walks all pages and returns each item once.</summary>
	[Theory]
	[InlineData("bills")]
	[InlineData("committees")]
	[InlineData("treaties")]
	[InlineData("members")]
	[InlineData("constituencies")]
	[InlineData("interests")]
	[InlineData("written-questions")]
	[InlineData("written-statements")]
	[InlineData("daily-reports")]
	[InlineData("oral-questions")]
	[InlineData("motions")]
	[InlineData("petitions")]
	public async Task GetAllListAsync_WalksEveryPage(string name)
	{
		var (client, stub) = CreateClient(Total);
		using var owner = client;

		var count = await Cases[name](client, TestContext.Current.CancellationToken);

		_ = count.Should().Be(Total);
		_ = stub.Requests.Should().HaveCount(2);
	}

	/// <summary>An empty first page ends the walk without a second request.</summary>
	[Theory]
	[InlineData("bills")]
	[InlineData("petitions")]
	public async Task GetAllListAsync_WithNoResults_ReturnsEmpty(string name)
	{
		var (client, stub) = CreateClient(0);
		using var owner = client;

		var count = await Cases[name](client, TestContext.Current.CancellationToken);

		_ = count.Should().Be(0);
		_ = stub.Requests.Should().ContainSingle();
	}

	/// <summary>The offset strategies send skip and take, so the second page starts after the first.</summary>
	[Fact]
	public async Task GetAllAsync_ForOffsetRequest_AdvancesSkipByPageSize()
	{
		var (client, stub) = CreateClient(Total);
		using var owner = client;

		_ = await client.GetAllListAsync(new GetBillsRequest { Take = PageSize }, TestContext.Current.CancellationToken);

		_ = QueryValues(stub, "skip").Should().Equal("0", "2");
	}

	/// <summary>The petitions strategy sends one-based page numbers.</summary>
	[Fact]
	public async Task GetAllAsync_ForPetitionsRequest_AdvancesPageNumber()
	{
		var (client, stub) = CreateClient(Total);
		using var owner = client;

		_ = await client.GetAllListAsync(new GetPetitionsRequest { PageSize = PageSize }, TestContext.Current.CancellationToken);

		_ = QueryValues(stub, "page").Should().Equal("1", "2");
	}

	/// <summary>A request type with no registered strategy is rejected.</summary>
	[Fact]
	public void GetAllAsync_ForUnknownRequest_Throws()
	{
		var (client, _) = CreateClient(Total);
		using var owner = client;

		var act = () => client.GetAllAsync(new UnsupportedRequest(), TestContext.Current.CancellationToken);

		_ = act.Should().Throw<NotSupportedException>().WithMessage("*UnsupportedRequest*");
	}
}
