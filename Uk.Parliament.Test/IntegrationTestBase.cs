namespace Uk.Parliament.Test;

/// <summary>
/// Base class for integration tests that provides common setup and teardown functionality
/// </summary>
public abstract class IntegrationTestBase : IDisposable
{
	/// <summary>
	/// Initializes a new instance of the integration test base class
	/// </summary>
	protected IntegrationTestBase()
	{
		Client = new ParliamentClient();
	}

	/// <summary>
	/// The Parliament client instance for making API calls
	/// </summary>
	protected ParliamentClient Client { get; }

	/// <summary>
	/// Disposes the client resources
	/// </summary>
	public void Dispose()
	{
		Client.Dispose();
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Common assertion helper for paginated responses
	/// </summary>
	/// <typeparam name="T">The type of items in the response</typeparam>
	/// <param name="result">The paginated response to validate</param>
	protected static void AssertValidPaginatedResponse<T>(PaginatedResponse<T> result)
	{
		_ = result.Should().NotBeNull();
		_ = result.Items.Should().NotBeNull();
		_ = result.TotalResults.Should().BePositive();
	}

	/// <summary>
	/// Common assertion helper for paginated responses with specific assertions on items
	/// </summary>
	/// <typeparam name="T">The type of items in the response</typeparam>
	/// <param name="result">The paginated response to validate</param>
	/// <param name="itemAssertion">Action to perform assertions on each item</param>
	protected static void AssertValidPaginatedResponse<T>(
		PaginatedResponse<T> result,
		Action<ValueWrapper<T>> itemAssertion)
	{
		AssertValidPaginatedResponse(result);
		_ = result.Items.Should().AllSatisfy(itemAssertion);
	}

	/// <summary>
	/// Common assertion helper for streamed results
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	/// <param name="items">The collected items from streaming</param>
	/// <param name="minimumCount">The minimum expected count (default is 5)</param>
	protected static void AssertValidStreamedResults<T>(List<T> items, int minimumCount = 5)
	{
		_ = items.Should().NotBeEmpty();
		_ = items.Should().HaveCountGreaterThanOrEqualTo(minimumCount);
	}

	/// <summary>
	/// Helper method to collect a limited number of items from an async enumerable stream
	/// </summary>
	/// <typeparam name="T">The type of items being streamed</typeparam>
	/// <param name="stream">The async enumerable stream</param>
	/// <param name="maxItems">Maximum number of items to collect (default is 10)</param>
	/// <returns>A list of collected items</returns>
	protected static async Task<List<T>> CollectStreamedItemsAsync<T>(
		IAsyncEnumerable<T> stream,
		int maxItems = 10)
	{
		var items = new List<T>();
		await foreach (var item in stream)
		{
			items.Add(item);
			if (items.Count >= maxItems)
			{
				break;
			}
		}

		return items;
	}
}
