using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
	private string BaseUrl => "https://petition.parliament.uk/";

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

		var @object = await response.Content.ReadFromJsonAsync<Result<T>>();

		// Return the object
		return @object.Data;
	}

	/// <summary>
	/// Get many items of type T
	/// </summary>
	/// <typeparam name="T">The type</typeparam>
	/// <param name="id">The entity id</param>
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
	/// <returns>The matching petitions</returns>
	public Task<Result<List<Petition>>> GetPetitionsAsync(
		Query query,
		CancellationToken cancellationToken)
		=> GetManyAsync<Petition>(query, cancellationToken);

	/// <summary>
	/// Gets a single petition by its Id
	/// </summary>
	/// <param name="petitionId">The petition id</param>
	/// <returns>The petition</returns>
	public Task<Result<Petition>> GetPetitionAsync(
		int petitionId,
		CancellationToken cancellationToken)
		=> GetSingleAsync<Petition>(petitionId, cancellationToken);
}