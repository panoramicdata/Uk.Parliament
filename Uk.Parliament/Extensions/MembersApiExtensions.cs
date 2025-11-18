using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Interfaces;
using Uk.Parliament.Models;
using Uk.Parliament.Models.Members;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for IMembersApi to provide additional functionality
/// </summary>
public static class MembersApiExtensions
{
	/// <summary>
	/// Get all members by automatically paginating through all results
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="name">Optional name to search for</param>
	/// <param name="house">Optional house filter (1 = Commons, 2 = Lords)</param>
	/// <param name="isCurrentMember">Optional filter for current members only</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all members</returns>
	public static async IAsyncEnumerable<Member> GetAllAsync(
		this IMembersApi api,
		string? name = null,
		int? house = null,
		bool? isCurrentMember = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		int skip = 0;

		while (true)
		{
			var response = await api.SearchAsync(name, skip, pageSize, house, isCurrentMember, cancellationToken);

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
	/// Get all members as a materialized list
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="name">Optional name to search for</param>
	/// <param name="house">Optional house filter (1 = Commons, 2 = Lords)</param>
	/// <param name="isCurrentMember">Optional filter for current members only</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of all members</returns>
	public static async Task<List<Member>> GetAllListAsync(
		this IMembersApi api,
		string? name = null,
		int? house = null,
		bool? isCurrentMember = null,
		int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		var allMembers = new List<Member>();

		await foreach (var member in api.GetAllAsync(name, house, isCurrentMember, pageSize, cancellationToken))
		{
			allMembers.Add(member);
		}

		return allMembers;
	}

	/// <summary>
	/// Get all constituencies by automatically paginating through all results
	/// </summary>
	/// <param name="api">The members API</param>
	/// <param name="searchText">Optional search text</param>
	/// <param name="pageSize">Items per page (default: 20)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Async enumerable of all constituencies</returns>
	public static async IAsyncEnumerable<Constituency> GetAllConstituenciesAsync(
		this IMembersApi api,
		string? searchText = null,
		int pageSize = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		int skip = 0;

		while (true)
		{
			var response = await api.SearchConstituenciesAsync(searchText, skip, pageSize, cancellationToken);

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
}
