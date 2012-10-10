using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence
{
	static class NinjectFactory
	{
		private static readonly IKernel kernel = new StandardKernel(
			new GameModule()
		);

		public static IKernel Kernel
		{
			get { return kernel; }
		}

	}
}
