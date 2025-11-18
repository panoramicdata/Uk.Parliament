using Uk.Parliament.Extensions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Bills API (requires live API)
/// </summary>
public class BillsIntegrationTests
{
	[Fact]
	public async Task GetBillsAsync_WithNoFilters_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var response = await client.Bills.GetBillsAsync(take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task GetBillByIdAsync_WithValidId_ReturnsBill()
	{
		// Arrange
		var client = new ParliamentClient();
		// First, get a valid bill ID
		var billsList = await client.Bills.GetBillsAsync(take: 1);
		var billId = billsList.Items[0].BillId;

		// Act
		var bill = await client.Bills.GetBillByIdAsync(billId);

		// Assert
		_ = bill.Should().NotBeNull();
		_ = bill.BillId.Should().Be(billId);
		_ = bill.ShortTitle.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task GetBillsAsync_WithPagination_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var page1 = await client.Bills.GetBillsAsync(skip: 0, take: 10);
		var page2 = await client.Bills.GetBillsAsync(skip: 10, take: 10);

		// Assert
		_ = page1.Items.Should().NotBeEmpty();
		_ = page2.Items.Should().NotBeEmpty();
		_ = page1.Items[0].BillId.Should().NotBe(page2.Items[0].BillId, "different pages should have different bills");
	}

	[Fact]
	public async Task GetBillsAsync_FilterByCurrentHouse_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act - Filter by Commons
		var response = await client.Bills.GetBillsAsync(currentHouse: "Commons", take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.Items.Should().AllSatisfy(bill =>
		{
			_ = bill.CurrentHouse.Should().Be("Commons");
		});
	}

	[Fact]
	public async Task GetBillsAsync_WithSearchTerm_Succeeds()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var response = await client.Bills.GetBillsAsync(searchTerm: "Bill", take: 10);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetBillTypesAsync_ReturnsTypes()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var response = await client.Bills.GetBillTypesAsync();

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.Items.Should().AllSatisfy(type =>
		{
			_ = type.Name.Should().NotBeNullOrWhiteSpace();
			_ = type.Category.Should().NotBeNullOrWhiteSpace();
		});
	}

	[Fact]
	public async Task GetAllBillsAsync_StreamingResults_Works()
	{
		// Arrange
		var client = new ParliamentClient();
		var count = 0;

		// Act
		await foreach (var bill in client.Bills.GetAllBillsAsync(currentHouse: "Commons", pageSize: 10))
		{
			_ = bill.Should().NotBeNull();
			_ = bill.ShortTitle.Should().NotBeNullOrWhiteSpace();
			count++;

			if (count >= 25)
			{
				break; // Test pagination by getting at least 3 pages
			}
		}

		// Assert
		_ = count.Should().BeGreaterThanOrEqualTo(25);
	}

	[Fact]
	public async Task GetAllBillsListAsync_RetrievesMultiplePages()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		var allBills = await client.Bills.GetAllBillsListAsync(
			currentHouse: "Lords",
			pageSize: 15);

		// Assert
		_ = allBills.Should().NotBeNull();
		_ = allBills.Should().NotBeEmpty();
		// Should have retrieved all Lords bills with multiple pages
		_ = allBills.Should().AllSatisfy(b =>
		{
			_ = b.CurrentHouse.Should().Be("Lords");
		});
	}

	[Fact]
	public async Task GetBillByIdAsync_HasCurrentStage()
	{
		// Arrange
		var client = new ParliamentClient();
		var billsList = await client.Bills.GetBillsAsync(take: 1);
		var billId = billsList.Items[0].BillId;

		// Act
		var bill = await client.Bills.GetBillByIdAsync(billId);

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
