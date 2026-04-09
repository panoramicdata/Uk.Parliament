using Uk.Parliament.Models.Divisions;

namespace Uk.Parliament.Test;

/// <summary>
/// Unit tests for Commons Divisions API (mocking)
/// </summary>
public class CommonsDivisionsApiUnitTests : IntegrationTestBase
{
	/// <summary>Verifies that <see cref="ICommonsDivisionsApi"/> can be mocked using Moq.</summary>
	[Fact]
	public void CommonsDivisionsApi_CanBeMocked()
	{
		// Arrange
		var mock = new Mock<ICommonsDivisionsApi>();

		// Assert
		_ = mock.Object.Should().NotBeNull();
	}

	/// <summary>Verifies that the <see cref="ICommonsDivisionsApi"/> interface is defined and accessible.</summary>
	[Fact]
	public void CommonsDivisionsApi_InterfaceExists()
	{
		var interfaceType = typeof(ICommonsDivisionsApi);
		_ = interfaceType.IsInterface.Should().BeTrue();
		_ = interfaceType.Name.Should().Be("ICommonsDivisionsApi");
	}

	/// <summary>Verifies that <c>SearchDivisionsAsync</c> returns mocked Commons division data.</summary>
	[Fact]
	public async Task SearchDivisionsAsync_WithMock_ReturnsExpectedData()
	{
		// Arrange
		var mockApi = new Mock<ICommonsDivisionsApi>();
		var expectedDivisions = new List<CommonsDivision>
		{
			new()
			{
				DivisionId = 2205,
				Title = "Budget Resolution No. 64",
				AyeCount = 357,
				NoCount = 174,
				Date = new DateTime(2025, 12, 2)
			}
		};

		_ = mockApi.Setup(x => x.SearchDivisionsAsync(
			It.IsAny<SearchCommonsDivisionsRequest>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedDivisions);

		// Act
		var result = await mockApi.Object.SearchDivisionsAsync(
			new SearchCommonsDivisionsRequest { SearchTerm = "Budget" },
			CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().ContainSingle();
		_ = result[0].Title.Should().Be("Budget Resolution No. 64");
		_ = result[0].AyeCount.Should().Be(357);
	}

	/// <summary>Verifies that <see cref="ICommonsDivisionsApi.GetDivisionByIdAsync"/> returns the mocked division including tellers.</summary>
	[Fact]
	public async Task GetDivisionByIdAsync_WithMock_ReturnsDivision()
	{
		// Arrange
		var mockApi = new Mock<ICommonsDivisionsApi>();
		var expectedDivision = new CommonsDivision
		{
			DivisionId = 2205,
			Title = "Budget Resolution No. 64",
			AyeCount = 357,
			NoCount = 174,
			AyeTellers =
			[
				new() { MemberId = 5075, Name = "Test Teller", Party = "Labour" }
			]
		};

		_ = mockApi.Setup(x => x.GetDivisionByIdAsync(2205, It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedDivision);

		// Act
		var result = await mockApi.Object.GetDivisionByIdAsync(2205, CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.DivisionId.Should().Be(2205);
		_ = result.AyeTellers.Should().ContainSingle();
		_ = result.AyeTellers[0].Party.Should().Be("Labour");
	}

	/// <summary>Verifies that <c>GetMemberVotingAsync</c> returns the mocked member voting history.</summary>
	[Fact]
	public async Task GetMemberVotingAsync_WithMock_ReturnsVotingHistory()
	{
		// Arrange
		var mockApi = new Mock<ICommonsDivisionsApi>();
		var expectedRecords = new List<MemberVotingRecord>
		{
			new()
			{
				MemberId = 172,
				MemberVotedAye = true,
				MemberVotedNo = false,
				MemberWasTeller = false,
				PublishedDivision = new CommonsDivision
				{
					DivisionId = 2298,
					Title = "Test Division"
				}
			}
		};

		_ = mockApi.Setup(x => x.GetMemberVotingAsync(
			It.IsAny<GetCommonsMemberVotingRequest>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedRecords);

		// Act
		var result = await mockApi.Object.GetMemberVotingAsync(
			new GetCommonsMemberVotingRequest { MemberId = 172 },
			CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().ContainSingle();
		_ = result[0].MemberVotedAye.Should().BeTrue();
		_ = result[0].PublishedDivision!.Title.Should().Be("Test Division");
	}
}
