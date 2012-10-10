using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Teuz.Games.TowerDefence.GameEntities;

namespace Teuz.Games.TowerDefence.Test
{
	[TestClass]
	public class TileCollectionTest
	{
		private readonly Random random = new Random();

		private TileCollection GenerateTiles(int sizeX, int sizeY, int count)
		{
			TileCollection collection = new TileCollection(sizeX, sizeY);

			for (int n = 0; n < count; n++)
			{
				int x = random.Next(sizeX);
				int y = random.Next(sizeY);
				Tile tile = collection.Get(x, y);
				tile.Entities.Add(new GameEntities.Entity() { Tile = tile });
			}

			return collection;
		}

		/*
		[TestMethod]
		public void TestLinkedListOrder()
		{
			var col = GenerateTiles(20, 20, 1000);
			int z = 0;
			foreach (var tile in col.ByZOrder())
			{
				int c= tile.Entities.Count;
				Assert.IsTrue(c >= z);
				z = c;
			}

			z = int.MaxValue;
			foreach (var tile in col.ByZOrderDesc())
			{
				int c = tile.Entities.Count;
				Assert.IsTrue(c <= z);
				z = c;
			}
		}
		 * */
	}
}
