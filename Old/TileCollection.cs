using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.GameEntities;

namespace Teuz.Games.TowerDefence
{
	public class TileCollection
	{
		private Node[,] nodes;
		private Node first, last;
		private readonly List<Node> levels = new List<Node>();

		private readonly int sizeX, sizeY;
		public int SizeX { get { return sizeX; } }
		public int SizeY { get { return sizeY; } }

		public TileCollection(int sizeX, int sizeY)
		{
			this.sizeX = sizeX;
			this.sizeY = sizeY;

			nodes = new Node[SizeX, SizeY];

			Node prev = null;
			Node cur = null;

			for (int x = 0; x < SizeX; x++)
			{
				for (int y = 0; y < SizeY; y++)
				{
					cur = nodes[x, y] = new Node()
					{
						Tile = new Tile(x, y),
						Previous = prev,
					};

					cur.Tile.EntitiesChanged += Tile_EntitiesChanged;

					if (prev != null) prev.Next = cur;
					prev = cur;
				}
			}

			first = nodes[0, 0];
			last = nodes[SizeX - 1, SizeY - 1];

			levels.Insert(0, first);
		}

		public IEnumerable<Tile> ByZOrder()
		{
			Node node = first;
			while (node != null)
			{
				yield return node.Tile;
				node = node.Next;
			}
		}

		public IEnumerable<Tile> ByZOrderDesc()
		{
			Node node = last;
			while (node != null)
			{
				yield return node.Tile;
				node = node.Previous;
			}
		}

		void Tile_EntitiesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Tile tile = (Tile)sender;
			Node node = nodes[tile.X, tile.Y];
			if (node.Tile != tile) throw new Exception("Unexpected behaviour in TileCollection!");
			int count = tile.Entities.Count;

			bool movedUp = false, movedDown = false;
			while (node.Next != null && node.Next.Tile.Entities.Count < count)
			{
				MoveUp(node);
				movedUp = true;
			}
			while (node.Previous != null && node.Previous.Tile.Entities.Count > count)
			{
				MoveUp(node.Previous);
				movedDown = true;
			}

			if (movedUp || movedDown)
			{
				if (movedUp)
				{
					if (count < levels.Count)
					{
						levels[count] = node;
					}
					else
					{
						levels.Add(node);
					}
				}
			}
		}

		protected void MoveUp(Node node1)
		{
			if (node1.Next != null)
			{
				Node node2 = node1.Next;
				Node left = node1.Previous;
				Node right = node2.Next;

				node2.Previous = left;
				node2.Next = node1;

				node1.Previous = node2;
				node1.Next = right;

				if (left != null) left.Next = node2;
				if (right != null) right.Previous = node1;

				while (first.Previous != null) first = first.Previous;
				while (last.Next != null) last = last.Next;
			}
		}

		public Tile Get(int x, int y)
		{
			return nodes[x, y].Tile;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("TileCollection:");
			Node node = first;
			while (node != null)
			{
				sb.AppendFormat("\t{0} {1} {2}", node.Tile.X, node.Tile.Y, node.Tile.Entities.Count);
				sb.AppendLine();
				node = node.Next;
			}
			return sb.ToString();
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
