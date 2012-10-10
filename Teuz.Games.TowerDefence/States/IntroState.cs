using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.Scenes;
using Ninject;

namespace Teuz.Games.TowerDefence.States
{
	class IntroState : GameState
	{
		private IntroScene introScene;
		private SceneManager sceneManager;

		public IntroState(GameStateManager gameStateManager, SceneManager sceneManager, IntroScene introScene) : base(gameStateManager)
		{
			this.introScene = introScene;
			this.sceneManager = sceneManager;
		}

		public override void Activate()
		{
			base.Activate();

			sceneManager.CurrentScene = introScene;
		}
	}
}
