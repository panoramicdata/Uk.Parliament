using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Uk.Parliament;

/// <summary>
/// HTTP message handler that logs requests and responses for diagnostics
/// </summary>
internal class LoggingHttpMessageHandler(HttpMessageHandler innerHandler, ILogger? logger, bool verboseLogging) : DelegatingHandler(innerHandler)
{
	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		var requestId = Guid.NewGuid();
		var stopwatch = Stopwatch.StartNew();

		// Use scope to attach the request ID to all log entries
		using var scope = logger?.BeginScope(new Dictionary<string, object>
		{
			["RequestId"] = requestId
		});

		if (logger != null && verboseLogging)
		{
			await LogRequestAsync(request);
		}

		HttpResponseMessage? response = null;
		try
		{
			response = await base.SendAsync(request, cancellationToken);

			if (logger != null)
			{
				await LogResponseAsync(response, stopwatch.Elapsed);
			}

			return response;
		}
		catch (Exception ex)
		{
			logger?.LogError(ex, "Request failed after {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
			throw;
		}
	}

	private async Task LogRequestAsync(HttpRequestMessage request)
	{
		var sb = new StringBuilder();
		_ = sb.AppendLine("========== HTTP REQUEST ==========");
		_ = sb.AppendLine($"{request.Method} {request.RequestUri}");
		_ = sb.AppendLine("Headers:");

		foreach (var header in request.Headers)
		{
			_ = sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
		}

		if (request.Content != null)
		{
			_ = sb.AppendLine("Content Headers:");
			foreach (var header in request.Content.Headers)
			{
				_ = sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
			}

			var content = await request.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(content))
			{
				_ = sb.AppendLine("Body:");
				_ = sb.AppendLine(content);
			}
		}

		logger?.LogDebug("{RequestLog}", sb.ToString());
	}

	private async Task LogResponseAsync(HttpResponseMessage response, TimeSpan elapsed)
	{
		var sb = new StringBuilder();
		_ = sb.AppendLine($"========== HTTP RESPONSE ({elapsed.TotalMilliseconds:F0}ms) ==========")
			.AppendLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}")
			.AppendLine("Headers:");

		AppendHeaders(sb, response.Headers);

		// Always log response body for errors (4xx, 5xx), even if verbose logging is off
		var isError = !response.IsSuccessStatusCode;
		var shouldLogBody = verboseLogging || isError;

		if (response.Content != null && shouldLogBody)
		{
			await AppendContentAsync(sb, response.Content, isError);
		}

		var logLevel = GetLogLevel(response.StatusCode);
		logger?.Log(logLevel, "{ResponseLog}", sb.ToString());
	}

	private static void AppendHeaders(StringBuilder sb, HttpHeaders headers)
	{
		foreach (var header in headers)
		{
			_ = sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
		}
	}

	private static async Task AppendContentAsync(StringBuilder sb, HttpContent content, bool isError)
	{
		_ = sb.AppendLine("Content Headers:");
		foreach (var header in content.Headers)
		{
			_ = sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
		}

		var contentString = await content.ReadAsStringAsync();
		if (!string.IsNullOrEmpty(contentString))
		{
			_ = sb.AppendLine($"Body ({contentString.Length} chars):");
			// For errors, always show full content (up to 10000 chars)
			// For success, truncate at 5000 chars if verbose logging
			var maxLength = isError ? 10000 : 5000;
			var displayContent = contentString.Length > maxLength ? contentString[..maxLength] + "... (truncated)" : contentString;
			_ = sb.AppendLine(displayContent);
		}
	}

	private static LogLevel GetLogLevel(HttpStatusCode statusCode)
	{
		// Use Error level for 5xx, Warning for 4xx, Debug for 2xx/3xx
		return (int)statusCode >= 500 ? LogLevel.Error :
			   (int)statusCode >= 400 ? LogLevel.Warning :
			   LogLevel.Debug;
	}
}
