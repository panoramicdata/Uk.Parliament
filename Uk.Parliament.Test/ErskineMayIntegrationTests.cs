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
	public async Task GetPartAsync_ForValidPartNumber_ReturnsPart()
	{
		// Arrange
		const int partNumber = 1;

		// Act
		var result = await _client.ErskineMay.GetPartAsync(partNumber);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.PartNumber.Should().Be(partNumber);
		_ = result.Title.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetChapterAsync_ForValidChapterNumber_ReturnsChapter()
	{
		// Arrange
		const int chapterNumber = 1;

		// Act
		var result = await _client.ErskineMay.GetChapterAsync(chapterNumber);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.ChapterNumber.Should().Be(chapterNumber);
		_ = result.Title.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetSectionByIdAsync_WithValidId_ReturnsSection()
	{
		// Arrange - First get a valid section ID from search results
		var searchResult = await _client.ErskineMay.SearchAsync("voting");
		
		if (searchResult?.SearchResults == null || searchResult.SearchResults.Count == 0)
		{
			// Skip test if no search results available
			return;
		}

		var validSectionId = searchResult.SearchResults[0].Id;

		// Act
		var result = await _client.ErskineMay.GetSectionByIdAsync(validSectionId);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(validSectionId);
		_ = result.Content.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task SearchAsync_WithSearchTerm_ReturnsResults()
	{
		// Arrange
		const string searchTerm = "voting";

		// Act
		var result = await _client.ErskineMay.SearchAsync(searchTerm);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.SearchResults.Should().NotBeEmpty();
	}

	[Fact]
	public async Task SearchAllAsync_StreamsResults()
	{
		// Arrange
		const string searchTerm = "voting";
		var results = new List<ErskineMaySearchResult>();

		// Act
		await foreach (var result in _client.ErskineMay.SearchAllAsync(searchTerm))
		{
			results.Add(result);
			if (results.Count >= 10)
			{
				break;
			}
		}

		// Assert
		_ = results.Should().NotBeEmpty();
	}

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
}
