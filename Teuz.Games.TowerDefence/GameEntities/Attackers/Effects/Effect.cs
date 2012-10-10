using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers.Effects
{
	abstract class Effect
	{
		public Attacker Attacker { get; set; }
		public TimeSpan Lifetime { get; set; }
		public TimeSpan Elapsed { get; private set; }
		public bool Enabled { get; set; }
		public bool Ended { get; set; }

		private bool hasBegun;

		public Effect()
		{
			Enabled = true;
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!Enabled) return;

			if (!hasBegun) 
			{
				hasBegun = true;
				BeginEffect();
			}

			Elapsed += gameTime.Elapsed;
			if (Elapsed >= Lifetime)
			{
				Remove();
				return;
			}
		}

		public virtual bool StacksWith(Effect effect)
		{
			return false;
		}

		protected virtual void BeginEffect()
		{

		}

		protected virtual void EndEffect()
		{

		}

		public virtual void Remove()
		{
			Enabled = false;
			Ended = true;
			EndEffect();
		}
	}
}
