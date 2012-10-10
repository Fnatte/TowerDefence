using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Bear : Attacker
	{
		public Bear(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager)
			: base(camera, contentProvider, gameStateManager)
		{
			AnimationInterval = TimeSpan.FromMilliseconds(170);
			MaxHealth = 250;
			Health = MaxHealth;
			Worth = 70;
			MovementSpeed = 1.3f;
		}

		public override void LoadContent()
		{
			SetTextureAnimationMap(ContentProvider.LoadTextureAnimation("Bear*"));
		}
	}
}
