using Uk.Parliament.Extensions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Bills API (requires live API)
/// </summary>
public class BillsIntegrationTests : IntegrationTestBase
{
	[Fact]
	public async Task GetBillsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var response = await Client.Bills.GetBillsAsync(take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BePositive();
	}

	[Fact]
	public async Task GetBillByIdAsync_WithValidId_ReturnsBill()
	{
		// Arrange
		// First, get a valid bill ID
		var billsList = await Client.Bills.GetBillsAsync(take: 1);
		var billId = billsList.Items[0].BillId;

		// Act
		var bill = await Client.Bills.GetBillByIdAsync(billId);

		// Assert
		_ = bill.Should().NotBeNull();
		_ = bill.BillId.Should().Be(billId);
		_ = bill.ShortTitle.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task GetBillsAsync_WithPagination_Succeeds()
	{
		// Act
		var page1 = await Client.Bills.GetBillsAsync(skip: 0, take: 10);
		var page2 = await Client.Bills.GetBillsAsync(skip: 10, take: 10);

		// Assert
		_ = page1.Items.Should().NotBeEmpty();
		_ = page2.Items.Should().NotBeEmpty();
		_ = page1.Items[0].BillId.Should().NotBe(page2.Items[0].BillId, "different pages should have different bills");
	}

	[Fact]
	public async Task GetBillsAsync_FilterByCurrentHouse_Succeeds()
	{
		// Act - Filter by Commons
		var response = await Client.Bills.GetBillsAsync(currentHouse: "Commons", take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.Items.Should().AllSatisfy(bill => _ = bill.CurrentHouse.Should().Be("Commons"));
	}

	[Fact]
	public async Task GetBillsAsync_WithSearchTerm_Succeeds()
	{
		// Act
		var response = await Client.Bills.GetBillsAsync(searchTerm: "Bill", take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BePositive();
	}

	[Fact]
	public async Task GetBillTypesAsync_ReturnsTypes()
	{
		// Act
		var result = await Client.Bills.GetBillTypesAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeEmpty();
		_ = result.Items.Should().AllSatisfy(billType =>
		{
			_ = billType.Name.Should().NotBeNullOrWhiteSpace();
			_ = billType.Category.Should().NotBeNullOrWhiteSpace();
		});
	}

	[Fact]
	public async Task GetAllBillsAsync_StreamingResults_Works()
	{
		// Act
		var bills = await CollectStreamedItemsAsync(
			Client.Bills.GetAllBillsAsync(currentHouse: "Commons", pageSize: 10),
			maxItems: 25);

		// Assert
		_ = bills.Should().NotBeEmpty();
		_ = bills.Should().HaveCountGreaterThanOrEqualTo(25);
		_ = bills.Should().AllSatisfy(bill =>
		{
			_ = bill.Should().NotBeNull();
			_ = bill.ShortTitle.Should().NotBeNullOrWhiteSpace();
		});
	}

	[Fact]
	public async Task GetAllBillsListAsync_RetrievesMultiplePages()
	{
		// Act
		var allBills = await Client.Bills.GetAllBillsListAsync(
			currentHouse: "Lords",
			pageSize: 15);

		// Assert
		_ = allBills.Should().NotBeNull();
		_ = allBills.Should().NotBeEmpty();
		// Should have retrieved all Lords bills with multiple pages
		_ = allBills.Should().AllSatisfy(b => _ = b.CurrentHouse.Should().Be("Lords"));
	}

	[Fact]
	public async Task GetBillByIdAsync_HasCurrentStage()
	{
		// Arrange
		var billsList = await Client.Bills.GetBillsAsync(take: 1);
		var billId = billsList.Items[0].BillId;

		// Act
		var bill = await Client.Bills.GetBillByIdAsync(billId);

		// Assert
		_ = bill.CurrentStage.Should().NotBeNull();
		_ = bill.CurrentStage!.Description.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task GetBillByIdAsync_MayHaveSponsors()
	{
		// Arrange
		var client = new ParliamentClient();
		// Get a bill with sponsors
		var billsList = await client.Bills.GetBillsAsync(take: 10);
		var billWithSponsors = billsList.Items.FirstOrDefault(b => b.Sponsors?.Count > 0);

		if (billWithSponsors == null)
		{
			// Skip test if no bills with sponsors found
			return;
		}

		// Act
		var bill = await client.Bills.GetBillByIdAsync(billWithSponsors.BillId);

		// Assert
		_ = bill.Sponsors.Should().NotBeNull();
		_ = bill.Sponsors.Should().NotBeEmpty();
	}
}
