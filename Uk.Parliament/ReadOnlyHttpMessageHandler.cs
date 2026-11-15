using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Uk.Parliament;

/// <summary>
/// HTTP message handler that refuses any request that is not a read while <see cref="ParliamentClientOptions.IsReadOnly"/> is set
/// </summary>
internal sealed class ReadOnlyHttpMessageHandler(HttpMessageHandler innerHandler, ParliamentClientOptions options) : DelegatingHandler(innerHandler)
{
	protected override Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		if (options.IsReadOnly && !IsRead(request.Method))
		{
			throw new InvalidOperationException(
				$"The client is read-only (IsReadOnly is set), so {request.Method} {request.RequestUri} was not sent.");
		}

		return base.SendAsync(request, cancellationToken);
	}

	private static bool IsRead(HttpMethod method)
		=> method == HttpMethod.Get || method == HttpMethod.Head || method == HttpMethod.Options;
}
