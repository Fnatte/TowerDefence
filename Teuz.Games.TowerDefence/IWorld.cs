using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.GameEntities;

namespace Teuz.Games.TowerDefence
{
	interface IWorld
	{
		TileCollection Tiles { get; }
		int SizeX { get; }
		int SizeY { get; }

		IEnumerable<Sprite> Sprites();
		IEnumerable<Entity> Entities();
	}
}
