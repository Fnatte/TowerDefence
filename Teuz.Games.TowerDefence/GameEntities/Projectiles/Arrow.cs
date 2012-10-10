using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Teuz.Games.TowerDefence.GameEntities.Projectiles
{
	class Arrow : Projectile
	{
		public Arrow(ICamera camera, IContentProvider content, IWorld world)
			: base(camera, content, world)
		{
			textureKeys = new string[] { "Arrow" };
			Velocity = new Vector2(1.2f);
			FlatRotation = MathUtil.Pi / 2;
			BaseDamage = 3;
		}
	}
}
