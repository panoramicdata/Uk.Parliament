using System;
using Xunit;
using Uk.Parliament.Extensions;
using Uk.Parliament.Models.Treaties;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Treaties API
/// </summary>
public class TreatiesIntegrationTests : IDisposable
{
	private readonly ParliamentClient _client;

	public TreatiesIntegrationTests()
	{
		_client = new ParliamentClient();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetTreatiesAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await _client.Treaties.GetTreatiesAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.TotalResults.Should().BeGreaterThan(0);
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetTreatiesAsync_FilterByStatus_ReturnsTreaties()
	{
		// Arrange
		const string status = "In Force";

		// Act
		var result = await _client.Treaties.GetTreatiesAsync(
			status: status,
			take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.Items.Should().AllSatisfy(item =>
		{
			_ = item.Value.Status.Should().Be(status);
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetTreatyByIdAsync_WithValidId_ReturnsTreaty()
	{
		// Arrange
		const int treatyId = 1;

		// Act
		var result = await _client.Treaties.GetTreatyByIdAsync(treatyId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(treatyId);
		_ = result.Title.Should().NotBeNullOrEmpty();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetTreatyBusinessItemsAsync_WithValidId_ReturnsBusinessItems()
	{
		// Arrange
		const int treatyId = 1;

		// Act
		var result = await _client.Treaties.GetTreatyBusinessItemsAsync(treatyId);

		// Assert
		_ = result.Should().NotBeNull();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetGovernmentOrganisationsAsync_ReturnsOrganisations()
	{
		// Act
		var result = await _client.Treaties.GetGovernmentOrganisationsAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().NotBeEmpty();
		_ = result.Should().AllSatisfy(org =>
		{
			_ = org.Id.Should().BeGreaterThan(0);
			_ = org.Name.Should().NotBeNullOrEmpty();
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetAllTreatiesAsync_StreamsResults()
	{
		// Arrange
		var treaties = new List<Treaty>();

		// Act
		await foreach (var treaty in _client.Treaties.GetAllTreatiesAsync(pageSize: 5))
		{
			treaties.Add(treaty);
			if (treaties.Count >= 10) break;
		}

		// Assert
		_ = treaties.Should().NotBeEmpty();
		_ = treaties.Count.Should().BeGreaterThanOrEqualTo(5);
	}

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
}

/// <summary>
/// Unit tests for Treaties API (mocking)
/// </summary>
public class TreatiesApiUnitTests
{
	[Fact]
	public void TreatiesApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ITreatiesApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public async Task GetTreatiesAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<ITreatiesApi>();
		var expectedResponse = new PaginatedResponse<Treaty>
		{
			TotalResults = 2,
			Items =
			[
				new ValueWrapper<Treaty>
				{
					Value = new Treaty
					{
						Id = 1,
						CommandPaperNumber = "CP 123",
						Title = "Test Treaty 1",
						Status = "In Force",
						DateLaid = DateTime.Now.AddMonths(-6)
					}
				},
				new ValueWrapper<Treaty>
				{
					Value = new Treaty
					{
						Id = 2,
						CommandPaperNumber = "CP 124",
						Title = "Test Treaty 2",
						Status = "Not Yet In Force",
						DateLaid = DateTime.Now.AddMonths(-3)
					}
				}
			]
		};

		_ = mockApi.Setup(x => x.GetTreatiesAsync(
			It.IsAny<int?>(),
			It.IsAny<string?>(),
			It.IsAny<string?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<DateTime?>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetTreatiesAsync(take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetGovernmentOrganisationsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<ITreatiesApi>();
		var expectedOrgs = new List<GovernmentOrganisation>
		{
			new() { Id = 1, Name = "Foreign, Commonwealth & Development Office", Abbreviation = "FCDO", IsActive = true },
			new() { Id = 2, Name = "Department for Business and Trade", Abbreviation = "DBT", IsActive = true }
		};

		_ = mockApi.Setup(x => x.GetGovernmentOrganisationsAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedOrgs);

		// Act
		var result = await mockApi.Object.GetGovernmentOrganisationsAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().HaveCount(2);
		_ = result[0].Name.Should().Be("Foreign, Commonwealth & Development Office");
		_ = result[1].Abbreviation.Should().Be("DBT");
	}

	[Fact]
	public async Task GetTreatyBusinessItemsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<ITreatiesApi>();
		var expectedItems = new List<TreatyBusinessItem>
		{
			new()
			{
				Id = 1,
				TreatyId = 1,
				BusinessItemType = "Debate",
				House = "Commons",
				Date = DateTime.Now.AddDays(-10)
			}
		};

		_ = mockApi.Setup(x => x.GetTreatyBusinessItemsAsync(1, It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedItems);

		// Act
		var result = await mockApi.Object.GetTreatyBusinessItemsAsync(1);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().HaveCount(1);
		_ = result[0].BusinessItemType.Should().Be("Debate");
	}
}
