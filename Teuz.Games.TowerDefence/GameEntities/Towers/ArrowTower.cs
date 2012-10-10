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
	[TowerAttribute("Arrow Tower", 90)]
	class ArrowTower : FiringTower<Arrow>
	{
		public ArrowTower(ICamera camera, IContentProvider contentProvider, IWorld world, PathProvider pathProvider, Player player)
			: base(camera, contentProvider, world, pathProvider, player)
		{
			textureKeys = new string[] { "ArrowTower" };
			Walkable = false;
			FireRange = 200;
		}

		protected override void OnLevelChanged()
		{
			base.OnLevelChanged();

			float s = MathUtil.Clamp(Level * 0.25f, 0, 1.8f);
			FireInterval = TimeSpan.FromSeconds(2) - TimeSpan.FromSeconds(s);
		}

	}
}
