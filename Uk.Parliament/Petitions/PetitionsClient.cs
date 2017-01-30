using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Uk.Parliament.Exceptions;
using Uk.Parliament.Interfaces;

namespace Uk.Parliament.Petitions
{
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
			_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		}

		private async Task<T> GetPayloadAsync<T>(HttpResponseMessage response) where T : class
		{
			if (!response.IsSuccessStatusCode)
			{
				throw new HttpStatusResponseException(response.StatusCode, await response.Content.ReadAsStringAsync());
			}

			var payload = await response.Content.ReadAsStringAsync();

			if (string.IsNullOrWhiteSpace(payload))
			{
				throw new MissingPayloadResponseException();
			}

			// Deserialize
			var @object = Deserialize<ApiResponse<T>>(payload);

			// Return the object
			return @object.Data;
		}

		private T Deserialize<T>(string payload) where T : class => JsonConvert.DeserializeObject<T>(payload);

		private string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);

		private HttpContent JsonPayload<T>(T obj) where T : class => new StringContent(Serialize(obj), Encoding.UTF8, "application/json");

		/// <summary>
		/// Get many items of type T
		/// </summary>
		/// <typeparam name="T">The type</typeparam>
		/// <param name="id">The entity id</param>
		/// <returns></returns>
		public async Task<Result<T>> GetSingleAsync<T>(int id) where T : Resource, new()
		{
			try
			{
				var path = $"{new T{Id=id}.Endpoint}";

				var response = await _httpClient.GetAsync(path).ConfigureAwait(false);
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
		public async Task<Result<List<T>>> GetManyAsync<T>(Query query) where T : Resource, new()
		{
			try
			{
				var path = $"{new T().Endpoint}{query}";

				var response = await _httpClient.GetAsync(path).ConfigureAwait(false);
				var data = await GetPayloadAsync<List<T>>(response);

				return new Result<List<T>>(data);
			}
			catch (Exception ex)
			{
				return new Result<List<T>>(ex);
			}
		}
	}
}