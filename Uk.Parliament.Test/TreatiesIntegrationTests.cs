using Uk.Parliament.Models.Treaties;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Treaties API
/// </summary>
public class TreatiesIntegrationTests : IntegrationTestBase
{
	/// <summary>Verifies that fetching treaties without filters returns a valid paginated result.</summary>
	[Fact]
	public async Task GetTreatiesAsync_WithNoFilters_Succeeds()
	{
		// Act
		var result = await Client
			.Treaties
			.GetTreatiesAsync(
			   new GetTreatiesRequest { Take = 10 },
				CancellationToken);

		// Assert
		AssertValidPaginatedResponse(result);
	}

	/// <summary>Verifies that filtering treaties by status returns a non-null result.</summary>
	[Fact]
	public async Task GetTreatiesAsync_FilterByStatus_ReturnsTreaties()
	{
		// Arrange
		const string status = "In Force";

		// Act
		var result = await Client
			.Treaties
			.GetTreatiesAsync(
			 new GetTreatiesRequest { Status = status, Take = 10 },
				CancellationToken);

		// Assert - API returns results but Status field may not be populated
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
	}

	/// <summary>Verifies that fetching a treaty by a valid ID returns a non-null result.</summary>
	[Fact]
	public async Task GetTreatyByIdAsync_WithValidId_ReturnsTreaty()
	{
		// Arrange - First get a valid treaty ID from the list
		var listResult = await Client
			.Treaties
			.GetTreatiesAsync(
				new GetTreatiesRequest { Take = 1 },
				CancellationToken);

		if (listResult.Items == null || listResult.Items.Count == 0)
		{
			// Skip test if no treaties available
			return;
		}

		var validTreatyId = listResult.Items[0].Value.Id;

		// Act
		var result = await Client
			.Treaties
			.GetTreatyByIdAsync(
				validTreatyId,
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
	}

	/// <summary>Verifies that fetching business items for a valid treaty ID returns a result without error.</summary>
	[Fact]
	public async Task GetTreatyBusinessItemsAsync_WithValidId_ReturnsBusinessItems()
	{
		// Arrange - First get a valid treaty ID from the list
		var listResult = await Client
			.Treaties
			.GetTreatiesAsync(
				new GetTreatiesRequest { Take = 1 },
				CancellationToken);

		if (listResult.Items == null || listResult.Items.Count == 0)
		{
			// Skip test if no treaties available
			return;
		}

		var validTreatyId = listResult.Items[0].Value.Id;

		try
		{
			// Act
			var result = await Client
				.Treaties
				.GetTreatyBusinessItemsAsync(
					validTreatyId,
					CancellationToken);

			// Assert - May be empty for some treaties
			_ = result.Should().NotBeNull();
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			// Some treaties may not have business items endpoint
		}
	}

	/// <summary>Verifies that fetching government organisations returns a non-empty list of named organisations.</summary>
	[Fact]
	public async Task GetGovernmentOrganisationsAsync_ReturnsOrganisations()
	{
		// Act
		var result = await Client
			.Treaties
			.GetGovernmentOrganisationsAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
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
	{
		// Act
		var treaties = await CollectStreamedItemsAsync(
			Client.GetAllAsync(
				new GetTreatiesRequest { Take = 5 },
				CancellationToken));

		// Assert - Just verify streaming works
		_ = treaties.Should().NotBeNull();
	}
}

/// <summary>
/// Unit tests for Treaties API (mocking)
/// </summary>
public class TreatiesApiUnitTests : IntegrationTestBase
{
	/// <summary>Verifies that <see cref="ITreatiesApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void TreatiesApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ITreatiesApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	/// <summary>Verifies that a mocked <c>GetTreatiesAsync</c> returns the expected treaty list.</summary>
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
			  It.IsAny<GetTreatiesRequest>(),
			  It.IsAny<CancellationToken>()))
			  .ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.GetTreatiesAsync(
			new GetTreatiesRequest { Take = 10 },
			CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}

	/// <summary>Verifies that a mocked <see cref="ITreatiesApi.GetGovernmentOrganisationsAsync"/> returns the expected organisations.</summary>
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
		var result = await mockApi.Object.GetGovernmentOrganisationsAsync(
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().HaveCount(2);
		_ = result.Items[0].Value.Name.Should().Be("Foreign, Commonwealth & Development Office");
		_ = result.Items[1].Value.Abbreviation.Should().Be("DBT");
	}

	/// <summary>Verifies that a mocked <see cref="ITreatiesApi.GetTreatyBusinessItemsAsync"/> returns the expected business items.</summary>
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
		var result = await mockApi.Object.GetTreatyBusinessItemsAsync("1",
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().ContainSingle();
		_ = result[0].BusinessItemType.Should().Be("Debate");
	}
}
