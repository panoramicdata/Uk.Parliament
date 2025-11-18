using System;
using Xunit;
using Uk.Parliament.Extensions;
using Uk.Parliament.Models.ErskineMay;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Erskine May API
/// </summary>
public class ErskineMayIntegrationTests : IDisposable
{
	private readonly ParliamentClient _client;

	public ErskineMayIntegrationTests()
	{
		_client = new ParliamentClient();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetPartsAsync_ReturnsAllParts()
	{
		// Act
		var result = await _client.ErskineMay.GetPartsAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().NotBeEmpty();
		_ = result.Should().AllSatisfy(part =>
		{
			_ = part.PartNumber.Should().BeGreaterThan(0);
			_ = part.Title.Should().NotBeNullOrEmpty();
		});
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetChaptersAsync_ForValidPart_ReturnsChapters()
	{
		// Arrange
		const int partNumber = 1;

		// Act
		var result = await _client.ErskineMay.GetChaptersAsync(partNumber);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().NotBeEmpty();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetSectionsAsync_ForValidChapter_ReturnsSections()
	{
		// Arrange
		const int chapterNumber = 1;

		// Act
		var result = await _client.ErskineMay.GetSectionsAsync(chapterNumber, skip: 0, take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetSectionByIdAsync_WithValidId_ReturnsSection()
	{
		// Arrange
		const int sectionId = 1;

		// Act
		var result = await _client.ErskineMay.GetSectionByIdAsync(sectionId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(sectionId);
		_ = result.Content.Should().NotBeNullOrEmpty();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task SearchAsync_WithSearchTerm_ReturnsResults()
	{
		// Arrange
		const string searchTerm = "voting";

		// Act
		var result = await _client.ErskineMay.SearchAsync(searchTerm, skip: 0, take: 10);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
	}

	[Fact(Skip = "Integration test - requires live API")]
	public async Task GetAllSectionsAsync_StreamsResults()
	{
		// Arrange
		var sections = new List<ErskineMaySection>();

		// Act
		await foreach (var section in _client.ErskineMay.GetAllSectionsAsync(chapterNumber: 1, pageSize: 5))
		{
			sections.Add(section);
			if (sections.Count >= 10) break;
		}

		// Assert
		_ = sections.Should().NotBeEmpty();
	}

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
}

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
		var expectedResponse = new PaginatedResponse<ErskineMaySearchResult>
		{
			TotalResults = 1,
			Items =
			[
				new ValueWrapper<ErskineMaySearchResult>
				{
					Value = new ErskineMaySearchResult
					{
						Id = 1,
						SectionNumber = "1.1",
						Title = "Test Section",
						Excerpt = "This is a test excerpt",
						Score = 0.95
					}
				}
			]
		};

		_ = mockApi.Setup(x => x.SearchAsync(
			It.IsAny<string>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse);

		// Act
		var result = await mockApi.Object.SearchAsync("test");

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.TotalResults.Should().Be(1);
		_ = result.Items[0].Value.Score.Should().Be(0.95);
	}
}
