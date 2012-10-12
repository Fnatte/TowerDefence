using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.Scenes;

namespace Teuz.Games.TowerDefence.States
{
	class DefendingState : GameState
	{
		private int health;

		public int Health
		{
			get { return health; }
			set
			{
				if (health != value)
				{
					health = value;

					if (health <= 0) Die();
				}
			}
		}

		public int EarnedCash { get; set; }

		private Player player;
        private GameScene gameScene;
        private SceneManager sceneManager;

		public DefendingState(GameStateManager gameStateManager, Player player, GameScene gameScene, SceneManager sceneManager) : base(gameStateManager)
		{
			Health = 5;
			this.player = player;
            this.gameScene = gameScene;
            this.sceneManager = sceneManager;
		}

		private void Die()
		{
			Manager.PopState();
		}

		public void EndSuccessfully()
		{
			player.Cash += EarnedCash;
			player.CurrentLevelId++;
		}
	}
}
