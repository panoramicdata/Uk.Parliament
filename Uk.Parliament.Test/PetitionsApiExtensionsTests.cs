using Uk.Parliament.Extensions;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for extension methods
/// </summary>
public class PetitionsApiExtensionsTests : IntegrationTestBase
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
					new() { Id = 1, Attributes = new() { Action = "Petition 1", Background = "BG1", AdditionalDetails = "AD1", State = PetitionState.Open, SignatureCount = 1, CreatedAt = DateTime.UtcNow },
			Links = new Links(), Type = "Type" },
					new() { Id = 2, Attributes = new() { Action = "Petition 2", Background = "BG2", AdditionalDetails = "AD2", State = PetitionState.Open, SignatureCount = 2, CreatedAt = DateTime.UtcNow },
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
					new() { Id = 3, Attributes = new() { Action = "Petition 3", Background = "BG3", AdditionalDetails = "AD3", State = PetitionState.Open, SignatureCount = 3, CreatedAt = DateTime.UtcNow },
			Links = new Links(), Type = "Type" }
				],
				Links = new Links()
			});

		var allPetitions = new List<Petition>();

		// Act
		await foreach (var petition in mockApi.Object.GetAllAsync(pageSize: 2,
			cancellationToken: CancellationToken))
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
					new() { Id = 1, Attributes = new() { Action = "Petition 1", Background = "BG", AdditionalDetails = "AD", State = PetitionState.Open, SignatureCount = 1, CreatedAt = DateTime.UtcNow }, Links = new Links(), Type = "Type"  },
					new() {Id = 2, Attributes = new() { Action = "Petition 2", Background = "BG", AdditionalDetails = "AD", State = PetitionState.Open, SignatureCount = 2, CreatedAt = DateTime.UtcNow }, Links = new Links(), Type = "Type"}
				],
				Links = new Links()
			});

		// Act
		var result = await mockApi.Object.GetAllListAsync(pageSize: 10,
			cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().HaveCount(2);
		_ = result[0].Id.Should().Be(1);
		_ = result[1].Id.Should().Be(2);
	}
}
