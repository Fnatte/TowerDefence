using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.GameEntities;
using Teuz.Games.TowerDefence.PathFinding;

namespace Teuz.Games.TowerDefence
{
	class PathProvider
	{
		private readonly Pathfinder pathfinder;
		private readonly Dictionary<Spawn, IList<Tile>> paths = new Dictionary<Spawn, IList<Tile>>();
		private readonly IWorld world;

		public PathProvider(IWorld world)
		{
			this.world = world;
			pathfinder = new Pathfinder(world);
		}

		public void FindPaths()
		{
			paths.Clear();

			// Find the arrival
			var arrival = world.Entities().OfType<Arrival>().FirstOrDefault(x => x.Direction == DirectionType.Middle);
			if (arrival == null) throw new Exception("Could not find arrival!");

			// Find spawns
			var spawns = world.Entities().OfType<Spawn>();

			// Find a path for each spawn
			foreach (Spawn spawn in spawns)
			{
				paths.Add(spawn, pathfinder.FindPath(spawn.Tile, arrival.Tile));
			}
		}

		public IEnumerable<IList<Tile>> Paths()
		{
			foreach (var path in paths)
				yield return path.Value;
		}

		public IList<Tile> PathBySpawn(Spawn spawn)
		{
			return paths[spawn];
		}

		public bool TestAllPaths()
		{
			return paths.All(x => x.Value != null);
		}
	}
}
