using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace Teuz.Games.TowerDefence
{
	interface ICamera : IDrawable
	{
		Vector2 GetPosition(Tile tile);
        Tile GetTileFromPosition(Vector2 position);
		int GetTileSize();
	}
}
