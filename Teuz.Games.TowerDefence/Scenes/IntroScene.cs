using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using Teuz.Games.TowerDefence.States;
using FactoryDWrite = SharpDX.DirectWrite.Factory;

namespace Teuz.Games.TowerDefence.Scenes
{
	class IntroScene : Scene
	{
		private SolidColorBrush brushBlack;
		private SolidColorBrush brushWhite;
		private IContentProvider contentProvider;
		private GameStateManager gameStateManager;
		private InputManager inputManager;

		private Bitmap bitmapTeuz, bitmapTowerDefence;
		private RectangleF rectangleTeuz, rectangleTowerDefence;

		private float alphaBackground, alphaTeuz, alphaTowerDefence;

		private TimeSpan elapsed;
		private TimeSpan fadeTimeBackground = TimeSpan.FromSeconds(2.5f);
		private TimeSpan fadeTimeLogo = TimeSpan.FromSeconds(3f);
		private TimeSpan fadeTimeTowerDefence = TimeSpan.FromSeconds(1f);
		private TimeSpan timeBetweenLogoAndTowerDefence = TimeSpan.FromSeconds(2.5f);
		private TimeSpan totalLifetime = TimeSpan.FromSeconds(8f);

		public IntroScene(IGraphicsRenderer graphicsRenderer, ITextRenderer textRenderer, IContentProvider contentProvider, GameStateManager gameStateManager, InputManager inputManager)
			: base(graphicsRenderer, textRenderer)
		{
			this.contentProvider = contentProvider;
			this.gameStateManager = gameStateManager;
			this.inputManager = inputManager;
		}

		public override void Initialize()
		{
			
		}

		public override void LoadContent()
		{
			float x, y;
			float halfWidth = GraphicsRenderer.Width/2f;
			float halfHeight = GraphicsRenderer.Height/2f;

			//brushBlack = contentProvider.LoadSolidColorBrush(Color.Black);
			brushBlack = new SolidColorBrush(RenderTarget, new Color4(0f, 0f, 0f, 1.0f));
			brushWhite = contentProvider.LoadSolidColorBrush(Color.White);

			bitmapTeuz = contentProvider.LoadTexture("intro1.png").Bitmap;
			x = halfWidth - bitmapTeuz.Size.Width/2f;
			y = halfHeight - bitmapTeuz.Size.Height/2f;
			rectangleTeuz = new RectangleF(x, y, bitmapTeuz.Size.Width + x, bitmapTeuz.Size.Height + y);

			bitmapTowerDefence = contentProvider.LoadTexture("TowerDefenceLogo.png").Bitmap;
			x = halfWidth - bitmapTowerDefence.Size.Width / 2f;
			y = halfHeight - bitmapTowerDefence.Size.Height / 2f;
			rectangleTowerDefence = new RectangleF(x, y, bitmapTowerDefence.Size.Width + x, bitmapTowerDefence.Size.Height + y);
		}

		public override void UnloadContent()
		{
			
		}

		public override void Draw(GameTime gameTime)
		{
			//this.RenderTarget.FillRectangle(new RectangleF(0, 0, GameWindow.Width, GameWindow.Height), backgroundBrush);

			this.RenderTarget.FillRectangle(
				new RectangleF(0, 0, GraphicsRenderer.Width, GraphicsRenderer.Height),
				brushWhite
			);

			if(alphaTeuz > 0)
				this.RenderTarget.DrawBitmap(bitmapTeuz, rectangleTeuz, alphaTeuz, BitmapInterpolationMode.Linear);

			if (alphaBackground > 0)
			{
				brushBlack.Dispose();
				this.RenderTarget.FillRectangle(
					new RectangleF(0, 0, GraphicsRenderer.Width, GraphicsRenderer.Height),
					brushBlack = new SolidColorBrush(RenderTarget, new Color4(0f, 0f, 0f, alphaBackground))
				);
			}

			if(alphaTowerDefence > 0)
				this.RenderTarget.DrawBitmap(bitmapTowerDefence, rectangleTowerDefence, alphaTowerDefence, BitmapInterpolationMode.Linear);
		}

		public override void Update(GameTime gameTime)
		{
			if (inputManager.MouseReleased != MouseButtons.None || inputManager.KeysPressed.Any(x => x == Keys.Enter))
			{
				Next();
				return;
			}

			this.elapsed += gameTime.Elapsed;
			this.alphaBackground = 1 - EaseHelper.Cubic(elapsed.TotalMilliseconds / fadeTimeBackground.TotalMilliseconds);
			this.alphaTeuz = EaseHelper.Cubic(elapsed.TotalMilliseconds / fadeTimeLogo.TotalMilliseconds);
			this.alphaTowerDefence = EaseHelper.Cubic(
				(elapsed - fadeTimeLogo - timeBetweenLogoAndTowerDefence).TotalMilliseconds /
				fadeTimeTowerDefence.TotalMilliseconds
			);

			if (alphaTowerDefence > 0)
			{
				this.alphaTeuz -= this.alphaTowerDefence;
				this.alphaBackground = this.alphaTowerDefence;
			}

			if (elapsed > totalLifetime)
			{
				Next();
			}
		}

		private void Next()
		{
			gameStateManager.ReplaceState<ShoppingState>();
		}
	}
}
