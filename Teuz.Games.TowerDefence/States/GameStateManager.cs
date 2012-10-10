using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence.States
{
	class GameStateManager
	{
		private readonly Stack<GameState> stateStack = new Stack<GameState>();
		public event EventHandler StateChanged;

		public GameState CurrentState
		{
			get
			{
				return stateStack.Count > 0 ? stateStack.Peek() : null;
			}
		}

		public GameStateManager()
		{
			
		}

		protected virtual void OnCurrentStateChanged()
		{
			if(CurrentState != null)
				CurrentState.Activate();

			EventHandler temp = StateChanged;
			if (temp != null)
			{
				temp(this, EventArgs.Empty);
			}
		}

		public void PushState<T>() where T : GameState
		{
			stateStack.Push(
				NinjectFactory.Kernel.Get<T>()
			);

			OnCurrentStateChanged();
		}

		public GameState ReplaceState<T>() where T : GameState
		{
			var state = PopState();
			PushState<T>();
			return state;
		}

		public GameState PopState()
		{
			GameState state = stateStack.Pop();
			OnCurrentStateChanged();
			return state;
		}
	}
}
