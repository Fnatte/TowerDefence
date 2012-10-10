using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Teuz.Games.TowerDefence.GameEntities;
using Ninject;
using SharpDX.Direct2D1;
using Teuz.Games.TowerDefence.States;
using Teuz.Games.TowerDefence.Levels;
using Teuz.Games.TowerDefence.GameEntities.Projectiles;
using Teuz.Games.TowerDefence.Scenes;

namespace Teuz.Games.TowerDefence
{
	class GameModule : NinjectModule
	{
		public override void Load()
		{

			Bind<Random>().ToSelf().InSingletonScope();
			Bind<SpriteFactory>().ToSelf().InSingletonScope();
			Bind<TowerDefenceWindow>().ToSelf().InSingletonScope();
			Bind<GameInputContext>().ToSelf().InSingletonScope();
			Bind<Player>().ToSelf().InSingletonScope();
			Bind<LevelProvider>().ToSelf().InSingletonScope();

			Bind<ITextRenderer>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>());
			Bind<IGraphicsRenderer>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>());
			Bind<InputManager>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>().InputManager);
			Bind<GameStateManager>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>().GameStateManager);
			Bind<RenderTarget>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>().RenderTarget);
			Bind<SceneManager>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>().SceneManager);

			Bind<ICamera>().ToMethod(x => (ICamera)x.Kernel.Get<SceneManager>().CurrentScene);
			Bind<IWorld>().ToMethod(x => {
				var gameScene = x.Kernel.Get<SceneManager>().CurrentScene as GameScene;
				if (gameScene != null) return gameScene.World;
				return null;
			});

			// Content
			Bind<IContentProvider>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>().ContentProvider);

			// Entities
			Bind<Projectile>().ToSelf().WithConstructorArgument("level", 1);
		}
	}
}
