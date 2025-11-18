using System;
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

	public XUnitLogger(ITestOutputHelper output, string categoryName, LogLevel minLevel = LogLevel.Debug)
	{
		_output = output;
		_categoryName = categoryName;
		_minLevel = minLevel;
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

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
		_output.WriteLine($"[{logLevel}] [{_categoryName}] {message}");
		
		if (exception != null)
		{
			_output.WriteLine($"Exception: {exception}");
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
