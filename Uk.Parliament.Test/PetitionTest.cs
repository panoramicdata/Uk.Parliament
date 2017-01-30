using Xunit.Abstractions;

namespace Uk.Parliament.Test
{
	public abstract class PetitionTest
	{
		protected ITestOutputHelper Output { get; }

		protected PetitionTest(ITestOutputHelper output)
		{
			Output = output;
		}
	}
}