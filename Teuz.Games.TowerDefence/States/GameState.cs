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
        public bool Initialized { get; set; }

		public GameState(GameStateManager gameStateManager)
		{
			this.Manager = gameStateManager;
		}

		public virtual void Activate()
		{

		}

        public virtual void Initialize()
        {

        }
	}
}
