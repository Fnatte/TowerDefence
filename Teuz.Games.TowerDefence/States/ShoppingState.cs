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
        private SceneManager sceneManager;
        private GameScene gameScene;

		public ShoppingState(GameStateManager gameStateManager, SceneManager sceneManager, GameScene gameScene) 
            : base(gameStateManager)
		{
            this.sceneManager = sceneManager;
            this.gameScene = gameScene;
		}

        public override void Activate()
        {
            base.Activate();

            sceneManager.CurrentScene = gameScene;
        }
	}
}
