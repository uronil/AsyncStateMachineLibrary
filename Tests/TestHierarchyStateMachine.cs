using System.Threading.Tasks;
using AsyncStateMachine;
using NUnit.Framework;

namespace Tests
{
	public class TestHierarchyStateMachine
	{
		[Test]
		public async Task TestAsyncHierarchyStateMachine()
		{
			var sm = new HierarchyStateMachine();
			var register = new RegisterServices();
			var game = new Game();
			var gameLoop = new GameLoop();
			var settings = new Settings();
			var exit = new Exit();

			sm.Add(register);
			sm.Add(game);
			sm.Add(gameLoop);
			sm.Add(settings);
			sm.Add(exit);
			sm.AddConnection(gameLoop, game);
			sm.AddConnection(settings, game);
			sm.AddConnection(settings, gameLoop);

			await sm.Enter<RegisterServices>();

			sm.IsEntered(register).True();
			sm.IsEntered(game).False();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).False();
			sm.IsEntered(exit).False();
			
			await sm.Enter<Game>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).True();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).False();
			sm.IsEntered(exit).False();

			await sm.Enter<Settings>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).True();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).True();
			sm.IsEntered(exit).False();

			await sm.Enter<GameLoop>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).True();
			sm.IsEntered(gameLoop).True();
			sm.IsEntered(settings).False();
			sm.IsEntered(exit).False();

			await sm.Enter<Settings>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).True();
			sm.IsEntered(gameLoop).True();
			sm.IsEntered(settings).True();
			sm.IsEntered(exit).False();

			await sm.Enter<Exit>();

			sm.IsEntered(register).False();
			sm.IsEntered(game).False();
			sm.IsEntered(gameLoop).False();
			sm.IsEntered(settings).False();
			sm.IsEntered(exit).True();
		}
	}
}