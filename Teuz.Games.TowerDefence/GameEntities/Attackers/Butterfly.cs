using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Butterfly : Attacker
	{
		public Butterfly(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager)
			: base(camera, contentProvider, gameStateManager)
		{
			AnimationInterval = TimeSpan.FromMilliseconds(50);
			MaxHealth = 35;
			Health = MaxHealth;
			Worth = 50;
			MovementSpeed = 1.8f;
		}

		public override void LoadContent()
		{
			SetTextureAnimationMap(ContentProvider.LoadTextureAnimation("Butterfly*"));
		}
	}
}
