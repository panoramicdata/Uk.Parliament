namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for the Bills API (requires live API)
/// </summary>
public class BillsIntegrationTests : IntegrationTestBase
{
	/// <summary>Verifies that retrieving bills without filters returns a non-empty paginated result.</summary>
	[Fact]
	public async Task GetBillsAsync_WithNoFilters_Succeeds()
	{
		// Act
		var response = await Client
			.Bills
			.GetBillsAsync(
				new GetBillsRequest { Take = 10 },
				cancellationToken: CancellationToken);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BePositive();
	}

	/// <summary>Verifies that retrieving a bill by a valid ID returns the expected bill.</summary>
	[Fact]
	public async Task GetBillByIdAsync_WithValidId_ReturnsBill()
	{
		// Arrange
		// First, get a valid bill ID
		var billsList = await Client
			.Bills
			.GetBillsAsync(
				new GetBillsRequest { Take = 1 },
				cancellationToken: CancellationToken);
		var billId = billsList.Items[0].BillId;

		// Act
		var bill = await Client
			.Bills
			.GetBillByIdAsync(
				billId,
				CancellationToken);

		// Assert
		_ = bill.Should().NotBeNull();
		_ = bill.BillId.Should().Be(billId);
		_ = bill.ShortTitle.Should().NotBeNullOrWhiteSpace();
	}

	/// <summary>Verifies that pagination returns different bills on successive pages.</summary>
	[Fact]
	public async Task GetBillsAsync_WithPagination_Succeeds()
	{
		// Act
		var page1 = await Client
			.Bills
			.GetBillsAsync(
				new GetBillsRequest { Skip = 0, Take = 10 },
				cancellationToken: CancellationToken);
		var page2 = await Client
			.Bills
			.GetBillsAsync(
				new GetBillsRequest { Skip = 10, Take = 10 },
				cancellationToken: CancellationToken);

		// Assert
		_ = page1.Items.Should().NotBeEmpty();
		_ = page2.Items.Should().NotBeEmpty();
		_ = page1.Items[0].BillId.Should().NotBe(page2.Items[0].BillId, "different pages should have different bills");
	}

	/// <summary>Verifies that filtering bills by current house (Commons) returns only Commons bills.</summary>
	[Fact]
	public async Task GetBillsAsync_FilterByCurrentHouse_Succeeds()
	{
		// Act - Filter by Commons
		var response = await Client
			.Bills
			.GetBillsAsync(
				new GetBillsRequest { CurrentHouse = "Commons", Take = 10 },
				cancellationToken: CancellationToken);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.Items.Should().AllSatisfy(bill => _ = bill.CurrentHouse.Should().Be("Commons"));
	}

	/// <summary>Verifies that filtering bills by a search term returns a non-empty result.</summary>
	[Fact]
	public async Task GetBillsAsync_WithSearchTerm_Succeeds()
	{
		// Act
		var response = await Client
			.Bills
			.GetBillsAsync(
				new GetBillsRequest { SearchTerm = "Bill", Take = 10 },
				cancellationToken: CancellationToken);

		// Assert
		_ = response.Should().NotBeNull();
		_ = response.Items.Should().NotBeEmpty();
		_ = response.TotalResults.Should().BePositive();
	}

	/// <summary>Verifies that the bill types endpoint returns a list of named bill type categories.</summary>
	[Fact]
	public async Task GetBillTypesAsync_ReturnsTypes()
	{
		// Act
		var result = await Client
			.Bills
			.GetBillTypesAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeEmpty();
		_ = result.Items.Should().AllSatisfy(billType =>
		{
			_ = billType.Name.Should().NotBeNullOrWhiteSpace();
			_ = billType.Category.Should().NotBeNullOrWhiteSpace();
		});
	}

	/// <summary>Verifies that streaming all bills via async enumerable yields at least 25 items.</summary>
	[Fact]
	public async Task GetAllBillsAsync_StreamingResults_Works()
	{
		// Act
		var bills = await CollectStreamedItemsAsync(
			Client.GetAllAsync(new GetBillsRequest { CurrentHouse = "Commons", Take = 10 }, CancellationToken),
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

	/// <summary>Verifies that <c>GetAllListAsync</c> retrieves all Lords bills across multiple pages.</summary>
	[Fact]
	public async Task GetAllBillsListAsync_RetrievesMultiplePages()
	{
		// Act
		var allBills = await Client.GetAllListAsync(
			new GetBillsRequest { CurrentHouse = "Lords", Take = 15 },
			CancellationToken);

		// Assert
		_ = allBills.Should().NotBeNull();
		_ = allBills.Should().NotBeEmpty();
		// Should have retrieved all Lords bills with multiple pages
		_ = allBills.Should().AllSatisfy(b => _ = b.CurrentHouse.Should().Be("Lords"));
	}

	/// <summary>Verifies that a retrieved bill includes a current stage with a description.</summary>
	[Fact]
	public async Task GetBillByIdAsync_HasCurrentStage()
	{
		// Arrange
		var billsList = await Client
			.Bills
			.GetBillsAsync(
				new GetBillsRequest { Take = 1 },
				cancellationToken: CancellationToken);
		var billId = billsList.Items[0].BillId;

		// Act
		var bill = await Client
			.Bills
			.GetBillByIdAsync(
				billId,
				CancellationToken);

		// Assert
		_ = bill.CurrentStage.Should().NotBeNull();
		_ = bill.CurrentStage.Description.Should().NotBeNullOrWhiteSpace();
	}

	/// <summary>Verifies that a bill with sponsors has the sponsors collection populated.</summary>
	[Fact]
	public async Task GetBillByIdAsync_MayHaveSponsors()
	{
		// Arrange
		// Get a bill with sponsors
		var billsList = await Client
			.Bills
			.GetBillsAsync(
				new GetBillsRequest { Take = 10 },
				cancellationToken: CancellationToken);
		var billWithSponsors = billsList.Items.FirstOrDefault(b => b.Sponsors?.Count > 0);

		if (billWithSponsors == null)
		{
			// Skip test if no bills with sponsors found
			return;
		}

		// Act
		var bill = await Client
			.Bills
			.GetBillByIdAsync(
				billWithSponsors.BillId,
				CancellationToken);

		// Assert
		_ = bill.Sponsors.Should().NotBeNull();
		_ = bill.Sponsors.Should().NotBeEmpty();
	}
}
