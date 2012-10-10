using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.GameEntities.Attackers;

namespace Teuz.Games.TowerDefence.GameEntities.Projectiles
{
	class Rock : Projectile
	{
		public Rock(ICamera camera, IContentProvider content, IWorld world)
			: base(camera, content, world)
		{
			textureKeys = new string[] { "Rock" };
			Velocity = new Vector2(0.5f);
			BaseDamage = 12;
		}

		protected override void OnHit(Attacker attacker)
		{
			// Get all attackers in the neighbor tiles
			var attackers = World.Tiles.NeighborsTo(attacker.Tile)
				.SelectMany(x => x.Entities)
				.OfType<Attacker>()
				.ToList();

			// Deals these attackers some splash damage
			foreach (var a in attackers)
				a.Health -= (int)(Damage * .4f);

			base.OnHit(attacker);
		}
	}
}
