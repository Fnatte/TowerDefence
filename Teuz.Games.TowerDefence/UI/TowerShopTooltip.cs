using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using Teuz.Games.TowerDefence.GameEntities.Towers;

namespace Teuz.Games.TowerDefence.UI
{
	class TowerShopTooltip<T> : UIControl where T : Tower
	{
		private TextBlock tbName, tbCost;
		private SolidColorBrush backgroundBrush;
		private SolidColorBrush borderBrush;
		private const int leftPadding = 5;
		private const int topPadding = 5;

		private RootControl rootControl;
		
		public TowerShopTooltip(IContentProvider contentProvider) : base(contentProvider)
		{
			var attr = (TowerAttribute)typeof(T).GetCustomAttributes(typeof(TowerAttribute), true).FirstOrDefault();
			if (attr == null) throw new Exception("Could not find a TowerAttribute for " + typeof(T).ToString());

			tbName = new TextBlock(attr.Name);
			tbName.Color = Color.Black;

			tbCost = new TextBlock("Cost: " + attr.Cost.ToString());
			tbCost.Color = Color.Black;
			tbCost.FontSize = 15;

			Children.Add(tbName);
			Children.Add(tbCost);

			this.Width = 150;
			this.Height = 75;
			this.DrawOrder = 100;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			backgroundBrush = new SolidColorBrush(RenderTarget, Color.White);
			borderBrush = new SolidColorBrush(RenderTarget, Color.Black);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			tbName.Position = Position + new Vector2(leftPadding, topPadding);
			tbCost.Position = tbName.Position + new Vector2(0, 25);

			if (rootControl == null)
				rootControl = FindRootControl();

			if (rootControl != null && Visible)
				rootControl.DrawList.Add(this);
		}

		public override void Draw(GameTime gameTime)
		{
			RectangleF rec = new RectangleF(Position.X, Position.Y, Width, Height);
			RenderTarget.FillRectangle(rec, backgroundBrush);
			RenderTarget.DrawRectangle(rec, borderBrush);

			base.Draw(gameTime);
		}
	}
}
