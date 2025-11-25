using System;
using System.Net;

namespace Uk.Parliament.Exceptions;

/// <summary>
/// An HTTP Status response exception
/// </summary>
/// <remarks>
///  Constructor
/// </remarks>
/// <param name="statusCode"></param>
/// <param name="message"></param>
public class HttpStatusResponseException(HttpStatusCode statusCode, string message) : Exception(message)
{
	/// <summary>
	///  The HTTP Status Code
	/// </summary>
	public HttpStatusCode StatusCode { get; } = statusCode;
}