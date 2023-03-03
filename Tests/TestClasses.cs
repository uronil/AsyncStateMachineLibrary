using Cysharp.Threading.Tasks;

namespace AsyncStateMachine.Tests
{
	public class TestClasses
	{
		internal class State : IState
		{
			async UniTask IExitableState.Exit()
			{
				await UniTask.Yield();
			}

			public async UniTask Enter()
			{
				await UniTask.Yield();
			}
		}

		internal class RegisterServices : State
		{
		}

		internal class Game : State
		{
		}

		internal class GameLoop : State
		{
		}

		internal class Settings : State
		{
		}

		internal class Exit : State
		{
		}
	}
}