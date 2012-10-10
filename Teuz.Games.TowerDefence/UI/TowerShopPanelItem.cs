using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence.UI
{
	class TowerShopPanelItem : PanelItem
	{
		public TowerShopPanelItem(IContentProvider contentProvider) : base(contentProvider, "Tower Shop")
		{
			this.Height = 200;
			this.Content = NinjectFactory.Kernel.Get<TowerShopControl>();
		}
	}
}
