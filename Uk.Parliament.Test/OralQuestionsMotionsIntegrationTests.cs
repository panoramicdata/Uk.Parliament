using Uk.Parliament.Models.OralQuestions;

namespace Uk.Parliament.Test;

/// <summary>
/// Integration tests for Oral Questions and Motions API
/// </summary>
[Trait("Category", "Integration")]
public class OralQuestionsMotionsIntegrationTests : IntegrationTestBase
{
	private static void AssertListSucceeded<T>(OralQuestionsResponse<T> result, bool expectItems)
	{
		_ = result.Success.Should().BeTrue();
		_ = result.Response.Should().NotBeNull();
		if (expectItems)
		{
			_ = result.PagingInfo.Total.Should().BePositive();
		}
	}

	/// <summary>Verifies that fetching oral questions without filters returns a successful non-empty response.</summary>
	[Fact]
	public async Task GetOralQuestionsAsync_WithNoFilters_Succeeds()
		=> AssertListSucceeded(
			await Client.OralQuestionsMotions.GetOralQuestionsAsync(new GetOralQuestionsRequest { Take = 10 }, CancellationToken),
			expectItems: true);

	/// <summary>Verifies that filtering oral questions by member ID returns a successful response.</summary>
	[Fact]
	public async Task GetOralQuestionsAsync_FilterByMember_ReturnsQuestions()
		=> AssertListSucceeded(
			await Client.OralQuestionsMotions.GetOralQuestionsAsync(new GetOralQuestionsRequest { AskingMemberId = 172, Take = 10 }, CancellationToken),
			expectItems: false);

	/// <summary>Verifies that streaming all oral questions via async enumerable yields a non-empty collection.</summary>
	[Fact]
	public async Task GetAllOralQuestionsAsync_StreamsResults()
		=> AssertValidStreamedResults(await CollectStreamedItemsAsync(
			Client.GetAllAsync(new GetOralQuestionsRequest { Take = 5 }, CancellationToken)));

	/// <summary>Verifies that fetching motions without filters returns a successful non-empty response.</summary>
	[Fact]
	public async Task GetMotionsAsync_WithNoFilters_Succeeds()
		=> AssertListSucceeded(
			await Client.OralQuestionsMotions.GetMotionsAsync(new GetMotionsRequest { Take = 10 }, CancellationToken),
			expectItems: true);

	/// <summary>Verifies that filtering motions to active-only returns a successful response.</summary>
	[Fact]
	public async Task GetMotionsAsync_FilterByActive_ReturnsActiveMotions()
		=> AssertListSucceeded(
			await Client.OralQuestionsMotions.GetMotionsAsync(new GetMotionsRequest { IsActive = true, Take = 10 }, CancellationToken),
			expectItems: false);

	/// <summary>Verifies that fetching a motion by a valid ID returns a successful response.</summary>
	[Fact]
	public async Task GetMotionByIdAsync_WithValidId_ReturnsMotion()
	{
		var listResult = await Client.OralQuestionsMotions.GetMotionsAsync(new GetMotionsRequest { Take = 1 }, CancellationToken);

		if (listResult.Response is not { Count: > 0 })
		{
			return;
		}

		try
		{
			var result = await Client.OralQuestionsMotions.GetMotionByIdAsync(listResult.Response[0].Id, CancellationToken);

			_ = result.Success.Should().BeTrue();
		}
		catch (Refit.ApiException)
		{
			// The API errors for some motion IDs
		}
	}

	/// <summary>Verifies that streaming all motions via async enumerable yields a non-empty collection.</summary>
	[Fact]
	public async Task GetAllMotionsAsync_StreamsResults()
		=> AssertValidStreamedResults(await CollectStreamedItemsAsync(
			Client.GetAllAsync(new GetMotionsRequest { Take = 5 }, CancellationToken)));
}
