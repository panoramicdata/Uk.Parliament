namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for mocking IPetitionsApi
/// </summary>
public class PetitionsApiMockTests : IntegrationTestBase
{
	[Fact]
	public async Task GetAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<IPetitionsApi>();
		var expectedPetitions = new List<Petition>
		{
			new() { Id = 1, Type = "petition", Attributes = new() {  Action = "Test Petition 1", Background = "Background 1", AdditionalDetails = "Details 1", State = PetitionState.Open, SignatureCount = 100, CreatedAt = DateTime.UtcNow },
			Links = new Links() },
			new() { Id = 2, Type = "petition", Attributes = new() { Action = "Test Petition 2", Background = "Background 2", AdditionalDetails = "Details 2", State = PetitionState.Open, SignatureCount = 200, CreatedAt = DateTime.UtcNow },
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
		var result = await mockApi.Object.GetAsync(state: "open",
			cancellationToken: CancellationToken);

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
				Background = "Test Background",
				AdditionalDetails = "Test Details",
				State = PetitionState.Open,
				SignatureCount = 1000,
				CreatedAt = DateTime.UtcNow
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
		var result = await mockApi.Object.GetByIdAsync(123,
			cancellationToken: CancellationToken);

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
			new() { Id = 1, Type = "petition", Attributes = new() { Action = "Archived Petition", Background = "Background", AdditionalDetails = "Details", State = PetitionState.Closed, SignatureCount = 500, CreatedAt = DateTime.UtcNow },
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
		var result = await mockApi.Object.GetArchivedAsync(
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Data.Should().ContainSingle();
		_ = result.Data[0].Attributes.State.Should().Be(PetitionState.Closed);
	}
}
