using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence
{
	abstract class GameState
	{
		protected GameStateManager Manager { get; private set; }

		public GameState(GameStateManager gameStateManager)
		{
			this.Manager = gameStateManager;
		}

		public virtual void Activate()
		{

		}
	}
}
