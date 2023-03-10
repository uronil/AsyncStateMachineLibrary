using Cysharp.Threading.Tasks;
using AsyncStateMachine;

namespace Tests
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