using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			TowerDefenceWindow window = NinjectFactory.Kernel.Get<TowerDefenceWindow>();
			window.InitializeWindow();
			window.Run();
		}
	}
}
