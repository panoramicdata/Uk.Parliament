using System.Text;
using Microsoft.Extensions.Logging;


namespace Uk.Parliament.Test;

/// <summary>
/// ILogger implementation that writes to xUnit ITestOutputHelper
/// </summary>
public class XUnitLogger(ITestOutputHelper output, string categoryName, LogLevel minLevel = LogLevel.Debug) : ILogger
{
	private readonly Stack<object> _scopes = new();

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		_scopes.Push(state);
		return new ScopeDisposable(() => _scopes.Pop());
	}

	public bool IsEnabled(LogLevel logLevel) => logLevel >= minLevel;

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
	public void AddProvider(ILoggerProvider provider)
	{
		// Not needed for test logger
	}

	public ILogger CreateLogger(string categoryName) => new XUnitLogger(output, categoryName, minLevel);

	public void Dispose()
	{
		// Nothing to dispose
	}
}
