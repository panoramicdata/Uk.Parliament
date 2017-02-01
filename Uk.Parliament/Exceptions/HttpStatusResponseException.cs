using System;
using System.Net;

namespace Uk.Parliament.Exceptions
{
	/// <summary>
	/// An HTTP Status response exception
	/// </summary>
	public class HttpStatusResponseException : Exception
	{
		private readonly HttpStatusCode _statusCode;

		/// <summary>
		///  Constructor
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		public HttpStatusResponseException(HttpStatusCode statusCode, string message) : base(message)
		{
			_statusCode = statusCode;
		}
	}
}