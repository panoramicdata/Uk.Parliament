using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

		if (_logger != null && _verboseLogging)
		{
			await LogRequestAsync(request, requestId);
		}

		HttpResponseMessage? response = null;
		try
		{
			response = await base.SendAsync(request, cancellationToken);
			
			if (_logger != null)
			{
				await LogResponseAsync(response, requestId, stopwatch.Elapsed);
			}

			return response;
		}
		catch (Exception ex)
		{
			_logger?.LogError(ex, "[{RequestId}] Request failed after {Elapsed}ms", requestId, stopwatch.ElapsedMilliseconds);
			throw;
		}
	}

	private async Task LogRequestAsync(HttpRequestMessage request, Guid requestId)
	{
		var sb = new StringBuilder();
		sb.AppendLine($"[{requestId}] ========== HTTP REQUEST ==========");
		sb.AppendLine($"[{requestId}] {request.Method} {request.RequestUri}");
		sb.AppendLine($"[{requestId}] Headers:");
		
		foreach (var header in request.Headers)
		{
			sb.AppendLine($"[{requestId}]   {header.Key}: {string.Join(", ", header.Value)}");
		}

		if (request.Content != null)
		{
			sb.AppendLine($"[{requestId}] Content Headers:");
			foreach (var header in request.Content.Headers)
			{
				sb.AppendLine($"[{requestId}]   {header.Key}: {string.Join(", ", header.Value)}");
			}

			var content = await request.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(content))
			{
				sb.AppendLine($"[{requestId}] Body:");
				sb.AppendLine($"[{requestId}] {content}");
			}
		}

		_logger?.LogDebug("{RequestLog}", sb.ToString());
	}

	private async Task LogResponseAsync(HttpResponseMessage response, Guid requestId, TimeSpan elapsed)
	{
		var sb = new StringBuilder();
		sb.AppendLine($"[{requestId}] ========== HTTP RESPONSE ({elapsed.TotalMilliseconds:F0}ms) ==========");
		sb.AppendLine($"[{requestId}] Status: {(int)response.StatusCode} {response.ReasonPhrase}");
		sb.AppendLine($"[{requestId}] Headers:");
		
		foreach (var header in response.Headers)
		{
			sb.AppendLine($"[{requestId}]   {header.Key}: {string.Join(", ", header.Value)}");
		}

		if (response.Content != null && _verboseLogging)
		{
			sb.AppendLine($"[{requestId}] Content Headers:");
			foreach (var header in response.Content.Headers)
			{
				sb.AppendLine($"[{requestId}]   {header.Key}: {string.Join(", ", header.Value)}");
			}

			var content = await response.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(content))
			{
				sb.AppendLine($"[{requestId}] Body ({content.Length} chars):");
				// Truncate very long responses for readability
				var displayContent = content.Length > 5000 ? content.Substring(0, 5000) + "... (truncated)" : content;
				sb.AppendLine($"[{requestId}] {displayContent}");
			}
		}

		var logLevel = response.IsSuccessStatusCode ? LogLevel.Debug : LogLevel.Warning;
		_logger?.Log(logLevel, "{ResponseLog}", sb.ToString());
	}
}
