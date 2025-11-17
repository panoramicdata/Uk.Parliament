using FluentAssertions;
using Uk.Parliament.Petitions;

namespace Uk.Parliament.Test;

public class Petitions
{
	[Fact]
	public async Task GetPetitionsAsync_Succeeds()
	{
		var petitionsClient = new PetitionsClient();

		var petitions = await petitionsClient.GetPetitionsAsync(new Query
		{
			Text = "Electric Vehicles",
			PageNumber = 1,
			PageSize = 10
		}, default);

		petitions.Ok.Should().BeTrue();
		petitions.Data.Should().NotBeNullOrEmpty();
	}
}
