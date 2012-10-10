using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.GameEntities;

namespace Teuz.Games.TowerDefence
{
	class World : IWorld
	{
		private readonly TileCollection tiles;
		public TileCollection Tiles { get { return tiles; } }

		public int SizeX { get { return tiles.SizeX; } }
		public int SizeY { get { return tiles.SizeY; } }

		public World()
		{
			this.tiles = new TileCollection(35, 22);

			Console.WriteLine("World constructed.");
		}

		public IEnumerable<Sprite> Sprites()
		{
			foreach (var entity in Entities())
			{
				//Sprite sprite = entity as Sprite;
				//if (sprite != null) yield return sprite;
				if(entity is Sprite) yield return (Sprite)entity;
			}
		}

		public IEnumerable<Entity> Entities()
		{
			List<Entity> temp = new List<Entity>();
			for (int x = 0; x < SizeX; x++)
			{
				for (int y = 0; y < SizeY; y++)
				{
					temp.Clear();
					foreach (var entity in Tiles.Get(x, y).Entities)
					{
						temp.Add(entity);
					}

					foreach (var entity in temp)
					{
						yield return entity;
					}
				}
			}
		}
	}
}
