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

	[Fact]
	public async Task GetPartsAsync_ReturnsAllParts()
	{
		// Act
		var result = await _client.ErskineMay.GetPartsAsync();

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().NotBeEmpty();
		_ = result.Should().AllSatisfy(part =>
		{
			_ = part.PartNumber.Should().BePositive();
			_ = part.Title.Should().NotBeNullOrEmpty();
		});
	}

	[Fact]
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

	[Fact]
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

	[Fact]
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

	[Fact]
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

	[Fact]
	public async Task GetAllSectionsAsync_StreamsResults()
	{
		// Arrange
		var sections = new List<ErskineMaySection>();

		// Act
		await foreach (var section in _client.ErskineMay.GetAllSectionsAsync(chapterNumber: 1, pageSize: 5))
		{
			sections.Add(section);
			if (sections.Count >= 10)
			{
				break;
			}
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
