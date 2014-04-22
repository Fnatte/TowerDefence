using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace Teuz.Games.TowerDefence.UI
{
	class PanelItem : UIControl
	{
		public string Title
		{
			get { return tbTitle.Text; }
			set { tbTitle.Text = value; }
		}
		
		public SolidColorBrush BackgroundBrush { get; set; }
		public SolidColorBrush OutlineBrush { get; set; }
		public SolidColorBrush HeaderBackgroundBrush { get; set; }

		private TextBlock tbTitle;

		private const int HeaderHeight = 25;
		private const int TitleLeftPadding = 10;

		private UIElement content;

		public UIElement Content
		{
			get { return content; }
			set
			{
				content = value;
				if (Children.Contains(content) == false)
				{
					Children.Add(content);
				}
				OnPropertyChanged("Content");
			}
		}


		public PanelItem(IContentProvider contentProvider, string title = "") : base(contentProvider)
		{
			this.Height = 100;
			this.tbTitle = new TextBlock(title);

			this.Children.Add(tbTitle);
		}

		public override void LoadContent()
		{
			HeaderBackgroundBrush = new SolidColorBrush(RenderTarget, Color.DarkGray);
			OutlineBrush = new SolidColorBrush(RenderTarget, Color.Black);
			BackgroundBrush = new SolidColorBrush(RenderTarget, Color.LightGray);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			
			if (!Enabled) return;
			
			base.Update(gameTime);

			if (Content != null)
			{
				Content.Width = Width;
				Content.Height = Height - HeaderHeight;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (!Visible) return;

			RenderTarget.FillRectangle(
				new RectangleF(Position.X, Position.Y, Width, Height),
				BackgroundBrush
			);

			RenderTarget.FillRectangle(
				new RectangleF(Position.X, Position.Y, Width, HeaderHeight),
				HeaderBackgroundBrush
			);

			tbTitle.Position = Position + new Vector2(TitleLeftPadding, 0);
			tbTitle.Draw(gameTime);

			RenderTarget.DrawLine(
				(Position + new Vector2(0, HeaderHeight)),
				(Position + new Vector2(Width, HeaderHeight)),
				OutlineBrush
			);

			RenderTarget.DrawLine(
				(Position + new Vector2(0, Height)),
				(Position + new Vector2(Width, Height)),
				OutlineBrush
			);

			if (Content != null)
			{
				Content.Position = Position + new Vector2(0, HeaderHeight);
				Content.Draw(gameTime);
			}
		}
	}
}
