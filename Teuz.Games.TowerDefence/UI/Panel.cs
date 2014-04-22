using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SharpDX;
using SharpDX.Direct2D1;

namespace Teuz.Games.TowerDefence.UI
{
	class Panel : UIControl
	{
		public SolidColorBrush BackgroundBrush { get; set; }

		public Panel(IContentProvider contentProvider): base(contentProvider)
		{
			Width = 100;
			Height = 100;
		}

		public override void LoadContent()
		{
			BackgroundBrush = new SolidColorBrush(RenderTarget, Color.Gray);
			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			RenderTarget.FillRectangle(
				new RectangleF(Position.X, Position.Y, Width, Height),
				BackgroundBrush
			);

			int y = 0;
			foreach (var item in Children)
			{
				if (y + item.Height < Height)
				{
					item.Width = Width;
					item.Position = new Vector2(Position.X, Position.Y + y);
					item.Draw(gameTime);
					y += item.Height;
				}
				else
				{
					break;
				}
			}
		}
	}
}
