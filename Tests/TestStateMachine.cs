using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncStateMachine.Tests
{
	public class TestStateMachine
	{
		[Test]
		public async Task TestAsyncStateMachine()
		{
			var sm = new StateMachine();
			var register = new TestClasses.RegisterServices();
			var game = new TestClasses.Game();
			var gameLoop = new TestClasses.GameLoop();
			var settings = new TestClasses.Settings();
			var exit = new TestClasses.Exit();

			sm.Add(register);
			sm.Add(game);
			sm.Add(gameLoop);
			sm.Add(settings);
			sm.Add(exit);

			await sm.Enter<TestClasses.RegisterServices>();

			sm.IsEntered(register).True();
			sm.IsEntered(game).False();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).False();
			sm.IsEntered(exit).False();

			await sm.Enter<TestClasses.Game>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).True();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).False();
			sm.IsEntered(exit).False();

			await sm.Enter<TestClasses.Settings>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).False();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).True();
			sm.IsEntered(exit).False();

			await sm.Enter<TestClasses.GameLoop>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).False();
			sm.IsEntered(gameLoop).True();
			sm.IsEntered(settings).False();
			sm.IsEntered(exit).False();

			await sm.Enter<TestClasses.Settings>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).False();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).True();
			sm.IsEntered(exit).False();

			await sm.Enter<TestClasses.Exit>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).False();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).False();
			sm.IsEntered(exit).True();
		}
	}
}