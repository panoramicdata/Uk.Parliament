using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Treaties;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for ITreatiesApi to provide additional functionality
/// </summary>
public static class TreatiesApiExtensions
{
	/// <summary>
	/// Get all treaties by automatically paginating through all results
	/// </summary>
	/// <param name="api">The treaties API</param>
	/// <param name="governmentOrganisationId">Filter by government organization</param>
	/// <param name="house">Filter by house</param>
	/// <param name="status">Filter by treaty status</param>
	/// <param name="dateLaidFrom">Filter by laid date from</param>
	/// <param name="dateLaidTo">Filter by laid date to</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all treaties</returns>
	public static async IAsyncEnumerable<Treaty> GetAllTreatiesAsync(
		this ITreatiesApi api,
		int? governmentOrganisationId = null,
		string? house = null,
		string? status = null,
		DateTime? dateLaidFrom = null,
		DateTime? dateLaidTo = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var skip = 0;

		while (true)
		{
			var response = await api.GetTreatiesAsync(
				governmentOrganisationId,
				house,
				status,
				dateLaidFrom,
				dateLaidTo,
				skip,
				pageSize,
				cancellationToken);

			if (response?.Items is null || response.Items.Count == 0)
			{
				yield break;
			}

			foreach (var item in response.Items)
			{
				yield return item.Value;
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
	/// Get all treaties as a materialized list
	/// </summary>
	/// <param name="api">The treaties API</param>
	/// <param name="governmentOrganisationId">Filter by government organization</param>
	/// <param name="house">Filter by house</param>
	/// <param name="status">Filter by treaty status</param>
	/// <param name="dateLaidFrom">Filter by laid date from</param>
	/// <param name="dateLaidTo">Filter by laid date to</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all treaties</returns>
	public static async Task<List<Treaty>> GetAllTreatiesListAsync(
		this ITreatiesApi api,
		int? governmentOrganisationId = null,
		string? house = null,
		string? status = null,
		DateTime? dateLaidFrom = null,
		DateTime? dateLaidTo = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allTreaties = new List<Treaty>();

		await foreach (var treaty in api.GetAllTreatiesAsync(
			governmentOrganisationId,
			house,
			status,
			dateLaidFrom,
			dateLaidTo,
			pageSize,
			cancellationToken))
		{
			allTreaties.Add(treaty);
		}

		return allTreaties;
	}
}
