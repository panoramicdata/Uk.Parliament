using System.Text;
using Microsoft.Extensions.Logging;


namespace Uk.Parliament.Test;

/// <summary>
/// ILogger implementation that writes to xUnit ITestOutputHelper
/// </summary>
public class XUnitLogger(ITestOutputHelper output, string categoryName, LogLevel minLevel = LogLevel.Debug) : ILogger
{
	private readonly Stack<object> _scopes = new();

	/// <summary>Begins a logical operation scope for log entries.</summary>
	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		_scopes.Push(state);
		return new ScopeDisposable(() => _scopes.Pop());
	}

	/// <summary>Determines whether the given log level is enabled.</summary>
	public bool IsEnabled(LogLevel logLevel) => logLevel >= minLevel;

	/// <summary>Writes a log entry to the xUnit test output.</summary>
	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}

		var message = formatter(state, exception);
		var scopeInfo = GetScopeInformation();

		var logBuilder = new StringBuilder();
		logBuilder.Append($"[{logLevel}] [{categoryName}]");

		if (!string.IsNullOrEmpty(scopeInfo))
		{
			logBuilder.Append($" {scopeInfo}");
		}

		logBuilder.Append($" {message}");

		output.WriteLine(logBuilder.ToString());

		if (exception != null)
		{
			output.WriteLine($"Exception: {exception}");
		}
	}

	private string GetScopeInformation()
	{
		if (_scopes.Count == 0)
		{
			return string.Empty;
		}

		var scopeBuilder = new StringBuilder();

		foreach (var scope in _scopes)
		{
			if (scope is IDictionary<string, object> dict)
			{
				foreach (var kvp in dict)
				{
					if (scopeBuilder.Length > 0)
					{
						scopeBuilder.Append(' ');
					}

					scopeBuilder.Append($"[{kvp.Key}: {kvp.Value}]");
				}
			}
			else
			{
				if (scopeBuilder.Length > 0)
				{
					scopeBuilder.Append(' ');
				}

				scopeBuilder.Append($"[{scope}]");
			}
		}

		return scopeBuilder.ToString();
	}

	private class ScopeDisposable(Action onDispose) : IDisposable
	{
		public void Dispose() => onDispose();
	}
}

/// <summary>
/// Logger factory for xUnit tests
/// </summary>
public class XUnitLoggerFactory(ITestOutputHelper output, LogLevel minLevel = LogLevel.Debug) : ILoggerFactory
{
	/// <summary>Not implemented for the test logger; provided to satisfy <see cref="ILoggerFactory"/>.</summary>
	public void AddProvider(ILoggerProvider provider)
	{
		// Not needed for test logger
	}

	/// <summary>Creates an <see cref="XUnitLogger"/> for the specified category name.</summary>
	public ILogger CreateLogger(string categoryName) => new XUnitLogger(output, categoryName, minLevel);

	/// <summary>Disposes the logger factory (no-op for the test logger).</summary>
	public void Dispose()
	{
		// Nothing to dispose
	}
}
