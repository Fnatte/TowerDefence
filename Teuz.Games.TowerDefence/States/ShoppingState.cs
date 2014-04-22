using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.Scenes;

namespace Teuz.Games.TowerDefence.States
{
	class ShoppingState : GameState
	{
		private GameScene gameScene;
		private SceneManager sceneManager;

		public ShoppingState(GameStateManager gameStateManager, SceneManager sceneManager, GameScene gameState)
			: base(gameStateManager)
		{
			this.gameScene = gameState;
			this.sceneManager = sceneManager;
		}

        public override void Initialize()
        {
            if (!gameScene.Initialized)
            {
                gameScene.Initialize();
                gameScene.LoadContent();
            }
        }

		public override void Activate()
		{
			base.Activate();

			sceneManager.CurrentScene = gameScene;
		}
	}
}
