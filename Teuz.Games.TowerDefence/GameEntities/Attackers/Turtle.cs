using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Turtle : Attacker
	{
		public Turtle(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager)
			: base(camera, contentProvider, gameStateManager)
		{
			AnimationInterval = TimeSpan.FromMilliseconds(200);
			MaxHealth = 200;
			Health = MaxHealth;
			Worth = 50;
			MovementSpeed = 0.5f;
		}

		public override void LoadContent()
		{
			SetTextureAnimationMap(ContentProvider.LoadTextureAnimation("Turtle*"));
		}
	}
}
