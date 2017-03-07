using System;
using System.Net;
using JetBrains.Annotations;

namespace Uk.Parliament.Exceptions
{
	/// <summary>
	/// An HTTP Status response exception
	/// </summary>
	public class HttpStatusResponseException : Exception
	{
		/// <summary>
		///  The HTTP Status Code
		/// </summary>
		[PublicAPI]
		public HttpStatusCode StatusCode { get; }

		/// <summary>
		///  Constructor
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		public HttpStatusResponseException(HttpStatusCode statusCode, string message) : base(message)
		{
			StatusCode = statusCode;
		}
	}
}