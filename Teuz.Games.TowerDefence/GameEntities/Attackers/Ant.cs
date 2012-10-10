using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Ant : Attacker
	{
		public Ant(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager) : base(camera, contentProvider, gameStateManager)
		{
			AnimationInterval = TimeSpan.FromMilliseconds(160);
			MaxHealth = 30;
			Health = MaxHealth;
			Worth = 25;
			MovementSpeed = 1.2f;
		}

		public override void LoadContent()
		{
			SetTextureAnimationMap(ContentProvider.LoadTextureAnimation("Ant*"));
		}
	}
}
