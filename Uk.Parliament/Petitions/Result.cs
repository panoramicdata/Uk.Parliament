using System;

namespace Uk.Parliament.Petitions;

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
	public T Data { get; }

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="data"></param>
	public Result(T data)
	{
		Ok = true;
		Data = data;
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="exception"></param>
	public Result(Exception exception)
	{
		Ok = false;
		Exception = exception;
	}

	/// <summary>
	/// If the result is not OK, this will contain the exception.
	/// Otherwise, null.
	/// </summary>
	public Exception Exception { get; }

	/// <inheritdoc />
	public override string ToString() => Ok ? Data.ToString() : Exception.ToString();
}