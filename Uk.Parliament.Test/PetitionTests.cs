using System.Linq;
using Uk.Parliament.Petitions;
using Xunit;
using Xunit.Abstractions;

namespace Uk.Parliament.Test
{
	public class PetitionTests : PetitionTest
	{
		private readonly PetitionsClient _petitionsClient;

		public PetitionTests(ITestOutputHelper output) : base(output)
		{
			Output.WriteLine("Creating client");
			_petitionsClient = new PetitionsClient();
		}

		[Fact]
		public void SearchTrump()
		{
			var query = new Query
			{
				Text = "Trump",
				PageSize = 50,
				PageNumber = 1
			};

			Output.WriteLine("Executing");
			var result = _petitionsClient.GetManyAsync<Petition>(query).Result;

			// We should have a result
			Assert.NotNull(result);
			Assert.True(result.Ok);
			Assert.NotNull(result.Data);
			Assert.Null(result.Exception);

			// There should be at least 2 petitions
			var data = result.Data;
			Assert.NotEmpty(data);
			Assert.True(data.Count >= 2);

			// The SignaturesByCountry SHOULD NOT be set
			Assert.All(data.Select(d=>d.Attributes?.SignaturesByCountry), Assert.Null);

			foreach (var petitionId in data.Select(p => p.Id))
			{
				var fullPetition = _petitionsClient.GetSingleAsync<Petition>(petitionId).Result;
				Assert.NotNull(fullPetition.Data);
				Assert.NotNull(fullPetition.Data.Attributes);
				// The SignaturesByCountry SHOULD be set
				Assert.NotNull(fullPetition.Data.Attributes.SignaturesByCountry);
			}

			Output.WriteLine("Done.");
		}
	}
}
