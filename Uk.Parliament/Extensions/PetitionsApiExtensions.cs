using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IPetitionsApi to provide additional functionality
/// </summary>
public static class PetitionsApiExtensions
{
	/// <summary>
	/// Get all petitions by automatically paginating through all results
	/// </summary>
	/// <param name="api">The petitions API</param>
	/// <param name="search">Optional search term</param>
	/// <param name="state">Optional state filter</param>
	/// <param name="pageSize">Items per page (default: 50)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all petitions</returns>
	public static async IAsyncEnumerable<Petition> GetAllAsync(
		this IPetitionsApi api,
		string? search = null,
		string? state = null,
		int pageSize = 50,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		int page = 1;

		while (true)
		{
			var response = await api.GetAsync(search, state, page, pageSize, cancellationToken);

			if (response?.Data is null || response.Data.Count == 0)
			{
				yield break;
			}

			foreach (var petition in response.Data)
			{
				yield return petition;
			}

			// Stop if this was the last page
			if (response.Data.Count < pageSize)
			{
				yield break;
			}

			page++;
		}
	}

	/// <summary>
	/// Get all petitions as a materialized list
	/// </summary>
	/// <param name="api">The petitions API</param>
	/// <param name="search">Optional search term</param>
	/// <param name="state">Optional state filter</param>
	/// <param name="pageSize">Items per page (default: 50)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all petitions</returns>
	public static async Task<List<Petition>> GetAllListAsync(
		this IPetitionsApi api,
		string? search = null,
		string? state = null,
		int pageSize = 50,
		CancellationToken cancellationToken = default)
	{
		var allPetitions = new List<Petition>();

		await foreach (var petition in api.GetAllAsync(search, state, pageSize, cancellationToken))
		{
			allPetitions.Add(petition);
		}

		return allPetitions;
	}
}
