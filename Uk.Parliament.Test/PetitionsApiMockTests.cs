namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for mocking IPetitionsApi
/// </summary>
public class PetitionsApiMockTests : IntegrationTestBase
{
	private static Petition CreatePetition(int id, string action, PetitionState state, int signatures) => new()
	{
		Id = id,
		Type = "petition",
		Attributes = new()
		{
			Action = action,
			Background = "Background",
			AdditionalDetails = "Details",
			State = state,
			SignatureCount = signatures,
			CreatedAt = DateTime.UtcNow
		},
		Links = new Links()
	};

	private static ParliamentApiResponse<T> Wrap<T>(T data) => new() { Data = data, Links = new Links() };

	/// <summary>Verifies that a mocked <c>GetAsync</c> call returns the expected petition list.</summary>
	[Fact]
	public async Task GetAsync_WithMock_ReturnsExpectedData()
	{
		var mockApi = new Mock<IPetitionsApi>();
		_ = mockApi.Setup(x => x.GetAsync(It.IsAny<GetPetitionsRequest>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Wrap(new List<Petition>
			{
				CreatePetition(1, "Test Petition 1", PetitionState.Open, 100),
				CreatePetition(2, "Test Petition 2", PetitionState.Open, 200)
			}));

		var result = await mockApi.Object.GetAsync(new GetPetitionsRequest { State = "open" }, CancellationToken);

		_ = result.Data.Should().HaveCount(2);
		_ = result.Data[0].Attributes.Action.Should().Be("Test Petition 1");
	}

	/// <summary>Verifies that a mocked <see cref="IPetitionsApi.GetByIdAsync"/> call returns the expected petition.</summary>
	[Fact]
	public async Task GetByIdAsync_WithMock_ReturnsExpectedPetition()
	{
		var mockApi = new Mock<IPetitionsApi>();
		_ = mockApi.Setup(x => x.GetByIdAsync(123, It.IsAny<CancellationToken>()))
			.ReturnsAsync(Wrap(CreatePetition(123, "Test Action", PetitionState.Open, 1000)));

		var result = await mockApi.Object.GetByIdAsync(123, cancellationToken: CancellationToken);

		_ = result.Data.Id.Should().Be(123);
		_ = result.Data.Attributes.Action.Should().Be("Test Action");
		_ = result.Data.Attributes.SignatureCount.Should().Be(1000);
	}

	/// <summary>Verifies that a mocked <c>GetArchivedAsync</c> call returns archived petitions.</summary>
	[Fact]
	public async Task GetArchivedAsync_WithMock_ReturnsArchivedPetitions()
	{
		var mockApi = new Mock<IPetitionsApi>();
		_ = mockApi.Setup(x => x.GetArchivedAsync(It.IsAny<GetPetitionsRequest>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Wrap(new List<Petition> { CreatePetition(1, "Archived Petition", PetitionState.Closed, 500) }));

		var result = await mockApi.Object.GetArchivedAsync(new GetPetitionsRequest(), CancellationToken);

		_ = result.Data.Should().ContainSingle();
		_ = result.Data[0].Attributes.State.Should().Be(PetitionState.Closed);
	}
}
