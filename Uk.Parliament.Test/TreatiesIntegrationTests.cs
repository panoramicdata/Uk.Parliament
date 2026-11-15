namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Treaties API
/// </summary>
[Trait("Category", "Integration")]
public class TreatiesIntegrationTests : IntegrationTestBase
{
	/// <summary>Verifies that fetching treaties without filters returns a valid paginated result.</summary>
	[Fact]
	public async Task GetTreatiesAsync_WithNoFilters_Succeeds()
		=> AssertValidPaginatedResponse(await Client.Treaties.GetTreatiesAsync(new GetTreatiesRequest { Take = 10 }, CancellationToken));

	/// <summary>Verifies that filtering treaties by status returns a non-null result.</summary>
	[Fact]
	public async Task GetTreatiesAsync_FilterByStatus_ReturnsTreaties()
	{
		var result = await Client.Treaties.GetTreatiesAsync(new GetTreatiesRequest { Status = "In Force", Take = 10 }, CancellationToken);

		_ = result.Items.Should().NotBeNull();
	}

	/// <summary>Verifies that fetching a treaty by a valid ID returns a non-null result.</summary>
	[Fact]
	public async Task GetTreatyByIdAsync_WithValidId_ReturnsTreaty()
	{
		if (await GetFirstTreatyIdAsync() is not { } treatyId)
		{
			return;
		}

		_ = (await Client.Treaties.GetTreatyByIdAsync(treatyId, CancellationToken)).Should().NotBeNull();
	}

	/// <summary>Verifies that fetching business items for a valid treaty ID returns a result without error.</summary>
	[Fact]
	public async Task GetTreatyBusinessItemsAsync_WithValidId_ReturnsBusinessItems()
	{
		if (await GetFirstTreatyIdAsync() is not { } treatyId)
		{
			return;
		}

		try
		{
			_ = (await Client.Treaties.GetTreatyBusinessItemsAsync(treatyId, CancellationToken)).Should().NotBeNull();
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			// Some treaties have no business items endpoint
		}
	}

	/// <summary>Verifies that fetching government organisations returns a non-empty list of named organisations.</summary>
	[Fact]
	public async Task GetGovernmentOrganisationsAsync_ReturnsOrganisations()
	{
		var result = await Client.Treaties.GetGovernmentOrganisationsAsync(CancellationToken);

		_ = result.Items.Should().NotBeEmpty();
		_ = result.Items.Should().AllSatisfy(org =>
		{
			_ = org.Value.Id.Should().BePositive();
			_ = org.Value.Name.Should().NotBeNullOrEmpty();
		});
	}

	/// <summary>Verifies that streaming all treaties via async enumerable completes without error.</summary>
	[Fact]
	public async Task GetAllTreatiesAsync_StreamsResults()
		=> _ = (await CollectStreamedItemsAsync(Client.GetAllAsync(new GetTreatiesRequest { Take = 5 }, CancellationToken))).Should().NotBeNull();

	private async Task<string?> GetFirstTreatyIdAsync()
	{
		var listResult = await Client.Treaties.GetTreatiesAsync(new GetTreatiesRequest { Take = 1 }, CancellationToken);

		return listResult.Items is { Count: > 0 } items ? items[0].Value.Id : null;
	}
}
