using Uk.Parliament.Models.Now;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for NOW API (mocking)
/// </summary>
public class NowApiUnitTests
{
	[Fact]
	public void NowApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<INowApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public async Task GetCommonsStatusAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<INowApi>();
		var expectedStatus = new ChamberStatus
		{
			House = "Commons",
			IsSitting = true,
			SessionDate = DateTime.Today,
			CurrentBusiness = "Prime Minister's Questions",
			IsInRecess = false
		};

		_ = mockApi.Setup(x => x.GetCommonsStatusAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedStatus);

		// Act
		var result = await mockApi.Object.GetCommonsStatusAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.House.Should().Be("Commons");
		_ = result.IsSitting.Should().BeTrue();
		_ = result.CurrentBusiness.Should().Be("Prime Minister's Questions");
	}

	[Fact]
	public async Task GetUpcomingBusinessAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<INowApi>();
		var expectedBusiness = new List<BusinessItem>
		{
			new()
			{
				Id = 1,
				House = "Commons",
				Description = "Question Time",
				BusinessType = "Questions",
				OrderNumber = 1,
				IsActive = false
			},
			new()
			{
				Id = 2,
				House = "Commons",
				Description = "Ten Minute Rule Motion",
				BusinessType = "Motion",
				OrderNumber = 2,
				IsActive = false
			}
		};

		_ = mockApi.Setup(x => x.GetUpcomingBusinessAsync("Commons", It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedBusiness);

		// Act
		var result = await mockApi.Object.GetUpcomingBusinessAsync("Commons");

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().HaveCount(2);
		_ = result[0].Description.Should().Be("Question Time");
	}

	[Fact]
	public async Task GetCurrentBusinessAsync_WithMock_ReturnsCurrentItem()
	{
		// Arrange
		var mockApi = new Mock<INowApi>();
		var expectedItem = new BusinessItem
		{
			Id = 1,
			House = "Lords",
			Description = "Debate on Second Reading",
			BusinessType = "Debate",
			OrderNumber = 3,
			IsActive = true
		};

		_ = mockApi.Setup(x => x.GetCurrentBusinessAsync("Lords", It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedItem);

		// Act
		var result = await mockApi.Object.GetCurrentBusinessAsync("Lords");

		// Assert
		_ = result.Should().NotBeNull();
		_ = result!.IsActive.Should().BeTrue();
		_ = result.Description.Should().Be("Debate on Second Reading");
	}
}
