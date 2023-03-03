using System.Collections.Generic;
using System.Threading.Tasks;

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

		protected override async Task ExitCurrent<TState>()
		{
			var nextState = GetState<TState>();

			while (Current != null)
			{
				if (Parents.ContainsKey(nextState) && Parents[nextState].Contains(Current))
				{
					Stack.Push(Current);
					return;
				}

				await Current.Exit();

				Current = Stack.Count > 0 ? Stack.Pop() : null;
			}
		}

		// protected async Task ExitCurrent2<TState>()
		// {
		// 	var nextState = GetState<TState>();
		//
		// 	if (Current == null)
		// 		return;
		//
		// 	while (true)
		// 	{
		// 		var currentIsParentForNext = Parents.ContainsKey(nextState) && Parents[nextState].Contains(Current);
		// 		if (currentIsParentForNext)
		// 		{
		// 			Stack.Push(Current);
		// 			break;
		// 		}
		//
		// 		await Current.Exit();
		//
		// 		var bothHasParents = Parents.ContainsKey(Current) && Parents.ContainsKey(nextState);
		// 		if (bothHasParents)
		// 		{
		// 			var parentFound = false;
		//
		// 			foreach (var currentParent in Parents[Current])
		// 			{
		// 				var statesHasSameParent = Parents[nextState].Contains(currentParent);
		//
		// 				if (!statesHasSameParent)
		// 					continue;
		//
		// 				Current = currentParent;
		// 				parentFound = true;
		// 				break;
		// 			}
		//
		// 			if (!parentFound)
		// 				Current = Stack.Pop();
		// 		}
		// 		else
		// 		{
		// 			break;
		// 		}
		// 	}
		// }
	}
}