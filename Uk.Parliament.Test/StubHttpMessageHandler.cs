using System.Net;
using System.Net.Http;
using System.Text;

namespace Uk.Parliament.Test;

/// <summary>Serves canned responses and records every request, so no test touches the network.</summary>
internal sealed class StubHttpMessageHandler(Func<HttpRequestMessage, string> respond) : HttpMessageHandler
{
	/// <summary>The requests received, in order.</summary>
	public List<HttpRequestMessage> Requests { get; } = [];

	/// <inheritdoc />
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		Requests.Add(request);

		return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(respond(request), Encoding.UTF8, "application/json")
		});
	}
}
