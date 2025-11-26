using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Bills;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IBillsApi to provide additional functionality
/// </summary>
public static class BillsApiExtensions
{
	/// <summary>
	/// Get all bills by automatically paginating through all results
	/// </summary>
	/// <param name="api">The bills API</param>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="session">Optional session ID filter</param>
	/// <param name="currentHouse">Optional current house filter</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all bills</returns>
	public static async IAsyncEnumerable<Bill> GetAllBillsAsync(
		this IBillsApi api,
		string? searchTerm = null,
		int? session = null,
		string? currentHouse = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (true)
		{
			var response = await api.GetBillsAsync(searchTerm, session, currentHouse, skip, pageSize, cancellationToken);

			if (response?.Items is null || response.Items.Count == 0)
			{
				yield break;
			}

			foreach (var bill in response.Items)
			{
				yield return bill;
			}

			// Stop if this was the last page
			if (response.Items.Count < pageSize || skip + pageSize >= response.TotalResults)
			{
				yield break;
			}

			skip += pageSize;
		}
	}

	/// <summary>
	/// Get all bills as a materialized list
	/// </summary>
	/// <param name="api">The bills API</param>
	/// <param name="searchTerm">Optional search term</param>
	/// <param name="session">Optional session ID filter</param>
	/// <param name="currentHouse">Optional current house filter</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all bills</returns>
	public static async Task<List<Bill>> GetAllBillsListAsync(
		this IBillsApi api,
		string? searchTerm = null,
		int? session = null,
		string? currentHouse = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allBills = new List<Bill>();

		await foreach (var bill in api.GetAllBillsAsync(searchTerm, session, currentHouse, pageSize, cancellationToken))
		{
			allBills.Add(bill);
		}

		return allBills;
	}
}
