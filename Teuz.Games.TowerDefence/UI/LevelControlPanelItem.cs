using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence.UI
{
	class LevelControlPanelItem : PanelItem
	{
		public LevelControlPanelItem(IContentProvider contentProvider)
			: base(contentProvider, "Level Control")
		{
			Content = NinjectFactory.Kernel.Get<LevelControl>();
		}
	}
}
