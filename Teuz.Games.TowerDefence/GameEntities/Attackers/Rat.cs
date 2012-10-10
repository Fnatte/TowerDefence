using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Rat : Attacker
	{
		public Rat(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager) : base(camera, contentProvider, gameStateManager)
		{
			AnimationInterval = TimeSpan.FromMilliseconds(250);
			MaxHealth = 6;
			Health = MaxHealth;
			Worth = 13;
			MovementSpeed = 0.6f;
		}

		public override void LoadContent()
		{
			for (int i = 1; i <= 2; i++)
			{
				SetTextureMap(ContentProvider.LoadTexture("Rat" + i.ToString()));
				SourceRectangles.Add(SourceRectangle.Value);
			}

			SourceRectangle = SourceRectangles.First();
		}
	}
}
