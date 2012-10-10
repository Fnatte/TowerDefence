using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence.UI
{
	class TowerPanelItem : PanelItem
	{
		public TowerPanelItem(IContentProvider contentProvider) : base(contentProvider, "Tower")
		{
			Visible = false;
			Enabled = false;

			Content = NinjectFactory.Kernel.Get<TowerControl>();
			Height = 150;
		}
	}
}
