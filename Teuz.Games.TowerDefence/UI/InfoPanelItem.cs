using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence.UI
{
	class InfoPanelItem : PanelItem
	{
		public InfoPanelItem(IContentProvider contentProvider) : base(contentProvider, "Information")
		{
			this.Height = 200;
			this.Content = NinjectFactory.Kernel.Get<InfoControl>();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
