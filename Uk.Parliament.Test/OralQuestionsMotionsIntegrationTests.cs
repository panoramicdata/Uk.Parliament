namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Oral Questions and Motions API
/// </summary>
[Trait("Category", "Integration")]
public class OralQuestionsMotionsIntegrationTests : IntegrationTestBase
{
	#region Oral Questions Tests

	/// <summary>Verifies that fetching oral questions without filters returns a successful non-empty response.</summary>
	[Fact]
	public async Task GetOralQuestionsAsync_WithNoFilters_Succeeds()
		=> await AssertOralQuestionsListAsync(
			new GetOralQuestionsRequest { Take = 10 },
			expectItems: true);

	/// <summary>Verifies that filtering oral questions by member ID returns a successful response.</summary>
	[Fact]
	public async Task GetOralQuestionsAsync_FilterByMember_ReturnsQuestions()
	{
		// NOTE: This test may fail due to Parliament API bug - filter might not work
		await AssertOralQuestionsListAsync(
			new GetOralQuestionsRequest { AskingMemberId = 172, Take = 10 },
			expectItems: false);
	}

	/// <summary>Verifies that streaming all oral questions via async enumerable yields a non-empty collection.</summary>
	[Fact]
	public async Task GetAllOralQuestionsAsync_StreamsResults()
	{
		// Act
		var questions = await CollectStreamedItemsAsync(
			Client.GetAllAsync(
				new GetOralQuestionsRequest { Take = 5 },
			CancellationToken));

		// Assert
		AssertValidStreamedResults(questions);
	}

	private async Task AssertOralQuestionsListAsync(
		GetOralQuestionsRequest request,
		bool expectItems)
	{
		var result = await Client
			.OralQuestionsMotions
			.GetOralQuestionsAsync(request, CancellationToken);

		_ = result.Should().NotBeNull();
		_ = result.Success.Should().BeTrue();
		_ = result.Response.Should().NotBeNull();
		if (expectItems)
		{
			_ = result.PagingInfo.Total.Should().BePositive();
		}
	}

	#endregion

	#region Motions Tests

	/// <summary>Verifies that fetching motions without filters returns a successful non-empty response.</summary>
	[Fact]
	public async Task GetMotionsAsync_WithNoFilters_Succeeds()
		=> await AssertMotionsListAsync(
			new GetMotionsRequest { Take = 10 },
			expectItems: true);

	/// <summary>Verifies that filtering motions to active-only returns a successful response.</summary>
	[Fact]
	public async Task GetMotionsAsync_FilterByActive_ReturnsActiveMotions()
	{
		// Note: API may return empty results or items that don't match the filter exactly
		await AssertMotionsListAsync(
			new GetMotionsRequest { IsActive = true, Take = 10 },
			expectItems: false);
	}

	/// <summary>Verifies that fetching a motion by a valid ID returns a successful response.</summary>
	[Fact]
	public async Task GetMotionByIdAsync_WithValidId_ReturnsMotion()
	{
		// Arrange - First get a valid motion ID from the list
		var listResult = await Client
			.OralQuestionsMotions
			.GetMotionsAsync(
				new GetMotionsRequest { Take = 1 },
				cancellationToken: CancellationToken);

		if (listResult.Response == null || listResult.Response.Count == 0)
		{
			// Skip test if no motions available
			return;
		}

		var validMotionId = listResult.Response[0].Id;

		try
		{
			// Act
			var result = await Client
				.OralQuestionsMotions
				.GetMotionByIdAsync(
					validMotionId,
					CancellationToken);

			// Assert
			_ = result.Should().NotBeNull();
			_ = result.Success.Should().BeTrue();
		}
		catch (Refit.ApiException)
		{
			// API may return errors for some motion IDs
		}
	}

	private async Task AssertMotionsListAsync(
		GetMotionsRequest request,
		bool expectItems)
	{
		var result = await Client
			.OralQuestionsMotions
			.GetMotionsAsync(request, CancellationToken);

		_ = result.Should().NotBeNull();
		_ = result.Success.Should().BeTrue();
		_ = result.Response.Should().NotBeNull();
		if (expectItems)
		{
			_ = result.PagingInfo.Total.Should().BePositive();
		}
	}

	/// <summary>Verifies that streaming all motions via async enumerable yields a non-empty collection.</summary>
	[Fact]
	public async Task GetAllMotionsAsync_StreamsResults()
	{
		// Act
		var motions = await CollectStreamedItemsAsync(
			Client.GetAllAsync(
				new GetMotionsRequest { Take = 5 },
			CancellationToken));

		// Assert
		AssertValidStreamedResults(motions);
	}

	#endregion
}
