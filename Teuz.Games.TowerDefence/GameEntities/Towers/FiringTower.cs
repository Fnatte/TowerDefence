using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Parameters;
using SharpDX;
using Teuz.Games.TowerDefence.GameEntities.Attackers;
using Teuz.Games.TowerDefence.GameEntities.Projectiles;

namespace Teuz.Games.TowerDefence.GameEntities.Towers
{
	class FiringTower<T> : Tower where T : Projectile
	{
		private TimeSpan timer;
		public TimeSpan FireInterval { get; protected set; }
		public float FireRange { get; set; }

		public FiringTower(ICamera camera, IContentProvider contentProvider, IWorld world, PathProvider pathProvider, Player player)
			: base(camera, contentProvider, world, pathProvider, player)
		{
			this.FireInterval = TimeSpan.FromSeconds(2);
			this.FireRange = 100;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (!IsPlaced) return;

			timer += gameTime.Elapsed;
			if (timer >= FireInterval)
			{
				timer = TimeSpan.Zero;
				Fire();
			}
		}

		protected virtual void Fire()
		{
			// Find target
			var attacker = FindClosestTarget();
			if (attacker != null && Vector2.Distance(GetPosition(), attacker.GetPosition()) <= FireRange)
			{
				var arrow = NinjectFactory.Kernel.Get<T>();
				arrow.Level = Level;
				arrow.Initialize();
				arrow.LoadContent();
				arrow.Direction = Vector2.Normalize(attacker.Tile.ToVector2() - Tile.ToVector2());
				arrow.Tile = Tile;
			}
			else
			{
				// Because we did not fire, we make sure the timer
				// only needs another 200 ms to search for a target again.
				timer = FireInterval - TimeSpan.FromMilliseconds(200);
			}
		}

		protected virtual Attacker FindClosestTarget()
		{
			var pos = GetPosition();
			return World.Sprites()
				.OfType<Attacker>()
				.OrderBy(x => Vector2.Distance(pos, x.GetPosition()))
				.FirstOrDefault();
		}
	}
}
