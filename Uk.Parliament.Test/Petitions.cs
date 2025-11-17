using FluentAssertions;
using Uk.Parliament.Petitions;

namespace Uk.Parliament.Test;

public class Petitions
{
	private const int ValidPetitionId = 700143; // A known closed petition with data

	[Fact]
	public async Task GetPetitionsAsync_WithTextSearch_Succeeds()
	{
		var petitionsClient = new PetitionsClient();

		var petitions = await petitionsClient.GetPetitionsAsync(new Query
		{
			Text = "Electric Vehicles",
			PageNumber = 1,
			PageSize = 10
		}, default);

		if (!petitions.Ok)
		{
			throw new Exception($"API call failed: {petitions.Exception?.Message}", petitions.Exception);
		}

		petitions.Ok.Should().BeTrue();
		petitions.Data.Should().NotBeNull();
		petitions.Data.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetPetitionsAsync_WithStateFilter_Open_Succeeds()
	{
		var petitionsClient = new PetitionsClient();

		var petitions = await petitionsClient.GetPetitionsAsync(new Query
		{
			State = PetitionState.Open,
			PageNumber = 1,
			PageSize = 5
		}, default);

		if (!petitions.Ok)
		{
			throw new Exception($"API call failed: {petitions.Exception?.Message}", petitions.Exception);
		}

		petitions.Ok.Should().BeTrue();
		petitions.Data.Should().NotBeNull();
		petitions.Data.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetPetitionsAsync_WithStateFilter_Closed_Succeeds()
	{
		var petitionsClient = new PetitionsClient();

		var petitions = await petitionsClient.GetPetitionsAsync(new Query
		{
			State = PetitionState.Closed,
			PageNumber = 1,
			PageSize = 5
		}, default);

		if (!petitions.Ok)
		{
			throw new Exception($"API call failed: {petitions.Exception?.Message}", petitions.Exception);
		}

		petitions.Ok.Should().BeTrue();
		petitions.Data.Should().NotBeNull();
		petitions.Data.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Closed));
	}

	[Fact]
	public async Task GetPetitionsAsync_WithStateFilter_Rejected_Succeeds()
	{
		var petitionsClient = new PetitionsClient();

		var petitions = await petitionsClient.GetPetitionsAsync(new Query
		{
			State = PetitionState.Rejected,
			PageNumber = 1,
			PageSize = 5
		}, default);

		if (!petitions.Ok)
		{
			throw new Exception($"API call failed: {petitions.Exception?.Message}", petitions.Exception);
		}

		petitions.Ok.Should().BeTrue();
		petitions.Data.Should().NotBeNull();
		petitions.Data.Should().AllSatisfy(p => p.Attributes.State.Should().Be(PetitionState.Rejected));
	}

	[Fact]
	public async Task GetPetitionAsync_ById_Succeeds()
	{
		var petitionsClient = new PetitionsClient();

		var petition = await petitionsClient.GetPetitionAsync(ValidPetitionId, default);

		if (!petition.Ok)
		{
			throw new Exception($"API call failed: {petition.Exception?.Message}", petition.Exception);
		}

		petition.Ok.Should().BeTrue();
		petition.Data.Should().NotBeNull();
		petition.Data.Id.Should().Be(ValidPetitionId);
		petition.Data.Attributes.Should().NotBeNull();
		petition.Data.Attributes.Action.Should().NotBeNullOrWhiteSpace();
		petition.Data.Attributes.SignatureCount.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task GetPetitionAsync_HasSignaturesByCountry()
	{
		var petitionsClient = new PetitionsClient();

		var petition = await petitionsClient.GetPetitionAsync(ValidPetitionId, default);

		if (!petition.Ok)
		{
			throw new Exception($"API call failed: {petition.Exception?.Message}", petition.Exception);
		}

		petition.Data.Attributes.SignaturesByCountry.Should().NotBeNull();
		petition.Data.Attributes.SignaturesByCountry.Should().NotBeEmpty();
		petition.Data.Attributes.SignaturesByCountry.Should().AllSatisfy(s =>
		{
			s.Name.Should().NotBeNullOrWhiteSpace();
			s.SignatureCount.Should().BeGreaterThanOrEqualTo(0);
		});
		petition.Data.Attributes.SignaturesByCountry.Should().Contain(s => s.Code == "GB");
	}

	[Fact]
	public async Task GetPetitionAsync_HasSignaturesByConstituency()
	{
		var petitionsClient = new PetitionsClient();

		var petition = await petitionsClient.GetPetitionAsync(ValidPetitionId, default);

		if (!petition.Ok)
		{
			throw new Exception($"API call failed: {petition.Exception?.Message}", petition.Exception);
		}

		petition.Data.Attributes.SignaturesByConstituency.Should().NotBeNull();
		petition.Data.Attributes.SignaturesByConstituency.Should().NotBeEmpty();
		petition.Data.Attributes.SignaturesByConstituency.Should().AllSatisfy(s =>
		{
			s.Name.Should().NotBeNullOrWhiteSpace();
			s.OnsCode.Should().NotBeNullOrWhiteSpace();
			s.SignatureCount.Should().BeGreaterThanOrEqualTo(0);
		});
	}

	[Fact]
	public async Task GetPetitionsAsync_WithPagination_Succeeds()
	{
		var petitionsClient = new PetitionsClient();

		var page1 = await petitionsClient.GetPetitionsAsync(new Query
		{
			State = PetitionState.Open,
			PageNumber = 1,
			PageSize = 10
		}, default);

		if (!page1.Ok)
		{
			throw new Exception($"API call failed: {page1.Exception?.Message}", page1.Exception);
		}

		page1.Data.Should().NotBeNull();
		page1.Data.Should().NotBeEmpty();

		var page2 = await petitionsClient.GetPetitionsAsync(new Query
		{
			State = PetitionState.Open,
			PageNumber = 2,
			PageSize = 10
		}, default);

		if (!page2.Ok)
		{
			throw new Exception($"API call failed: {page2.Exception?.Message}", page2.Exception);
		}

		page2.Data.Should().NotBeNull();
		page2.Data.Should().NotBeEmpty();
		page1.Data[0].Id.Should().NotBe(page2.Data[0].Id);
	}

	[Fact]
	public async Task GetPetitionsAsync_NoFilters_Succeeds()
	{
		var petitionsClient = new PetitionsClient();

		var petitions = await petitionsClient.GetPetitionsAsync(new Query
		{
			PageSize = 5
		}, default);

		if (!petitions.Ok)
		{
			throw new Exception($"API call failed: {petitions.Exception?.Message}", petitions.Exception);
		}

		petitions.Ok.Should().BeTrue();
		petitions.Data.Should().NotBeNull();
		petitions.Data.Should().NotBeEmpty();
	}
}
