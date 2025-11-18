using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uk.Parliament;

/// <summary>
/// HTTP message handler that logs requests and responses for diagnostics
/// </summary>
internal class LoggingHttpMessageHandler : DelegatingHandler
{
	private readonly ILogger? _logger;
	private readonly bool _verboseLogging;

	public LoggingHttpMessageHandler(HttpMessageHandler innerHandler, ILogger? logger, bool verboseLogging)
		: base(innerHandler)
	{
		_logger = logger;
		_verboseLogging = verboseLogging;
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		var requestId = Guid.NewGuid();
		var stopwatch = Stopwatch.StartNew();

		// Use scope to attach the request ID to all log entries
		using var scope = _logger?.BeginScope(new Dictionary<string, object>
		{
			["RequestId"] = requestId
		});

		if (_logger != null && _verboseLogging)
		{
			await LogRequestAsync(request);
		}

		HttpResponseMessage? response = null;
		try
		{
			response = await base.SendAsync(request, cancellationToken);

			if (_logger != null)
			{
				await LogResponseAsync(response, stopwatch.Elapsed);
			}

			return response;
		}
		catch (Exception ex)
		{
			_logger?.LogError(ex, "Request failed after {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
			throw;
		}
	}

	private async Task LogRequestAsync(HttpRequestMessage request)
	{
		var sb = new StringBuilder();
		sb.AppendLine("========== HTTP REQUEST ==========");
		sb.AppendLine($"{request.Method} {request.RequestUri}");
		sb.AppendLine("Headers:");

		foreach (var header in request.Headers)
		{
			sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
		}

		if (request.Content != null)
		{
			sb.AppendLine("Content Headers:");
			foreach (var header in request.Content.Headers)
			{
				sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
			}

			var content = await request.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(content))
			{
				_ = sb.AppendLine("Body:");
				_ = sb.AppendLine(content);
			}
		}

		_logger?.LogDebug("{RequestLog}", sb.ToString());
	}

	private async Task LogResponseAsync(HttpResponseMessage response, TimeSpan elapsed)
	{
		var sb = new StringBuilder();
		_ = sb.AppendLine($"========== HTTP RESPONSE ({elapsed.TotalMilliseconds:F0}ms) ==========")
			.AppendLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}")
			.AppendLine("Headers:");

		foreach (var header in response.Headers)
		{
			sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
		}

		// Always log response body for errors (4xx, 5xx), even if verbose logging is off
		var isError = !response.IsSuccessStatusCode;
		var shouldLogBody = _verboseLogging || isError;

		if (response.Content != null && shouldLogBody)
		{
			sb.AppendLine("Content Headers:");
			foreach (var header in response.Content.Headers)
			{
				sb.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
			}

			var content = await response.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(content))
			{
				sb.AppendLine($"Body ({content.Length} chars):");
				// For errors, always show full content (up to 10000 chars)
				// For success, truncate at 5000 chars if verbose logging
				var maxLength = isError ? 10000 : 5000;
				var displayContent = content.Length > maxLength ? content.Substring(0, maxLength) + "... (truncated)" : content;
				sb.AppendLine(displayContent);
			}
		}

		// Use Error level for 5xx, Warning for 4xx, Debug for 2xx/3xx
		var logLevel = (int)response.StatusCode >= 500 ? LogLevel.Error :
					   (int)response.StatusCode >= 400 ? LogLevel.Warning :
					   LogLevel.Debug;

		_logger?.Log(logLevel, "{ResponseLog}", sb.ToString());
	}
}
