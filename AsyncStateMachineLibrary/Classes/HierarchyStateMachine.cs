using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace AsyncStateMachine
{
	public class HierarchyStateMachine : StateMachine
	{
		Dictionary<IExitableState, HashSet<IExitableState>> Parents { get; } = new();
		Stack<IExitableState>                               Stack   { get; } = new();

		public void AddConnection(IExitableState state, IExitableState parent)
		{
			if (!Parents.ContainsKey(state))
				Parents.Add(state, new HashSet<IExitableState>());

			Parents[state].Add(parent);
		}

		public override bool IsEntered(IExitableState state) => Stack.Contains(state) || base.IsEntered(state);

		protected override async UniTask ExitCurrent<TState>()
		{
			var nextState = GetState<TState>();

			while (Current != null)
			{
				var currentIsParentForNext = Parents.ContainsKey(nextState) && Parents[nextState].Contains(Current);
				if (currentIsParentForNext)
				{
					Stack.Push(Current);
					return;
				}

				await ExitCurrent();

				Current = Stack.Count > 0 ? Stack.Pop() : null;
			}
		}
	}
}