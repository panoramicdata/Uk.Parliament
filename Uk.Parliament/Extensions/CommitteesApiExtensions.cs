using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models.Committees;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for ICommitteesApi to provide additional functionality
/// </summary>
public static class CommitteesApiExtensions
{
	/// <summary>
	/// Get all committees by automatically paginating through all results
	/// </summary>
	/// <param name="api">The committees API</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all committees</returns>
	public static async IAsyncEnumerable<Committee> GetAllCommitteesAsync(
		this ICommitteesApi api,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		int skip = 0;

		while (true)
		{
			var response = await api.GetCommitteesAsync(skip, pageSize, cancellationToken);

			if (response?.Items is null || response.Items.Count == 0)
			{
				yield break;
			}

			foreach (var committee in response.Items)
			{
				yield return committee;
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
	/// Get all committees as a materialized list
	/// </summary>
	/// <param name="api">The committees API</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all committees</returns>
	public static async Task<List<Committee>> GetAllCommitteesListAsync(
		this ICommitteesApi api,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allCommittees = new List<Committee>();

		await foreach (var committee in api.GetAllCommitteesAsync(pageSize, cancellationToken))
		{
			allCommittees.Add(committee);
		}

		return allCommittees;
	}
}
