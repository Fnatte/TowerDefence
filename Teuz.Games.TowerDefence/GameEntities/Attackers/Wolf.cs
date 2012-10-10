using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Wolf : Attacker
	{
		public Wolf(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager)
			: base(camera, contentProvider, gameStateManager)
		{
			AnimationInterval = TimeSpan.FromMilliseconds(150);
			MaxHealth = 9;
			Health = MaxHealth;
			Worth = 22;
			MovementSpeed = 1.35f;
		}

		public override void LoadContent()
		{
			SetTextureMap(ContentProvider.LoadTexture("Wolf1"));
			SourceRectangles.Add(SourceRectangle.Value);

			SetTextureMap(ContentProvider.LoadTexture("Wolf2"));
			SourceRectangles.Add(SourceRectangle.Value);

			SourceRectangle = SourceRectangles.First();
		}
	}
}
