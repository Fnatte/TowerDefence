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
	[TowerAttribute("Block Tower", 25)]
	class BlockTower : Tower
	{
		public BlockTower(ICamera camera, IContentProvider contentProvider, IWorld world, PathProvider pathProvider, Player player)
			: base(camera, contentProvider, world, pathProvider, player)
		{
			textureKeys = new string[] { "BlockTower" };
			Walkable = false;
		}

	}
}
