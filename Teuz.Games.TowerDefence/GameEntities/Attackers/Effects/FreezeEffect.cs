using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers.Effects
{
	class FreezeEffect : Effect
	{
		private float power;

		public float Power
		{
			get { return power; }
			set { power = MathUtil.Clamp(value, 0.05f, 0.95f); }
		}


		private float slowAmount;

		public FreezeEffect()
		{
			this.Power = 0.22f;
			this.Lifetime = TimeSpan.FromMilliseconds(3000);
		}

		public override bool StacksWith(Effect effect)
		{
			return !(effect is FreezeEffect);
		}

		protected override void BeginEffect()
		{
			base.BeginEffect();

			float prev = Attacker.MovementSpeed;

			if (Attacker.MovementSpeed * Power < 0.3f)
			{
				Attacker.MovementSpeed = 0.3f;
			}
			else
			{
				Attacker.MovementSpeed *= Power;
			}

			slowAmount = prev - Attacker.MovementSpeed;
		}

		protected override void EndEffect()
		{
			base.EndEffect();

			Attacker.MovementSpeed += slowAmount;
		}
	}
}
