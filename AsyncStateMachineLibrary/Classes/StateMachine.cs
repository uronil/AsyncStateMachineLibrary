using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace AsyncStateMachine
{
	public class StateMachine
	{
		public event Action<IExitableState> OnEnter;
		public event Action<IExitableState> OnExit;
		public event Action                 OnChange;

		public Dictionary<Type, IExitableState> States  { get; } = new();
		public IExitableState                   Current { get; set; }

		public void Add(IExitableState state)
		{
			States.Add(state.GetType(), state);
		}

		public virtual bool IsEntered(IExitableState state) => Current == state;

		public async UniTask Enter<TState>() where TState : class, IState
		{
			var state = await ChangeState<TState>();
			await state.Enter();
			OnEnter?.Invoke(state);
			OnChange?.Invoke();
		}

		public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
		{
			var state = await ChangeState<TState>();
			await state.Enter(payload);
			OnEnter?.Invoke(state);
			OnChange?.Invoke();
		}

		async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
		{
			await ExitCurrent<TState>();

			var nextState = GetState<TState>();
			Current = nextState;
			return nextState;
		}

		protected virtual async UniTask ExitCurrent<TState>() where TState : class, IExitableState
		{
			if (Current != null)
				await ExitCurrent();
		}

		protected async UniTask ExitCurrent()
		{
			await Current.Exit();
			OnExit?.Invoke(Current);
			OnChange?.Invoke();
		}

		protected TState GetState<TState>() where TState : class, IExitableState
		{
			return States[typeof(TState)] as TState;
		}
	}
}