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
            Bind<TowerDefenceWindow>().ToSelf().InSingletonScope();

			Bind<Random>().ToSelf().InSingletonScope();
			Bind<SpriteFactory>().ToSelf().InSingletonScope();
			Bind<GameInputContext>().ToSelf().InSingletonScope();
			Bind<Player>().ToSelf().InSingletonScope();
			Bind<LevelProvider>().ToSelf().InSingletonScope();
            Bind<GameStateManager>().ToSelf().InSingletonScope();
            Bind<SceneManager>().ToSelf().InSingletonScope();
            Bind<InputManager>().ToSelf().InSingletonScope();

			Bind<ITextRenderer>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>());
			Bind<IGraphicsRenderer>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>());

			Bind<RenderTarget>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>().RenderTarget);

			Bind<ICamera>().ToMethod(x => x.Kernel.Get<GameScene>());
            Bind<IWorld>().ToMethod(x => { return x.Kernel.Get<GameScene>().World; });

			// Content
			Bind<IContentProvider>().ToMethod(x => x.Kernel.Get<TowerDefenceWindow>().ContentProvider);

			// Entities
			Bind<Projectile>().ToSelf().WithConstructorArgument("level", 1);

            // GameScene
            Bind<GameScene>().ToSelf().InScope(x => x.Kernel.Get<SceneManager>());

            /*
            Bind<GameScene>().ToMethod(x =>
            {
                var gameScene = x.Kernel.Get<SceneManager>().CurrentScene as GameScene;
                if (gameScene != null) return gameScene;
                else return new GameScene(
                    x.Kernel.Get<IGraphicsRenderer>(),
                    x.Kernel.Get<ITextRenderer>(),
                    x.Kernel.Get<LevelProvider>(),
                    x.Kernel.Get<Player>(),
                    x.Kernel.Get<GameStateManager>(),
                    x.Kernel.Get<World>()
                    );
            });
            */
		}
	}
}
