using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Uk.Parliament.Extensions;

/// <summary>
/// Extension methods for registering UK Parliament API clients with dependency injection
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds the UK Parliament API client to the service collection
	/// </summary>
	/// <param name="services">The service collection</param>
	/// <param name="configure">Optional callback to configure client options</param>
	/// <returns>The service collection for chaining</returns>
	public static IServiceCollection AddUkParliamentClient(
		this IServiceCollection services,
		Action<ParliamentClientOptions>? configure = null)
	{
		// Register options
		if (configure is not null)
		{
			_ = services.Configure(configure);
		}
		else
		{
			_ = services.AddOptions<ParliamentClientOptions>();
		}

		// Register HttpClient for ParliamentClient
		_ = services.AddHttpClient<ParliamentClient>((serviceProvider, httpClient) =>
		{
			var options = serviceProvider.GetService<IOptions<ParliamentClientOptions>>()?.Value
				?? new ParliamentClientOptions();

			httpClient.Timeout = options.Timeout;
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));
		});

		// Register ParliamentClient as scoped
		_ = services.AddScoped<ParliamentClient>(serviceProvider =>
		{
			var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
			var httpClient = httpClientFactory.CreateClient(nameof(ParliamentClient));
			var options = serviceProvider.GetService<IOptions<ParliamentClientOptions>>()?.Value;

			return new ParliamentClient(httpClient, options);
		});

		return services;
	}

	/// <summary>
	/// Adds the UK Parliament API client with Polly resilience policies
	/// </summary>
	/// <param name="services">The service collection</param>
	/// <param name="configure">Optional callback to configure client options</param>
	/// <returns>The service collection for chaining</returns>
	public static IServiceCollection AddUkParliamentClientWithResilience(
		this IServiceCollection services,
		Action<ParliamentClientOptions>? configure = null)
	{
		_ = services.AddUkParliamentClient(configure);

		// Add Polly policies to the HttpClient
		_ = services.AddHttpClient<ParliamentClient>()
			.AddPolicyHandler(HttpPolicyExtensions
				.HandleTransientHttpError()
				.WaitAndRetryAsync(3, retryAttempt =>
					TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
			.AddPolicyHandler(HttpPolicyExtensions
				.HandleTransientHttpError()
				.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

		return services;
	}
}
