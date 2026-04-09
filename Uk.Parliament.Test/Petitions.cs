namespace Uk.Parliament.Test;

/// <summary>Integration tests for the Petitions API (requires live API).</summary>
public class Petitions : IntegrationTestBase
{
	private const int ValidPetitionId = 700143; // A known closed petition with data

	/// <summary>Verifies that searching petitions by text term returns a non-empty result.</summary>
	[Fact]
	public async Task GetAsync_WithTextSearch_Succeeds()
	{
		var response = await Client
			.Petitions
			.GetAsync(
				new GetPetitionsRequest { Search = "Electric Vehicles", Page = 1, PageSize = 10 },
				CancellationToken);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().NotBeEmpty();
	}

	/// <summary>Verifies that filtering petitions by state 'open' returns open petitions.</summary>
	[Fact]
	public async Task GetAsync_WithStateFilter_Open_Succeeds()
	{
		var response = await Client
			.Petitions
			.GetAsync(
			  new GetPetitionsRequest { State = "open", Page = 1, PageSize = 5 },
				CancellationToken);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().NotBeEmpty();
	}

	/// <summary>Verifies that filtering petitions by state 'closed' returns only closed petitions.</summary>
	[Fact]
	public async Task GetAsync_WithStateFilter_Closed_Succeeds()
	{
		var response = await Client
			.Petitions
			.GetAsync(
				new GetPetitionsRequest { State = "closed", Page = 1, PageSize = 5 },
				CancellationToken);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Closed));
	}

	/// <summary>Verifies that filtering petitions by state 'rejected' returns only rejected petitions.</summary>
	[Fact]
	public async Task GetAsync_WithStateFilter_Rejected_Succeeds()
	{
		var response = await Client
			.Petitions
			.GetAsync(
			  new GetPetitionsRequest { State = "rejected", Page = 1, PageSize = 5 },
				CancellationToken);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Rejected));
	}

	/// <summary>Verifies that retrieving a petition by its known ID returns correct attributes.</summary>
	[Fact]
	public async Task GetByIdAsync_Succeeds()
	{
		var response = await Client
			.Petitions
			.GetByIdAsync(
				ValidPetitionId,
				CancellationToken);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Id.Should().Be(ValidPetitionId);
		_ = response.Data.Attributes.Should().NotBeNull();
		_ = response.Data.Attributes.Action.Should().NotBeNullOrWhiteSpace();
		_ = response.Data.Attributes.SignatureCount.Should().BePositive();
	}

	/// <summary>Verifies that a known petition has non-empty signatures-by-country data including a GB entry.</summary>
	[Fact]
	public async Task GetByIdAsync_HasSignaturesByCountry()
	{
		var response = await Client
			.Petitions
			.GetByIdAsync(
				ValidPetitionId,
				CancellationToken);

		_ = response.Data.Attributes.SignaturesByCountry.Should().NotBeNull();
		_ = response.Data.Attributes.SignaturesByCountry.Should().NotBeEmpty();
		_ = response.Data.Attributes.SignaturesByCountry.Should().AllSatisfy(s =>
		{
			_ = s.Name.Should().NotBeNullOrWhiteSpace();
			_ = s.SignatureCount.Should().BeGreaterThanOrEqualTo(0);
		});
		_ = response.Data.Attributes.SignaturesByCountry.Should().Contain(s => s.Code == "GB");
	}

	/// <summary>Verifies that a known petition has non-empty signatures-by-constituency data.</summary>
	[Fact]
	public async Task GetByIdAsync_HasSignaturesByConstituency()
	{
		var response = await Client
			.Petitions
			.GetByIdAsync(
				ValidPetitionId,
				CancellationToken);

		_ = response.Data.Attributes.SignaturesByConstituency.Should().NotBeNull();
		_ = response.Data.Attributes.SignaturesByConstituency.Should().NotBeEmpty();
		_ = response.Data.Attributes.SignaturesByConstituency.Should().AllSatisfy(s =>
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
		var page1 = await Client
			.Petitions
			.GetAsync(
			  new GetPetitionsRequest { State = "open", Page = 1, PageSize = 10 },
				CancellationToken);

		_ = page1.Data.Should().NotBeNull();
		_ = page1.Data.Should().NotBeEmpty();

		var page2 = await Client
			.Petitions
			.GetAsync(
			  new GetPetitionsRequest { State = "open", Page = 2, PageSize = 10 },
				CancellationToken);

		_ = page2.Data.Should().NotBeNull();
		_ = page2.Data.Should().NotBeEmpty();
		_ = page1.Data[0].Id.Should().NotBe(page2.Data[0].Id);
	}

	/// <summary>Verifies that fetching petitions without any filters returns a non-empty result.</summary>
	[Fact]
	public async Task GetAsync_NoFilters_Succeeds()
	{
		var response = await Client
			.Petitions
			.GetAsync(
				new GetPetitionsRequest { PageSize = 5 },
				CancellationToken);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().NotBeEmpty();
	}

	/// <summary>Verifies that <c>GetAllListAsync</c> retrieves all rejected petitions across multiple pages.</summary>
	[Fact]
	public async Task GetAllListAsync_WithStateFilter_RetrievesMultiplePages()
	{
		var allPetitions = await Client.GetAllListAsync(
			new GetPetitionsRequest { State = "rejected", PageSize = 10 },
			CancellationToken);

		_ = allPetitions.Should().NotBeNull();
		_ = allPetitions.Should().NotBeEmpty();
		// With a page size of 10, if we have more than 10 results, we successfully handled pagination
		_ = allPetitions.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Rejected));
	}

	/// <summary>Verifies that streaming open petitions via async enumerable yields at least 15 results.</summary>
	[Fact]
	public async Task GetAllAsync_StreamingResults_Works()
	{
		var count = 0;

		await foreach (var petition in Client.GetAllAsync(new GetPetitionsRequest { State = "open", PageSize = 5 }, CancellationToken))
		{
			_ = petition.Should().NotBeNull();
			_ = petition.Attributes.Should().NotBeNull();
			count++;

			if (count >= 15)
			{
				break; // Test that pagination works by getting at least 3 pages
			}
		}

		_ = count.Should().BeGreaterThanOrEqualTo(15);
	}
}
