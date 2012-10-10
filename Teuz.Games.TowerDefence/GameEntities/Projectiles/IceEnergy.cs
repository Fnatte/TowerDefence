using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.GameEntities.Attackers;
using Ninject;
using Teuz.Games.TowerDefence.GameEntities.Attackers.Effects;

namespace Teuz.Games.TowerDefence.GameEntities.Projectiles
{
	class IceEnergy : Projectile
	{
		public IceEnergy(ICamera camera, IContentProvider content, IWorld world)
			: base(camera, content, world)
		{
			textureKeys = new string[] { "IceEnergy" };
			Velocity = new Vector2(0.2f);
			BaseDamage = 1;
			MaximumTravelDistance = 200;
		}

		protected override void OnHit(Attacker attacker)
		{
			// Get all attackers in the neighbor tiles also
			var attackers = World.Tiles.NeighborsTo(attacker.Tile)
				.SelectMany(x => x.Entities)
				.OfType<Attacker>()
				.Concat(new[] { attacker })
				.ToList();

			ApplyFreeze(attackers);

			base.OnHit(attacker);
		}

		protected virtual void ApplyFreeze(IEnumerable<Attacker> attackers)
		{
			foreach (var attacker in attackers)
			{
				var freezeEffect = NinjectFactory.Kernel.Get<FreezeEffect>();
				freezeEffect.Power += 0.1f * (Level-1);
				attacker.Effects.Add(freezeEffect);
			}
		}
	}
}
