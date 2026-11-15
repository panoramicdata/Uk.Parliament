using System.Net.Http;

namespace Uk.Parliament.Test;

/// <summary>Unit tests for the IsReadOnly guard.</summary>
public class ReadOnlyHttpMessageHandlerTests
{
	private static readonly Uri Target = new("https://members-api.parliament.uk/api/Members/1");

	private static (HttpMessageInvoker Invoker, StubHttpMessageHandler Stub) Create(ParliamentClientOptions options)
	{
		var stub = new StubHttpMessageHandler(_ => "{}");
		return (new HttpMessageInvoker(new ReadOnlyHttpMessageHandler(stub, options)), stub);
	}

	/// <summary>The option defaults to false so adding it changes nothing for existing callers.</summary>
	[Fact]
	public void IsReadOnly_DefaultsToFalse()
		=> new ParliamentClientOptions().IsReadOnly.Should().BeFalse();

	/// <summary>Reads pass through when the client is read-only.</summary>
	[Theory]
	[InlineData("GET")]
	[InlineData("HEAD")]
	[InlineData("OPTIONS")]
	public async Task Send_WhenReadOnly_AllowsReads(string method)
	{
		var (invoker, stub) = Create(new ParliamentClientOptions { IsReadOnly = true });

		using var response = await invoker.SendAsync(new HttpRequestMessage(new HttpMethod(method), Target), TestContext.Current.CancellationToken);

		_ = response.IsSuccessStatusCode.Should().BeTrue();
		_ = stub.Requests.Should().ContainSingle();
	}

	/// <summary>Writes throw before any request reaches the inner handler.</summary>
	[Theory]
	[InlineData("POST")]
	[InlineData("PUT")]
	[InlineData("PATCH")]
	[InlineData("DELETE")]
	public async Task Send_WhenReadOnly_RejectsWritesBeforeSending(string method)
	{
		var (invoker, stub) = Create(new ParliamentClientOptions { IsReadOnly = true });

		var act = () => invoker.SendAsync(new HttpRequestMessage(new HttpMethod(method), Target), TestContext.Current.CancellationToken);

		var exception = await act.Should().ThrowAsync<InvalidOperationException>();
		_ = exception.Which.Message.Should().Contain(method).And.Contain("read-only");
		_ = stub.Requests.Should().BeEmpty();
	}

	/// <summary>Writes pass through when the option is off.</summary>
	[Fact]
	public async Task Send_WhenNotReadOnly_AllowsWrites()
	{
		var (invoker, stub) = Create(new ParliamentClientOptions());

		using var response = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Post, Target), TestContext.Current.CancellationToken);

		_ = response.IsSuccessStatusCode.Should().BeTrue();
		_ = stub.Requests.Should().ContainSingle();
	}

	/// <summary>The option is read on every request, so it can be switched on after construction.</summary>
	[Fact]
	public async Task Send_WhenOptionTurnedOnLater_RejectsWrites()
	{
		var options = new ParliamentClientOptions();
		var (invoker, _) = Create(options);
		options.IsReadOnly = true;

		var act = () => invoker.SendAsync(new HttpRequestMessage(HttpMethod.Delete, Target), TestContext.Current.CancellationToken);

		_ = await act.Should().ThrowAsync<InvalidOperationException>();
	}

	/// <summary>A read-only client still serves its GET calls through the full pipeline.</summary>
	[Fact]
	public async Task Client_WhenReadOnly_StillServesGets()
	{
		var stub = new StubHttpMessageHandler(_ => """{"items":[],"totalResults":0,"itemsPerPage":0}""");
		using var client = new ParliamentClient(stub, new ParliamentClientOptions { IsReadOnly = true });

		var result = await client.Bills.GetBillsAsync(new GetBillsRequest { Take = 1 }, TestContext.Current.CancellationToken);

		_ = result.Items.Should().BeEmpty();
		_ = stub.Requests.Should().ContainSingle().Which.Method.Should().Be(HttpMethod.Get);
	}
}
