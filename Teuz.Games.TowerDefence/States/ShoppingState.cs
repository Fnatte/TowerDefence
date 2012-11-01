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
		private bool firstTimeActivated = true;

		public ShoppingState(GameStateManager gameStateManager, SceneManager sceneManager, GameScene gameState)
			: base(gameStateManager)
		{
			this.gameScene = gameState;
			this.sceneManager = sceneManager;
		}

		public override void Activate()
		{
			base.Activate();

			sceneManager.CurrentScene = gameScene;

			if (firstTimeActivated)
			{
				gameScene.Initialize();
				gameScene.LoadContent();
				firstTimeActivated = false;
			}
		}
	}
}
