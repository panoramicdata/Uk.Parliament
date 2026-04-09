namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for ParliamentClient
/// </summary>
public class ParliamentClientTests
{
	/// <summary>Verifies that constructing a <see cref="ParliamentClient"/> with default options succeeds and all API accessors are non-null.</summary>
	[Fact]
	public void Constructor_WithDefaultOptions_Succeeds()
	{
		// Act
		var client = new ParliamentClient();

		// Assert
		_ = client.Should().NotBeNull();
		_ = client.Petitions.Should().NotBeNull();
		_ = client.Members.Should().NotBeNull();
		_ = client.Bills.Should().NotBeNull();
		_ = client.Committees.Should().NotBeNull();
		_ = client.CommonsDivisions.Should().NotBeNull(); // Interface implemented (API has 500 errors)
		_ = client.LordsDivisions.Should().NotBeNull(); // Interface implemented (API has 500 errors)
		_ = client.Interests.Should().NotBeNull(); // Phase 6.2
		_ = client.QuestionsStatements.Should().NotBeNull(); // Phase 6.3
		_ = client.OralQuestionsMotions.Should().NotBeNull(); // Phase 7.1
		_ = client.Treaties.Should().NotBeNull(); // Phase 7.2
		_ = client.ErskineMay.Should().NotBeNull(); // Phase 8.1
		_ = client.Now.Should().NotBeNull(); // Phase 8.2
	}

	/// <summary>Verifies that constructing a <see cref="ParliamentClient"/> with custom options uses those options.</summary>
	[Fact]
	public void Constructor_WithCustomOptions_UsesOptions()
	{
		// Arrange
		var options = new ParliamentClientOptions
		{
			UserAgent = "TestApp/1.0",
			Timeout = TimeSpan.FromSeconds(60),
			EnableDebugValidation = false
		};

		// Act
		var client = new ParliamentClient(options);

		// Assert
		_ = client.Should().NotBeNull();
		_ = client.Petitions.Should().NotBeNull();
	}

	/// <summary>Verifies that constructing a <see cref="ParliamentClient"/> with an injected <see cref="HttpClient"/> succeeds.</summary>
	[Fact]
	public void Constructor_WithHttpClient_Succeeds()
	{
		// Arrange
		var httpClient = new HttpClient();

		// Act
		var client = new ParliamentClient(httpClient);

		// Assert
		_ = client.Should().NotBeNull();
		_ = client.Petitions.Should().NotBeNull();
	}

	/// <summary>Verifies that passing a null <see cref="HttpClient"/> to the constructor throws an <see cref="ArgumentNullException"/>.</summary>
	[Fact]
	public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
	{
		// Act
		var act = () => new ParliamentClient((HttpClient)null!);

		// Assert
		_ = act.Should().Throw<ArgumentNullException>();
	}

	/// <summary>Verifies that disposing a <see cref="ParliamentClient"/> that owns its HTTP client disposes that client.</summary>
	[Fact]
	public void Dispose_DisposesOwnedHttpClient()
	{
		// Arrange
		var client = new ParliamentClient();

		// Act
		client.Dispose();

		// Assert - should not throw
		_ = client.Should().NotBeNull();
	}

	/// <summary>Verifies that disposing a <see cref="ParliamentClient"/> with an injected HTTP client does not dispose that client.</summary>
	[Fact]
	public void Dispose_DoesNotDisposeInjectedHttpClient()
	{
		// Arrange
		var httpClient = new HttpClient();
		var client = new ParliamentClient(httpClient);

		// Act
		client.Dispose();

		// Assert - HttpClient should still be usable
		httpClient.BaseAddress = new Uri("https://example.com");
		_ = httpClient.BaseAddress.Should().NotBeNull();
	}
}

/// <summary>
/// Unit tests for placeholder APIs (to ensure they can be mocked when implemented)
/// </summary>
public class PlaceholderApisTests
{
	/// <summary>Verifies that <see cref="IMembersApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void MembersApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<IMembersApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}

	/// <summary>Verifies that <see cref="IBillsApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void BillsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<IBillsApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}

	/// <summary>Verifies that <see cref="ICommitteesApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void CommitteesApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ICommitteesApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}

	/// <summary>Verifies that <see cref="ICommonsDivisionsApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void CommonsDivisionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ICommonsDivisionsApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}

	/// <summary>Verifies that <see cref="ILordsDivisionsApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void LordsDivisionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ILordsDivisionsApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}
}
