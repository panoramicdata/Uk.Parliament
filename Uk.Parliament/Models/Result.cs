namespace Uk.Parliament.Models;

/// <summary>
/// An API response
/// </summary>
/// <typeparam name="T">The type</typeparam>
public class Result<T>
{
	/// <summary>
	/// Whether the result was OK
	/// </summary>
	public bool Ok { get; }

	/// <summary>
	/// If the result is OK, this will contain the data.
	/// Otherwise, null.
	/// </summary>
	public T? Data { get; }

	/// <summary>
	/// If the result is not OK, this will contain the exception.
	/// Otherwise, null.
	/// </summary>
	public Exception? Exception { get; }

	/// <summary>
	/// Constructor for successful result
	/// </summary>
	/// <param name="data">The result data</param>
	public Result(T data)
	{
		Ok = true;
		Data = data;
		Exception = null;
	}

	/// <summary>
	/// Constructor for error result
	/// </summary>
	/// <param name="exception">The exception</param>
	public Result(Exception exception)
	{
		Ok = false;
		Data = default;
		Exception = exception;
	}

	/// <inheritdoc />
	public override string ToString() => Ok ? Data?.ToString() ?? string.Empty : Exception?.ToString() ?? string.Empty;
}