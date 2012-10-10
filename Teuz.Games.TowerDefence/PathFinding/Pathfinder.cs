using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.PathFinding
{
	sealed class Pathfinder
	{
		private readonly List<SearchNode> openList = new List<SearchNode>();
		private readonly List<SearchNode> closedList = new List<SearchNode>();
		private readonly IWorld world;
		private SearchNode[,] searchNodes;

		public Pathfinder(IWorld world)
		{
			this.world = world;
			this.searchNodes = new SearchNode[world.Tiles.SizeX, world.Tiles.SizeY];
		}

		public IList<Tile> FindPath(Tile startTile, Tile endTile)
		{
			if (startTile == endTile) return new List<Tile>();

			Reset();

			SearchNode startNode = searchNodes[startTile.X, startTile.Y];
			SearchNode endNode = searchNodes[endTile.X, endTile.Y];

			startNode.InOpenList = true;
			startNode.DistanceToGoal = Heuristic(startTile, endTile);
			startNode.DistanceTraveled = 0;

			openList.Add(startNode);

			while (openList.Count > 0)
			{
				SearchNode currentNode = FindBestNode();

				if (currentNode == null) break;
				if (currentNode == endNode) return FindFinalPath(startNode, endNode);

				for (int i = 0; i < currentNode.Neighbors.Length; i++)
				{
					SearchNode neighbor = currentNode.Neighbors[i];

					if (neighbor == null || neighbor.Walkable == false) continue;

					float distanceTraveled = currentNode.DistanceTraveled + 1;
					float heuristic = Heuristic(neighbor.Tile, endTile);

					if (neighbor.InOpenList == false && neighbor.InClosedList == false)
					{
						neighbor.DistanceTraveled = distanceTraveled;
						neighbor.DistanceToGoal = distanceTraveled + heuristic;
						neighbor.Parent = currentNode;
						neighbor.InOpenList = true;
						openList.Add(neighbor);
					}
					else if (neighbor.InOpenList || neighbor.InClosedList)
					{
						if (neighbor.DistanceTraveled > distanceTraveled)
						{
							neighbor.DistanceTraveled = distanceTraveled;
							neighbor.DistanceToGoal = distanceTraveled + heuristic;
							neighbor.Parent = currentNode;
						}
					}
				}

				openList.Remove(currentNode);
				closedList.Add(currentNode);
				currentNode.InOpenList = false;
				currentNode.InClosedList = true;
			}

			return null;
		}

		private int Heuristic(Tile tile1, Tile tile2)
		{
			return Math.Abs(tile1.X - tile2.X) + Math.Abs(tile1.Y - tile2.Y);
		}

		private void Reset()
		{
			openList.Clear();
			closedList.Clear();

			for (int x = 0; x < world.Tiles.SizeX; x++)
			{
				for (int y = 0; y < world.Tiles.SizeY; y++)
				{
					SearchNode node = searchNodes[x, y];

					if (node == null)
					{
						node = searchNodes[x, y] = new SearchNode();
					}

					node.InOpenList = false;
					node.InClosedList = false;
					node.DistanceToGoal = float.MaxValue;
					node.DistanceTraveled = float.MaxValue;
					node.Tile = world.Tiles.Get(x, y);
					node.Neighbors = new SearchNode[4];
					node.Walkable = node.Tile.CanWalk();
					node.Parent = null;
				}
			}

			for (int x = 0; x < world.Tiles.SizeX; x++)
			{
				for (int y = 0; y < world.Tiles.SizeY; y++)
				{
					SearchNode node = searchNodes[x, y];
					if (x > 0) node.Neighbors[0] = searchNodes[x - 1, y];
					if (x + 1 < world.Tiles.SizeX) node.Neighbors[1] = searchNodes[x + 1, y];
					if (y > 0) node.Neighbors[2] = searchNodes[x, y - 1];
					if (y + 1 < world.Tiles.SizeY) node.Neighbors[3] = searchNodes[x, y + 1];
				}
			}
		}

		private SearchNode FindBestNode()
		{
			SearchNode current = openList[0];
			float smallestDistanceToGoal = float.MaxValue;

			for (int i = 0; i < openList.Count; i++)
			{
				if (openList[i].DistanceToGoal < smallestDistanceToGoal)
				{
					current = openList[i];
					smallestDistanceToGoal = current.DistanceToGoal;
				}
			}

			return current;
		}

		private IList<Tile> FindFinalPath(SearchNode startNode, SearchNode endNode)
		{
			List<Tile> tiles = new List<Tile>();
			tiles.Add(endNode.Tile);

			SearchNode parent = endNode.Parent;

			while (parent != startNode)
			{
				tiles.Add(parent.Tile);
				parent = parent.Parent;
			};

			tiles.Add(startNode.Tile);

			tiles.Reverse();
			return tiles;
		}
	}
}
