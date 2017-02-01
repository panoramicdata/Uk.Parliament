using Xunit.Abstractions;

namespace Uk.Parliament.Test
{
	public abstract class PetitionTest
	{
		protected PetitionTest(ITestOutputHelper output)
		{
			Output = output;
		}

		protected ITestOutputHelper Output { get; }
	}
}