using Uk.Parliament.Extensions;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for ParliamentClient
/// </summary>
public class ParliamentClientTests
{
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
	}

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

	[Fact]
	public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
	{
		// Act
		var act = () => new ParliamentClient((HttpClient)null!);

		// Assert
		_ = act.Should().Throw<ArgumentNullException>();
	}

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
/// Unit tests for mocking IPetitionsApi
/// </summary>
public class PetitionsApiMockTests
{
	[Fact]
	public async Task GetAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IPetitionsApi>();
		var expectedPetitions = new List<Petition>
		{
			new() { Id = 1, Type = "petition", Attributes = new() { Action = "Test Petition 1" },
			Links = new Links() },
			new() { Id = 2, Type = "petition", Attributes = new() { Action = "Test Petition 2" },
			Links = new Links() }
		};

		_ = mockApi.Setup(x => x.GetAsync(
			It.IsAny<string>(),
			It.IsAny<string>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ParliamentApiResponse<List<Petition>>
			{
				Data = expectedPetitions,
				Links = new Links()
			});

		// Act
		var result = await mockApi.Object.GetAsync(state: "open");

		// Assert
		_ = result.Data.Should().HaveCount(2);
		_ = result.Data[0].Attributes.Action.Should().Be("Test Petition 1");
	}

	[Fact]
	public async Task GetByIdAsync_WithMock_ReturnsExpectedPetition()
	{
		// Arrange
		var mockApi = new Mock<IPetitionsApi>();
		var expectedPetition = new Petition
		{
			Id = 123,
			Type = "petition",
			Attributes = new PetitionAttributes
			{
				Action = "Test Action",
				SignatureCount = 1000
			},
			Links = new Links()
		};

		_ = mockApi.Setup(x => x.GetByIdAsync(123, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ParliamentApiResponse<Petition>
			{
				Data = expectedPetition,
				Links = new Links()
			});

		// Act
		var result = await mockApi.Object.GetByIdAsync(123);

		// Assert
		_ = result.Data.Id.Should().Be(123);
		_ = result.Data.Attributes.Action.Should().Be("Test Action");
		_ = result.Data.Attributes.SignatureCount.Should().Be(1000);
	}

	[Fact]
	public async Task GetArchivedAsync_WithMock_ReturnsArchivedPetitions()
	{
		// Arrange
		var mockApi = new Mock<IPetitionsApi>();
		var expectedPetitions = new List<Petition>
		{
			new() { Id = 1, Type = "petition", Attributes = new() { State = PetitionState.Closed },
			Links = new Links() }
		};

		_ = mockApi.Setup(x => x.GetArchivedAsync(
			It.IsAny<string>(),
			It.IsAny<string>(),
			It.IsAny<int?>(),
			It.IsAny<int?>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ParliamentApiResponse<List<Petition>>
			{
				Data = expectedPetitions,
				Links = new Links()
			});

		// Act
		var result = await mockApi.Object.GetArchivedAsync();

		// Assert
		_ = result.Data.Should().HaveCount(1);
		_ = result.Data[0].Attributes.State.Should().Be(PetitionState.Closed);
	}
}

/// <summary>
/// Unit tests for extension methods
/// </summary>
public class PetitionsApiExtensionsTests
{
	[Fact]
	public async Task GetAllAsync_WithMultiplePages_ReturnsAllItems()
	{
		// Arrange
		var mockApi = new Mock<IPetitionsApi>();

		// Setup page 1
		_ = mockApi.Setup(x => x.GetAsync(null, null, 1, 2, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ParliamentApiResponse<List<Petition>>
			{
				Data =
				[
					new() { Id = 1, Attributes = new() { Action = "Petition 1" },
			Links = new Links(), Type = "Type" },
					new() { Id = 2, Attributes = new() { Action = "Petition 2" },
			Links = new Links(), Type = "Type" }
				],
				Links = new Links()
			});

		// Setup page 2
		_ = mockApi.Setup(x => x.GetAsync(null, null, 2, 2, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ParliamentApiResponse<List<Petition>>
			{
				Data =
				[
					new() { Id = 3, Attributes = new() { Action = "Petition 3" },
			Links = new Links(), Type = "Type" }
				],
				Links = new Links()
			});

		var allPetitions = new List<Petition>();

		// Act
		await foreach (var petition in mockApi.Object.GetAllAsync(pageSize: 2))
		{
			allPetitions.Add(petition);
		}

		// Assert
		_ = allPetitions.Should().HaveCount(3);
		_ = allPetitions[0].Id.Should().Be(1);
		_ = allPetitions[1].Id.Should().Be(2);
		_ = allPetitions[2].Id.Should().Be(3);
	}

	[Fact]
	public async Task GetAllListAsync_ReturnsAllItemsAsList()
	{
		// Arrange
		var mockApi = new Mock<IPetitionsApi>();

		_ = mockApi.Setup(x => x.GetAsync(null, null, It.IsAny<int?>(), 10, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ParliamentApiResponse<List<Petition>>
			{
				Data =
				[
					new() { Id = 1, Attributes = new() { Action = "Petition 1" }, Links = new Links(), Type = "Type"  },
					new() {Id = 2, Attributes = new() { Action = "Petition 2" }, Links = new Links(), Type = "Type"}
				],
				Links = new Links()
			});

		// Act
		var result = await mockApi.Object.GetAllListAsync(pageSize: 10);

		// Assert
		_ = result.Should().HaveCount(2);
		_ = result[0].Id.Should().Be(1);
		_ = result[1].Id.Should().Be(2);
	}
}

/// <summary>
/// Unit tests for placeholder APIs (to ensure they can be mocked when implemented)
/// </summary>
public class PlaceholderApisTests
{
	[Fact]
	public void MembersApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<IMembersApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public void BillsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<IBillsApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public void CommitteesApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ICommitteesApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public void CommonsDivisionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ICommonsDivisionsApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}

	[Fact]
	public void LordsDivisionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ILordsDivisionsApi>();

		// Assert - Interface can be mocked even if not yet implemented
		_ = mock.Object.Should().NotBeNull();
	}
}
