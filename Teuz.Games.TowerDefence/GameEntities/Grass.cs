using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence.GameEntities
{
	class Grass : Sprite
	{
		public Grass(ICamera camera, IContentProvider contentProvider)
			: base(camera, contentProvider)
		{

		}

		public override void LoadContent()
		{
			IKernel kernel = NinjectFactory.Kernel;
			Random random = kernel.Get<Random>();

			SetTextureMap(ContentProvider.LoadTexture("Grass" + random.Next(1, 4)));
		}
	}
}
