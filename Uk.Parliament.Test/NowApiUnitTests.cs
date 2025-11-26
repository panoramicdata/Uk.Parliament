using Uk.Parliament.Models.Now;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for NOW API (mocking)
/// </summary>
public class NowApiUnitTests : IntegrationTestBase
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
	public async Task GetCurrentMessageAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<INowApi>();
		var expectedMessage = new AnnunciatorMessage
		{
			Id = 123,
			AnnunciatorType = "CommonsMain",
			PublishTime = DateTime.Now,
			ShowCommonsBell = true,
			ShowLordsBell = false,
			Slides =
			[
				new AnnunciatorSlide
				{
					Id = 1,
					Lines =
					[
						new SlideLine
						{
							DisplayOrder = 1,
							ContentType = "Generic",
							Style = "Text100",
							Content = "Test slide content",
							HorizontalAlignment = "Centre"
						}
					],
					Type = "Generic",
					CarouselOrder = 1
				}
			]
		};

		_ = mockApi.Setup(x => x.GetCurrentMessageAsync("commons", It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedMessage);

		// Act
		var result = await mockApi.Object.GetCurrentMessageAsync("commons",
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(123);
		_ = result.Slides.Should().ContainSingle();
		_ = result.ShowCommonsBell.Should().BeTrue();
	}

	[Fact]
	public async Task GetMessageByDateAsync_WithMock_ReturnsMessage()
	{
		// Arrange
		var mockApi = new Mock<INowApi>();
		var expectedMessage = new AnnunciatorMessage
		{
			Id = 456,
			AnnunciatorType = "LordsMain",
			PublishTime = DateTime.Now.AddDays(-1),
			ShowCommonsBell = false,
			ShowLordsBell = true,
			Slides = []
		};

		_ = mockApi.Setup(x => x.GetMessageByDateAsync("lords", "2024-01-01", It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedMessage);

		// Act
		var result = await mockApi.Object.GetMessageByDateAsync(
			"lords",
			"2024-01-01",
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(456);
		_ = result.ShowLordsBell.Should().BeTrue();
	}
}
