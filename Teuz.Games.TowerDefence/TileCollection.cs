using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.GameEntities;

namespace Teuz.Games.TowerDefence
{
	public class TileCollection
	{
		private readonly Tile[,] tiles;

		private readonly int sizeX, sizeY;
		public int SizeX { get { return sizeX; } }
		public int SizeY { get { return sizeY; } }

		public TileCollection(int sizeX, int sizeY)
		{
			this.sizeX = sizeX;
			this.sizeY = sizeY;

			tiles = new Tile[SizeX, SizeY];

			for (int x = 0; x < SizeX; x++)
			{
				for (int y = 0; y < SizeY; y++)
				{
					tiles[x, y] = new Tile(x, y);
				}
			}
		}

		public Tile Get(int x, int y)
		{
			return tiles[x, y];
		}

		public IEnumerable<Tile> NeighborsTo(Tile tile)
		{
			for (int x = tile.X > 0 ? tile.X - 1 : 0; x <= tile.X + 1 && x < SizeX; x++)
			{
				for (int y = tile.Y > 0 ? tile.Y - 1 : 0; y <= tile.Y + 1 && y < SizeY; y++)
				{
					if (tile.X == x && tile.Y == y) continue;
					yield return Get(x,y);
				}
			}
		}

		protected class Node
		{
			public Tile Tile;
			public Node Next, Previous;

			public Node()
			{
				
			}
		}

	}
}
