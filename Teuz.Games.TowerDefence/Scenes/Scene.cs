using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SharpDX.Direct2D1;
using FactoryDWrite = SharpDX.DirectWrite.Factory;

namespace Teuz.Games.TowerDefence.Scenes
{
	abstract class Scene : IGameModel
	{
		public IGraphicsRenderer GraphicsRenderer { get; private set; }
		public ITextRenderer TextRenderer { get; private set; }
		public FactoryDWrite FactoryDWrite { get { return TextRenderer.FactoryDWrite; } }
		public RenderTarget RenderTarget { get { return GraphicsRenderer.RenderTarget; } }
        public bool Initialized { get; set; }

		public Scene(IGraphicsRenderer graphicsRenderer, ITextRenderer textRenderer)
		{
			this.GraphicsRenderer = graphicsRenderer;
			this.TextRenderer = textRenderer;
		}

        public virtual void Initialize()
        {
            Initialized = true;
        }

		public abstract void LoadContent();
		public abstract void UnloadContent();

		public abstract void Draw(GameTime gameTime);
		public abstract void Update(GameTime gameTime);
	}
}
