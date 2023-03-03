using NUnit.Framework;

namespace Tests
{
	public static class TestExtensions
	{
		public static void True(this bool value) => Assert.True(value);
		public static void False(this bool value) => Assert.False(value);
	}
}