using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Uk.Parliament.Test;

/// <summary>
/// ILogger implementation that writes to xUnit ITestOutputHelper
/// </summary>
public class XUnitLogger : ILogger
{
	private readonly ITestOutputHelper _output;
	private readonly string _categoryName;
	private readonly LogLevel _minLevel;
	private readonly Stack<object> _scopes = new();

	public XUnitLogger(ITestOutputHelper output, string categoryName, LogLevel minLevel = LogLevel.Debug)
	{
		_output = output;
		_categoryName = categoryName;
		_minLevel = minLevel;
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		_scopes.Push(state);
		return new ScopeDisposable(() => _scopes.Pop());
	}

	public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;

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
		logBuilder.Append($"[{logLevel}] [{_categoryName}]");
		
		if (!string.IsNullOrEmpty(scopeInfo))
		{
			logBuilder.Append($" {scopeInfo}");
		}
		
		logBuilder.Append($" {message}");
		
		_output.WriteLine(logBuilder.ToString());
		
		if (exception != null)
		{
			_output.WriteLine($"Exception: {exception}");
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

	private class ScopeDisposable : IDisposable
	{
		private readonly Action _onDispose;

		public ScopeDisposable(Action onDispose)
		{
			_onDispose = onDispose;
		}

		public void Dispose()
		{
			_onDispose();
		}
	}
}

/// <summary>
/// Logger factory for xUnit tests
/// </summary>
public class XUnitLoggerFactory : ILoggerFactory
{
	private readonly ITestOutputHelper _output;
	private readonly LogLevel _minLevel;

	public XUnitLoggerFactory(ITestOutputHelper output, LogLevel minLevel = LogLevel.Debug)
	{
		_output = output;
		_minLevel = minLevel;
	}

	public void AddProvider(ILoggerProvider provider)
	{
		// Not needed for test logger
	}

	public ILogger CreateLogger(string categoryName)
	{
		return new XUnitLogger(_output, categoryName, _minLevel);
	}

	public void Dispose()
	{
		// Nothing to dispose
	}
}
