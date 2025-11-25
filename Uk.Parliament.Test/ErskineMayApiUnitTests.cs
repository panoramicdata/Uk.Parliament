using Uk.Parliament.Models.ErskineMay;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Erskine May API (mocking)
/// </summary>
public class ErskineMayApiUnitTests
{
	[Fact]
	public void ErskineMayApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<IErskineMayApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public async Task GetPartsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IErskineMayApi>();
		var expectedParts = new List<ErskineMayPart>
		{
			new() { PartNumber = 1, Title = "Parliamentary Procedure", ChapterCount = 10 },
			new() { PartNumber = 2, Title = "The House of Commons", ChapterCount = 15 }
		};

		_ = mockApi.Setup(x => x.GetPartsAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedParts);

		// Act
		var result = await mockApi.Object.GetPartsAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().HaveCount(2);
		_ = result[0].Title.Should().Be("Parliamentary Procedure");
	}

	[Fact]
	public async Task SearchAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IErskineMayApi>();
		var expectedResponse = new List<ErskineMaySearchResult>
		{
			new()
			{
				Id = 1,
				SectionNumber = "1.1",
				Title = "Test Section",
				Excerpt = "This is a test excerpt",
				Score = 0.95
			}
		};

		_ = mockApi.Setup(x => x.SearchAsync(
			It.IsAny<string>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.SearchAsync("test");

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().ContainSingle();
		_ = result[0].Score.Should().Be(0.95);
	}
}
