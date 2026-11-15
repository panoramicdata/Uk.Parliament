using Uk.Parliament.Models.Treaties;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Treaties API (mocking)
/// </summary>
public class TreatiesApiUnitTests : IntegrationTestBase
{
	/// <summary>Verifies that <see cref="ITreatiesApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void TreatiesApi_CanBeMocked()
		=> new Mock<ITreatiesApi>().Object.Should().NotBeNull();

	/// <summary>Verifies that a mocked <c>GetTreatiesAsync</c> returns the expected treaty list.</summary>
	[Fact]
	public async Task GetTreatiesAsync_WithMock_ReturnsExpectedData()
	{
		var mockApi = new Mock<ITreatiesApi>();
		_ = mockApi.Setup(x => x.GetTreatiesAsync(It.IsAny<GetTreatiesRequest>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Page(
				new Treaty { Id = "1", CommandPaperNumber = "CP 123", Title = "Test Treaty 1", Status = "In Force" },
				new Treaty { Id = "2", CommandPaperNumber = "CP 124", Title = "Test Treaty 2", Status = "Not Yet In Force" }));

		var result = await mockApi.Object.GetTreatiesAsync(new GetTreatiesRequest { Take = 10 }, CancellationToken);

		_ = result.TotalResults.Should().Be(2);
		_ = result.Items.Should().HaveCount(2);
	}

	/// <summary>Verifies that a mocked <see cref="ITreatiesApi.GetGovernmentOrganisationsAsync"/> returns the expected organisations.</summary>
	[Fact]
	public async Task GetGovernmentOrganisationsAsync_WithMock_ReturnsExpectedData()
	{
		var mockApi = new Mock<ITreatiesApi>();
		_ = mockApi.Setup(x => x.GetGovernmentOrganisationsAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(Page(
				new GovernmentOrganisation { Id = 1, Name = "Foreign, Commonwealth & Development Office", Abbreviation = "FCDO", IsActive = true },
				new GovernmentOrganisation { Id = 2, Name = "Department for Business and Trade", Abbreviation = "DBT", IsActive = true }));

		var result = await mockApi.Object.GetGovernmentOrganisationsAsync(cancellationToken: CancellationToken);

		_ = result.Items.Should().HaveCount(2);
		_ = result.Items[0].Value.Name.Should().Be("Foreign, Commonwealth & Development Office");
		_ = result.Items[1].Value.Abbreviation.Should().Be("DBT");
	}

	/// <summary>Verifies that a mocked <see cref="ITreatiesApi.GetTreatyBusinessItemsAsync"/> returns the expected business items.</summary>
	[Fact]
	public async Task GetTreatyBusinessItemsAsync_WithMock_ReturnsExpectedData()
	{
		var mockApi = new Mock<ITreatiesApi>();
		_ = mockApi.Setup(x => x.GetTreatyBusinessItemsAsync("1", It.IsAny<CancellationToken>()))
			.ReturnsAsync([new TreatyBusinessItem { Id = 1, TreatyId = "1", BusinessItemType = "Debate", House = "Commons" }]);

		var result = await mockApi.Object.GetTreatyBusinessItemsAsync("1", cancellationToken: CancellationToken);

		_ = result.Should().ContainSingle();
		_ = result[0].BusinessItemType.Should().Be("Debate");
	}
}
