using System;
using System.Net;

namespace Uk.Parliament.Exceptions
{
	public class HttpStatusResponseException : Exception
	{
		private readonly HttpStatusCode _statusCode;

		public HttpStatusResponseException(HttpStatusCode statusCode, string message) : base(message)
		{
			_statusCode = statusCode;
		}
	}
}