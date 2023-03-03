using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace AsyncStateMachine
{
	public class StateMachine
	{
		Dictionary<Type, IExitableState> States { get; } = new();

		protected IExitableState Current { get; set; }

		public void Add(IExitableState state)
		{
			States.Add(state.GetType(), state);
		}
		
		public virtual bool IsEntered(IExitableState state) => Current == state;

		public async UniTask Enter<TState>() where TState : class, IState
		{
			var state = await ChangeState<TState>();
			await state.Enter();
		}

		public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
		{
			var state = await ChangeState<TState>();
			await state.Enter(payload);
		}

		async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
		{
			await ExitCurrent<TState>();

			var nextState = GetState<TState>();
			Current = nextState;
			return nextState;
		}

		protected virtual async Task ExitCurrent<TState>() where TState : class, IExitableState
		{
			if (Current != null)
				await Current.Exit();
		}

		protected TState GetState<TState>() where TState : class, IExitableState
		{
			return States[typeof(TState)] as TState;
		}
	}
}