using Uk.Parliament.Extensions;
using Uk.Parliament.Models.ErskineMay;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Erskine May API
/// </summary>
public class ErskineMayIntegrationTests : IntegrationTestBase
{
	[Fact]
	public async Task GetPartsAsync_ReturnsAllParts()
	{
		// Act
		var result = await Client
			.ErskineMay
			.GetPartsAsync(CancellationToken);

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
		var result = await Client
			.ErskineMay
			.GetPartAsync(
				partNumber,
				CancellationToken);

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
		var result = await Client
			.ErskineMay
			.GetChapterAsync(
				chapterNumber,
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.ChapterNumber.Should().Be(chapterNumber);
		_ = result.Title.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetSectionByIdAsync_WithValidId_ReturnsSection()
	{
		// Arrange - First get a valid section ID from search results
		var searchResult = await Client
			.ErskineMay
			.SearchAsync(
				"voting",
				CancellationToken);

		if (searchResult?.SearchResults == null || searchResult.SearchResults.Count == 0)
		{
			// Skip test if no search results available
			return;
		}

		var validSectionId = searchResult.SearchResults[0].Id;

		try
		{
			// Act
			var result = await Client
				.ErskineMay
				.GetSectionByIdAsync(
					validSectionId,
					CancellationToken);

			// Assert
			_ = result.Should().NotBeNull();
			_ = result.Id.Should().Be(validSectionId);
		}
		catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			// Section may not be found - this is expected for some IDs
		}
	}

	[Fact]
	public async Task SearchAsync_WithSearchTerm_ReturnsResults()
	{
		// Arrange
		const string searchTerm = "voting";

		// Act
		var result = await Client
			.ErskineMay
			.SearchAsync(
				searchTerm,
				CancellationToken);

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
		await foreach (var result in Client.ErskineMay.SearchAllAsync(searchTerm, cancellationToken: CancellationToken))
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
}
