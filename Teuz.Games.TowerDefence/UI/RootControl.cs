using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.UI
{
	class RootControl : UIControl
	{
		private readonly List<UIElement> drawList = new List<UIElement>();
		public List<UIElement> DrawList { get { return drawList; } }

		public RootControl(IContentProvider contentProvider) : base(contentProvider)
		{

		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			foreach (var element in DrawList.OrderBy(x => x.DrawOrder))
				element.Draw(gameTime);

			DrawList.Clear();
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var child in Children)
			{
				child.Position = Position;
				child.Height = Height;
				child.Width = Width;
			}

			base.Update(gameTime);
		}
	}
}
