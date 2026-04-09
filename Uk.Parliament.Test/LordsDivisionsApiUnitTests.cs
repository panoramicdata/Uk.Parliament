using System.Text.Json;
using Uk.Parliament.Models.Divisions;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Lords Divisions API (mocking)
/// </summary>
public class LordsDivisionsApiUnitTests : IntegrationTestBase
{
	/// <summary>Verifies that <see cref="ILordsDivisionsApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void LordsDivisionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ILordsDivisionsApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	/// <summary>Verifies that the <see cref="ILordsDivisionsApi"/> interface is defined and accessible.</summary>
	[Fact]
	public void LordsDivisionsApi_InterfaceExists()
	{
		var interfaceType = typeof(ILordsDivisionsApi);
		_ = interfaceType.IsInterface.Should().BeTrue();
		_ = interfaceType.Name.Should().Be("ILordsDivisionsApi");
	}

	/// <summary>Verifies that <c>SearchDivisionsAsync</c> returns the mocked Lords division data.</summary>
	[Fact]
	public async Task SearchDivisionsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<ILordsDivisionsApi>();
		var expectedDivisions = new List<LordsDivision>
		{
			new()
			{
				DivisionId = 3571,
				Title = "Test Amendment Order",
				AuthoritativeContentCount = 26,
				AuthoritativeNotContentCount = 134,
				IsGovernmentWin = true
			}
		};

		_ = mockApi.Setup(x => x.SearchDivisionsAsync(
			It.IsAny<SearchLordsDivisionsRequest>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedDivisions);

		// Act
		var result = await mockApi.Object.SearchDivisionsAsync(
			new SearchLordsDivisionsRequest { SearchTerm = "Amendment" },
			CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().ContainSingle();
		_ = result[0].Title.Should().Be("Test Amendment Order");
		_ = result[0].AuthoritativeContentCount.Should().Be(26);
		_ = result[0].IsGovernmentWin.Should().BeTrue();
	}

	/// <summary>Verifies that endpoint payloads with a null <c>isGovernmentWin</c> value deserialize successfully.</summary>
	[Fact]
	public void LordsDivision_Deserialize_WithNullIsGovernmentWin_SetsNullableProperty()
	{
		// Arrange
		const string json = """
		[
			{
				"divisionId": 3571,
				"title": "Test Division",
				"authoritativeContentCount": 26,
				"authoritativeNotContentCount": 134,
				"isGovernmentWin": null
			}
		]
		""";

		// Act
		var result = JsonSerializer.Deserialize<List<LordsDivision>>(json);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result!.Should().ContainSingle();
		_ = result[0].DivisionId.Should().Be(3571);
		_ = result[0].IsGovernmentWin.Should().BeNull();
	}

	/// <summary>Verifies that endpoint payloads missing <c>isGovernmentWin</c> deserialize successfully.</summary>
	[Fact]
	public void LordsDivision_Deserialize_WithoutIsGovernmentWin_SetsNullableProperty()
	{
		// Arrange
		const string json = """
		[
			{
				"divisionId": 3572,
				"title": "Test Division Without Win Flag",
				"authoritativeContentCount": 10,
				"authoritativeNotContentCount": 12
			}
		]
		""";

		// Act
		var result = JsonSerializer.Deserialize<List<LordsDivision>>(json);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result!.Should().ContainSingle();
		_ = result[0].DivisionId.Should().Be(3572);
		_ = result[0].IsGovernmentWin.Should().BeNull();
	}

	/// <summary>Verifies that endpoint payloads with a true <c>isGovernmentWin</c> value deserialize correctly.</summary>
	[Fact]
	public void LordsDivision_Deserialize_WithTrueIsGovernmentWin_SetsNullablePropertyTrue()
	{
		// Arrange
		const string json = """
		[
			{
				"divisionId": 3573,
				"title": "Test Division True",
				"authoritativeContentCount": 15,
				"authoritativeNotContentCount": 5,
				"isGovernmentWin": true
			}
		]
		""";

		// Act
		var result = JsonSerializer.Deserialize<List<LordsDivision>>(json);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result!.Should().ContainSingle();
		_ = result[0].DivisionId.Should().Be(3573);
		_ = result[0].IsGovernmentWin.Should().BeTrue();
	}

	/// <summary>Verifies that <see cref="ILordsDivisionsApi.GetDivisionByIdAsync"/> returns the mocked division with members.</summary>
	[Fact]
	public async Task GetDivisionByIdAsync_WithMock_ReturnsDivision()
	{
		// Arrange
		var mockApi = new Mock<ILordsDivisionsApi>();
		var expectedDivision = new LordsDivision
		{
			DivisionId = 3571,
			Title = "Test Division",
			IsWhipped = true,
			DivisionHadTellers = true,
			Contents =
			[
				new() { MemberId = 4579, Name = "Test Peer", Party = "Labour", PartyIsMainParty = true }
			]
		};

		_ = mockApi.Setup(x => x.GetDivisionByIdAsync(3571, It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedDivision);

		// Act
		var result = await mockApi.Object.GetDivisionByIdAsync(3571, CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.DivisionId.Should().Be(3571);
		_ = result.IsWhipped.Should().BeTrue();
		_ = result.Contents.Should().ContainSingle();
		_ = result.Contents[0].Party.Should().Be("Labour");
	}
}
