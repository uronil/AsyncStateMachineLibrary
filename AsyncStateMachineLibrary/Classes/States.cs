using Cysharp.Threading.Tasks;

namespace AsyncStateMachine
{
	public interface IState : IExitableState
	{
		UniTask Enter();
	}

	public interface IPayloadedState<in TPayload> : IExitableState
	{
		UniTask Enter(TPayload payload);
	}

	public interface IExitableState
	{
		UniTask Exit();
	}
}