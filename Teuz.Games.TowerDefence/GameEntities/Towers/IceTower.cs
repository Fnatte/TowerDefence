using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SharpDX;
using Teuz.Games.TowerDefence.GameEntities.Projectiles;

namespace Teuz.Games.TowerDefence.GameEntities.Towers
{
	[TowerAttribute("Ice Tower", 230)]
	class IceTower : FiringTower<IceEnergy>
	{
		public IceTower(ICamera camera, IContentProvider contentProvider, IWorld world, PathProvider pathProvider, Player player)
			: base(camera, contentProvider, world, pathProvider, player)
		{
			textureKeys = new string[] { "IceTower" };
			Walkable = false;
			FireRange = 100;
		}

	}
}
