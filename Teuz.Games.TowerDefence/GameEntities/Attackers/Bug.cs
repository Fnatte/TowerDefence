using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Bug : Attacker
	{
		public Bug(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager) : base(camera, contentProvider, gameStateManager)
		{
			AnimationInterval = TimeSpan.FromMilliseconds(120);
			MaxHealth = 2;
			Health = MaxHealth;
			Worth = 7;
			MovementSpeed = 0.7f;
		}

		public override void LoadContent()
		{
			for (int i = 1; i < 4; i++)
			{
				SetTextureMap(ContentProvider.LoadTexture("Bug" + i.ToString()));
				SourceRectangles.Add(SourceRectangle.Value);
			}

			SourceRectangle = SourceRectangles.First();
		}
	}
}
