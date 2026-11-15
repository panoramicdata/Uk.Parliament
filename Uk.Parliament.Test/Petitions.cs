namespace Uk.Parliament.Test;

/// <summary>Integration tests for the Petitions API (requires live API).</summary>
[Trait("Category", "Integration")]
public class Petitions : IntegrationTestBase
{
	private const int ValidPetitionId = 700143; // A known closed petition with data

	private async Task<List<Petition>> GetPetitionsAsync(string? state = null, string? search = null, int page = 1, int pageSize = 5)
	{
		var response = await Client.Petitions.GetAsync(
			new GetPetitionsRequest { Search = search, State = state, Page = page, PageSize = pageSize },
			CancellationToken);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();

		return response.Data;
	}

	private async Task<PetitionAttributes> GetKnownPetitionAttributesAsync()
	{
		var response = await Client.Petitions.GetByIdAsync(ValidPetitionId, CancellationToken);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Id.Should().Be(ValidPetitionId);

		return response.Data.Attributes;
	}

	/// <summary>Verifies that searching petitions by text term returns a non-empty result.</summary>
	[Fact]
	public async Task GetAsync_WithTextSearch_Succeeds()
		=> _ = (await GetPetitionsAsync(search: "Electric Vehicles", pageSize: 10)).Should().NotBeEmpty();

	/// <summary>Verifies that fetching petitions without any filters returns a non-empty result.</summary>
	[Fact]
	public async Task GetAsync_NoFilters_Succeeds()
		=> _ = (await GetPetitionsAsync()).Should().NotBeEmpty();

	/// <summary>Verifies that filtering petitions by state 'open' returns open petitions.</summary>
	[Fact]
	public async Task GetAsync_WithStateFilter_Open_Succeeds()
		=> _ = (await GetPetitionsAsync("open")).Should().NotBeEmpty();

	/// <summary>Verifies that filtering petitions by state 'closed' returns only closed petitions.</summary>
	[Fact]
	public async Task GetAsync_WithStateFilter_Closed_Succeeds()
		=> _ = (await GetPetitionsAsync("closed")).Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Closed));

	/// <summary>Verifies that filtering petitions by state 'rejected' returns only rejected petitions.</summary>
	[Fact]
	public async Task GetAsync_WithStateFilter_Rejected_Succeeds()
		=> _ = (await GetPetitionsAsync("rejected")).Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Rejected));

	/// <summary>Verifies that retrieving a petition by its known ID returns correct attributes.</summary>
	[Fact]
	public async Task GetByIdAsync_Succeeds()
	{
		var attributes = await GetKnownPetitionAttributesAsync();

		_ = attributes.Should().NotBeNull();
		_ = attributes.Action.Should().NotBeNullOrWhiteSpace();
		_ = attributes.SignatureCount.Should().BePositive();
	}

	/// <summary>Verifies that a known petition has non-empty signatures-by-country data including a GB entry.</summary>
	[Fact]
	public async Task GetByIdAsync_HasSignaturesByCountry()
	{
		var countries = (await GetKnownPetitionAttributesAsync()).SignaturesByCountry;

		_ = countries.Should().NotBeNullOrEmpty();
		_ = countries.Should().AllSatisfy(s =>
		{
			_ = s.Name.Should().NotBeNullOrWhiteSpace();
			_ = s.SignatureCount.Should().BeGreaterThanOrEqualTo(0);
		});
		_ = countries.Should().Contain(s => s.Code == "GB");
	}

	/// <summary>Verifies that a known petition has non-empty signatures-by-constituency data.</summary>
	[Fact]
	public async Task GetByIdAsync_HasSignaturesByConstituency()
	{
		var constituencies = (await GetKnownPetitionAttributesAsync()).SignaturesByConstituency;

		_ = constituencies.Should().NotBeNullOrEmpty();
		_ = constituencies.Should().AllSatisfy(s =>
		{
			_ = s.Name.Should().NotBeNullOrWhiteSpace();
			_ = s.OnsCode.Should().NotBeNullOrWhiteSpace();
			_ = s.SignatureCount.Should().BeGreaterThanOrEqualTo(0);
		});
	}

	/// <summary>Verifies that successive pages of petitions return different results.</summary>
	[Fact]
	public async Task GetAsync_WithPagination_Succeeds()
	{
		var page1 = await GetPetitionsAsync("open", page: 1, pageSize: 10);
		var page2 = await GetPetitionsAsync("open", page: 2, pageSize: 10);

		_ = page1.Should().NotBeEmpty();
		_ = page2.Should().NotBeEmpty();
		_ = page1[0].Id.Should().NotBe(page2[0].Id);
	}

	/// <summary>Verifies that <c>GetAllListAsync</c> retrieves all rejected petitions across multiple pages.</summary>
	[Fact]
	public async Task GetAllListAsync_WithStateFilter_RetrievesMultiplePages()
	{
		var allPetitions = await Client.GetAllListAsync(
			new GetPetitionsRequest { State = "rejected", PageSize = 10 },
			CancellationToken);

		_ = allPetitions.Should().NotBeEmpty();
		_ = allPetitions.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Rejected));
	}

	/// <summary>Verifies that streaming open petitions via async enumerable yields at least 15 results.</summary>
	[Fact]
	public async Task GetAllAsync_StreamingResults_Works()
	{
		var petitions = await CollectStreamedItemsAsync(
			Client.GetAllAsync(new GetPetitionsRequest { State = "open", PageSize = 5 }, CancellationToken),
			15);

		_ = petitions.Should().HaveCount(15);
		_ = petitions.Should().AllSatisfy(p => p.Attributes.Should().NotBeNull());
	}
}
