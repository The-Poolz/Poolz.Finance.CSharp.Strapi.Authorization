using Xunit;
using FluentAssertions;

namespace Poolz.Finance.CSharp.Strapi.Authorization.Tests
{
	public class UnitTest
	{
		[Fact]
		public void Test()
		{
			const string message = "hello world!";

			message.Should().Be(message);
		}
	}
}