using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Uk.Parliament.Exceptions;
using Uk.Parliament.Interfaces;

namespace Uk.Parliament.Petitions;

/// <summary>
/// A UK Parliament PetitionsClient
/// </summary>
public class PetitionsClient
{
	private readonly HttpClient _httpClient;
	private static string BaseUrl => "https://petition.parliament.uk/";
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNameCaseInsensitive = true,
#if DEBUG
		// In debug mode, fail if there are unmapped properties to help ensure our model is complete
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
#else
		// In release mode, skip unmapped properties for forward compatibility
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
#endif
	};

	/// <summary>
	/// Constructor
	/// </summary>
	public PetitionsClient()
	{
		_httpClient = new HttpClient
		{
			BaseAddress = new Uri(BaseUrl)
		};
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	private static async Task<T> GetPayloadAsync<T>(HttpResponseMessage response) where T : class
	{
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpStatusResponseException(response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		var @object = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions);

		// Return the object
		return @object.Data;
	}

	/// <summary>
	/// Get many items of type T
	/// </summary>
	/// <typeparam name="T">The type</typeparam>
	/// <param name="id">The entity id</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	private async Task<Result<T>> GetSingleAsync<T>(int id, CancellationToken cancellationToken) where T : Resource, new()
	{
		try
		{
			var path = $"{new T { Id = id }.Endpoint}";

			var response = await _httpClient.GetAsync(path, cancellationToken).ConfigureAwait(false);
			var data = await GetPayloadAsync<T>(response);

			return new Result<T>(data);
		}
		catch (Exception ex)
		{
			return new Result<T>(ex);
		}
	}

	/// <summary>
	/// Get many items of type T
	/// </summary>
	/// <typeparam name="T">The type</typeparam>
	/// <param name="query">An optional query</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	private async Task<Result<List<T>>> GetManyAsync<T>(Query query, CancellationToken cancellationToken) where T : Resource, new()
	{
		try
		{
			var path = $"{new T().Endpoint}{query}";

			var response = await _httpClient.GetAsync(path, cancellationToken).ConfigureAwait(false);
			var data = await GetPayloadAsync<List<T>>(response);

			return new Result<List<T>>(data);
		}
		catch (Exception ex)
		{
			return new Result<List<T>>(ex);
		}
	}

	/// <summary>
	/// Get all petitions that match a query
	/// </summary>
	/// <param name="query">The query</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The matching petitions</returns>
	public Task<Result<List<Petition>>> GetPetitionsAsync(
		Query query,
		CancellationToken cancellationToken)
		=> GetManyAsync<Petition>(query, cancellationToken);

	/// <summary>
	/// Gets all petitions that match a query, automatically handling pagination to retrieve all results
	/// </summary>
	/// <param name="query">The query (PageNumber will be ignored as all pages are retrieved)</param>
	/// <param name="cancellationToken"></param>
	/// <returns>All matching petitions across all pages</returns>
	public async Task<Result<List<Petition>>> GetAllPetitionsAsync(
		Query query,
		CancellationToken cancellationToken)
	{
		var allPetitions = new List<Petition>();
		var pageNumber = 1;

		try
		{
			while (true)
			{
				// Create a query for this specific page
				var pageQuery = new Query
				{
					Text = query.Text,
					State = query.State,
					PageSize = query.PageSize ?? 50, // Use specified page size or default to 50
					PageNumber = pageNumber
				};

				var pageResult = await GetPetitionsAsync(pageQuery, cancellationToken);

				if (!pageResult.Ok)
				{
					return new Result<List<Petition>>(pageResult.Exception);
				}

				if (pageResult.Data == null || pageResult.Data.Count == 0)
				{
					// No more results
					break;
				}

				allPetitions.AddRange(pageResult.Data);

				// If we got fewer results than the page size, we've reached the last page
				if (pageResult.Data.Count < (query.PageSize ?? 50))
				{
					break;
				}

				pageNumber++;
			}

			return new Result<List<Petition>>(allPetitions);
		}
		catch (Exception ex)
		{
			return new Result<List<Petition>>(ex);
		}
	}

	/// <summary>
	/// Gets a single petition by its Id
	/// </summary>
	/// <param name="petitionId">The petition id</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The petition</returns>
	public Task<Result<Petition>> GetPetitionAsync(
		int petitionId,
		CancellationToken cancellationToken)
		=> GetSingleAsync<Petition>(petitionId, cancellationToken);
}