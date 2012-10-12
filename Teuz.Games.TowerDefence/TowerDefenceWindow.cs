using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Teuz.Games.TowerDefence.Levels;
using Teuz.Games.TowerDefence.Scenes;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence
{
	class TowerDefenceWindow : GameWindow, ITextRenderer, IGraphicsRenderer
	{
		private TextFormat textFormat;
		private SolidColorBrush textBrush;

		private RectangleF clientRectangle;

		public int Width { get { return Form.Width; } }
		public int Height { get { return Form.Height; } }

		private IKernel kernel;

		public InputManager InputManager { get; private set; }
		public IContentProvider ContentProvider { get; private set; }
		public GameStateManager GameStateManager { get; private set; }
		public SceneManager SceneManager { get; private set; }

		public TowerDefenceWindow()
		{
			this.Title = "Tower Defence";
			this.kernel = NinjectFactory.Kernel;

			Console.WriteLine("TowerDefenceWindow constructed.");
		}

		protected override void CreateForm()
		{
			base.CreateForm();
			this.Form.Width = 1280;
			this.Form.Height = 720;
			this.Form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		}

		protected override void Initialize()
		{
			// this.input = kernel.Get<InputManager>();
			//this.contentProvider = kernel.Get<IContentProvider>();
			this.ContentProvider = new ContentProvider(RenderTarget);
			this.InputManager = new InputManager(this);
			this.GameStateManager = new GameStateManager();
			this.SceneManager = new SceneManager();

			// GameStateManager.PushState<IntroState>();
            GameStateManager.PushState<ShoppingState>();

			InputManager.Initialize();

			SceneManager.CurrentScene.Initialize();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			clientRectangle = new SharpDX.RectangleF(0, 0, Form.Width, Form.Height);

			kernel.Get<LevelProvider>().LoadContent();

			SceneManager.CurrentScene.LoadContent();

			textFormat = new TextFormat(FactoryDWrite, "Calibri", 18);
			textBrush = new SolidColorBrush(RenderTarget, Color.Black);

			RenderTarget.TextAntialiasMode = TextAntialiasMode.Cleartype;
		}

		protected override void UnloadContent()
		{
			
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			SceneManager.CurrentScene.Update(gameTime);

			InputManager.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			SceneManager.CurrentScene.Draw(gameTime);

			RectangleF rect = new RectangleF(Form.Width - 150, 0, Form.Width, Form.Height);
			RenderTarget.DrawText("FPS: " + FramesPerSecond, textFormat, rect, textBrush);
		}
	}
}
