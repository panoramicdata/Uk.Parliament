namespace Uk.Parliament.Test;

/// <summary>
/// Base class for integration tests that provides common setup and teardown functionality
/// </summary>
public abstract class IntegrationTestBase : IDisposable
{
	/// <summary>Gets the cancellation token for the current test run.</summary>
	protected static CancellationToken CancellationToken => TestContext.Current.CancellationToken;

	/// <summary>The minimum number of streamed results a test expects by default.</summary>
	protected const int DefaultMinimumStreamedCount = 5;

	/// <summary>The number of items collected from a stream by default.</summary>
	protected const int DefaultMaxStreamedItems = 10;

	/// <summary>
	/// Initializes a new instance of the integration test base class
	/// </summary>
	protected IntegrationTestBase()
	{
		Client = new ParliamentClient(new ParliamentClientOptions
		{
			EnableDebugValidation = false
		});
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
	protected static void AssertValidStreamedResults<T>(List<T> items)
		=> AssertValidStreamedResults(items, DefaultMinimumStreamedCount);

	/// <summary>
	/// Common assertion helper for streamed results
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	/// <param name="items">The collected items from streaming</param>
	/// <param name="minimumCount">The minimum expected count</param>
	protected static void AssertValidStreamedResults<T>(List<T> items, int minimumCount)
	{
		_ = items.Should().NotBeEmpty();
		_ = items.Should().HaveCountGreaterThanOrEqualTo(minimumCount);
	}

	/// <summary>
	/// Helper method to collect a limited number of items from an async enumerable stream
	/// </summary>
	/// <typeparam name="T">The type of items being streamed</typeparam>
	/// <param name="stream">The async enumerable stream</param>
	/// <returns>A list of collected items</returns>
	protected static Task<List<T>> CollectStreamedItemsAsync<T>(IAsyncEnumerable<T> stream)
		=> CollectStreamedItemsAsync(stream, DefaultMaxStreamedItems);

	/// <summary>
	/// Helper method to collect a limited number of items from an async enumerable stream
	/// </summary>
	/// <typeparam name="T">The type of items being streamed</typeparam>
	/// <param name="stream">The async enumerable stream</param>
	/// <param name="maxItems">Maximum number of items to collect</param>
	/// <returns>A list of collected items</returns>
	protected static async Task<List<T>> CollectStreamedItemsAsync<T>(
		IAsyncEnumerable<T> stream,
		int maxItems)
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
