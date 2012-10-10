using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.Scenes;

namespace Teuz.Games.TowerDefence.States
{
	class MenuState : GameState
	{
		private MenuScene menuScene;
		private SceneManager sceneManager;

		public MenuState(GameStateManager gameStateManager, SceneManager sceneManager, MenuScene menuScene)
			: base(gameStateManager)
		{
			this.menuScene = menuScene;
			this.sceneManager = sceneManager;
		}

		public override void Activate()
		{
			base.Activate();

			sceneManager.CurrentScene = menuScene;
		}
	}
}
