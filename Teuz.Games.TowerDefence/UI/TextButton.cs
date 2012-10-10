using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using Ninject;
using SharpDX;

namespace Teuz.Games.TowerDefence.UI
{
	class TextButton : Button
	{
		private TextBlock textBlock;
		private SolidColorBrush borderBrush;
		private SolidColorBrush backgroundBrush;
		private Dictionary<ButtonState, SolidColorBrush[]> brushes = new Dictionary<ButtonState, SolidColorBrush[]>();

		private const int padding = 2;

		public TextButton(string text = "") : base()
		{
			Width = 100;
			Height = 25;

			textBlock = new TextBlock(text);
			textBlock.Color = Color.Black;
			textBlock.Parent = this;
			textBlock.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
		}

		public override void Draw(GameTime gameTime)
		{
			RectangleF rec = GetRectangle();
			RenderTarget.FillRectangle(rec, backgroundBrush);
			RenderTarget.DrawRectangle(rec, borderBrush);
			textBlock.Draw(gameTime);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			backgroundBrush = brushes[State][0];
			borderBrush = brushes[State][1];
			textBlock.Color = (Color)brushes[State][2].Color;

			textBlock.Update(gameTime);
			textBlock.Position = Position + new Vector2(0, padding);
		}

		public override void UnloadContent()
		{
			textBlock.UnloadContent();
		}

		public override void LoadContent()
		{
			textBlock.LoadContent();

			brushes.Add(ButtonState.Normal, new [] {
				ContentProvider.LoadSolidColorBrush(Color.LightCyan),
				ContentProvider.LoadSolidColorBrush(Color.Black),
				ContentProvider.LoadSolidColorBrush(Color.Black)
			});

			brushes.Add(ButtonState.Hover, new[] {
				ContentProvider.LoadSolidColorBrush(Color.White),
				ContentProvider.LoadSolidColorBrush(Color.Black),
				ContentProvider.LoadSolidColorBrush(Color.Black)
			});

			brushes.Add(ButtonState.Pressed, new[] {
				ContentProvider.LoadSolidColorBrush(Color.Gray),
				ContentProvider.LoadSolidColorBrush(Color.White),
				ContentProvider.LoadSolidColorBrush(Color.White)
			});
		}

		public override void Initialize()
		{
			textBlock.Initialize();
		}
	}
}
