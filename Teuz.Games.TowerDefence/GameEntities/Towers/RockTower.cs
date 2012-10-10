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
	[TowerAttribute("Rock Tower", 150)]
	class RockTower : FiringTower<Rock>
	{
		public RockTower(ICamera camera, IContentProvider contentProvider, IWorld world, PathProvider pathProvider, Player player)
			: base(camera, contentProvider, world, pathProvider, player)
		{
			textureKeys = new string[] { "RockTower" };
			Walkable = false;
			FireRange = 100;
			FireInterval = TimeSpan.FromSeconds(3.5);
		}

	}
}
