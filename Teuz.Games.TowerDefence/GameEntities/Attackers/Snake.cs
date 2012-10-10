using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Snake : Attacker
	{
		public Snake(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager) : base(camera, contentProvider, gameStateManager)
		{
			AnimationInterval = TimeSpan.FromMilliseconds(90);
			MaxHealth = 8;
			Health = MaxHealth;
			Worth = 15;
			MovementSpeed = 1.65f;
		}

		public override void LoadContent()
		{
			SetTextureAnimationMap(ContentProvider.LoadTextureAnimation("Snake*"));
		}
	}
}
