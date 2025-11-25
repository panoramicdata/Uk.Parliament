using Uk.Parliament.Extensions;
using Uk.Parliament.Models.Treaties;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Treaties API
/// </summary>
public class TreatiesIntegrationTests : IntegrationTestBase
{

	[Fact]
	public async Task GetTreatiesAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await Client.Treaties.GetTreatiesAsync(take: 10);

		// Assert
		AssertValidPaginatedResponse(result);
	}

	[Fact]
	public async Task GetTreatiesAsync_FilterByStatus_ReturnsTreaties()
	{
		// Arrange
		const string status = "In Force";

		// Act
		var result = await Client.Treaties.GetTreatiesAsync(
			status: status,
			take: 10);

		// Assert
		AssertValidPaginatedResponse(result, item => _ = item.Value.Status.Should().Be(status));
	}

	[Fact]
	public async Task GetTreatyByIdAsync_WithValidId_ReturnsTreaty()
	{
		// Arrange
		const string treatyId = "1";

		// Act
		var result = await Client.Treaties.GetTreatyByIdAsync(treatyId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(treatyId);
		_ = result.Title.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetTreatyBusinessItemsAsync_WithValidId_ReturnsBusinessItems()
	{
		// Arrange
		const string treatyId = "1";

		// Act
		var result = await Client.Treaties.GetTreatyBusinessItemsAsync(treatyId);

		// Assert
		_ = result.Should().NotBeNull();
	}

	[Fact]
	public async Task GetGovernmentOrganisationsAsync_ReturnsOrganisations()
	{
		// Act
		var result = await Client.Treaties.GetGovernmentOrganisationsAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeEmpty();
		_ = result.Items.Should().AllSatisfy(org =>
		{
			_ = org.Value.Id.Should().BePositive();
			_ = org.Value.Name.Should().NotBeNullOrEmpty();
		});
	}

	[Fact]
	public async Task GetAllTreatiesAsync_StreamsResults()
	{
		// Act
		var treaties = await CollectStreamedItemsAsync(
			Client.Treaties.GetAllTreatiesAsync(pageSize: 5));

		// Assert
		AssertValidStreamedResults(treaties);
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
						Id = "1",
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
						Id = "2",
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
		var expectedResponse = new PaginatedResponse<GovernmentOrganisation>
		{
			TotalResults = 2,
			Items =
			[
				new ValueWrapper<GovernmentOrganisation>
				{
					Value = new GovernmentOrganisation { Id = 1, Name = "Foreign, Commonwealth & Development Office", Abbreviation = "FCDO", IsActive = true }
				},
				new ValueWrapper<GovernmentOrganisation>
				{
					Value = new GovernmentOrganisation { Id = 2, Name = "Department for Business and Trade", Abbreviation = "DBT", IsActive = true }
				}
			]
		};

		_ = mockApi.Setup(x => x.GetGovernmentOrganisationsAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetGovernmentOrganisationsAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().HaveCount(2);
		_ = result.Items[0].Value.Name.Should().Be("Foreign, Commonwealth & Development Office");
		_ = result.Items[1].Value.Abbreviation.Should().Be("DBT");
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
				TreatyId = "1",
				BusinessItemType = "Debate",
				House = "Commons",
				Date = DateTime.Now.AddDays(-10)
			}
		};

		_ = mockApi.Setup(x => x.GetTreatyBusinessItemsAsync("1", It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedItems);

		// Act
		var result = await mockApi.Object.GetTreatyBusinessItemsAsync("1");

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().ContainSingle();
		_ = result[0].BusinessItemType.Should().Be("Debate");
	}
}
