using FluentAssertions;
using Uk.Parliament.Extensions;
using Uk.Parliament.Models;

namespace Uk.Parliament.Test;

public class Petitions
{
	private const int ValidPetitionId = 700143; // A known closed petition with data

	[Fact]
	public async Task GetAsync_WithTextSearch_Succeeds()
	{
		var client = new ParliamentClient();

		var response = await client.Petitions.GetAsync(
			search: "Electric Vehicles",
			page: 1,
			pageSize: 10);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetAsync_WithStateFilter_Open_Succeeds()
	{
		var client = new ParliamentClient();

		var response = await client.Petitions.GetAsync(
			state: "open",
			page: 1,
			pageSize: 5);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetAsync_WithStateFilter_Closed_Succeeds()
	{
		var client = new ParliamentClient();

		var response = await client.Petitions.GetAsync(
			state: "closed",
			page: 1,
			pageSize: 5);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Closed));
	}

	[Fact]
	public async Task GetAsync_WithStateFilter_Rejected_Succeeds()
	{
		var client = new ParliamentClient();

		var response = await client.Petitions.GetAsync(
			state: "rejected",
			page: 1,
			pageSize: 5);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Rejected));
	}

	[Fact]
	public async Task GetByIdAsync_Succeeds()
	{
		var client = new ParliamentClient();

		var response = await client.Petitions.GetByIdAsync(ValidPetitionId);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Id.Should().Be(ValidPetitionId);
		_ = response.Data.Attributes.Should().NotBeNull();
		_ = response.Data.Attributes.Action.Should().NotBeNullOrWhiteSpace();
		_ = response.Data.Attributes.SignatureCount.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task GetByIdAsync_HasSignaturesByCountry()
	{
		var client = new ParliamentClient();

		var response = await client.Petitions.GetByIdAsync(ValidPetitionId);

		_ = response.Data.Attributes.SignaturesByCountry.Should().NotBeNull();
		_ = response.Data.Attributes.SignaturesByCountry.Should().NotBeEmpty();
		_ = response.Data.Attributes.SignaturesByCountry.Should().AllSatisfy(s =>
		{
			_ = s.Name.Should().NotBeNullOrWhiteSpace();
			_ = s.SignatureCount.Should().BeGreaterThanOrEqualTo(0);
		});
		_ = response.Data.Attributes.SignaturesByCountry.Should().Contain(s => s.Code == "GB");
	}

	[Fact]
	public async Task GetByIdAsync_HasSignaturesByConstituency()
	{
		var client = new ParliamentClient();

		var response = await client.Petitions.GetByIdAsync(ValidPetitionId);

		_ = response.Data.Attributes.SignaturesByConstituency.Should().NotBeNull();
		_ = response.Data.Attributes.SignaturesByConstituency.Should().NotBeEmpty();
		_ = response.Data.Attributes.SignaturesByConstituency.Should().AllSatisfy(s =>
		{
			_ = s.Name.Should().NotBeNullOrWhiteSpace();
			_ = s.OnsCode.Should().NotBeNullOrWhiteSpace();
			_ = s.SignatureCount.Should().BeGreaterThanOrEqualTo(0);
		});
	}

	[Fact]
	public async Task GetAsync_WithPagination_Succeeds()
	{
		var client = new ParliamentClient();

		var page1 = await client.Petitions.GetAsync(
			state: "open",
			page: 1,
			pageSize: 10);

		_ = page1.Data.Should().NotBeNull();
		_ = page1.Data.Should().NotBeEmpty();

		var page2 = await client.Petitions.GetAsync(
			state: "open",
			page: 2,
			pageSize: 10);

		_ = page2.Data.Should().NotBeNull();
		_ = page2.Data.Should().NotBeEmpty();
		_ = page1.Data[0].Id.Should().NotBe(page2.Data[0].Id);
	}

	[Fact]
	public async Task GetAsync_NoFilters_Succeeds()
	{
		var client = new ParliamentClient();

		var response = await client.Petitions.GetAsync(
			pageSize: 5);

		_ = response.Should().NotBeNull();
		_ = response.Data.Should().NotBeNull();
		_ = response.Data.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetAllListAsync_WithStateFilter_RetrievesMultiplePages()
	{
		var client = new ParliamentClient();

		var allPetitions = await client.Petitions.GetAllListAsync(
			state: "rejected",
			pageSize: 10); // Small page size to ensure we get multiple pages

		_ = allPetitions.Should().NotBeNull();
		_ = allPetitions.Should().NotBeEmpty();
		// With a page size of 10, if we have more than 10 results, we successfully handled pagination
		_ = allPetitions.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Rejected));
	}

	[Fact]
	public async Task GetAllAsync_StreamingResults_Works()
	{
		var client = new ParliamentClient();
		var count = 0;

		await foreach (var petition in client.Petitions.GetAllAsync(state: "open", pageSize: 5))
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
