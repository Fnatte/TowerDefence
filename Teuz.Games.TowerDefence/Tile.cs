using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.GameEntities;

namespace Teuz.Games.TowerDefence
{
	public class Tile
	{
		private readonly int x, y;
		public int X { get { return x; } }
		public int Y { get { return y; } }

		//private readonly List<Entity> entities = new List<Entity>();
		//public List<Entity> Entities { get { return entities; } }
		private readonly ObservableCollection<Entity> entities = new ObservableCollection<Entity>();
		public IList<Entity> Entities { get { return entities; } }

		public Tile(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public bool CanWalk()
		{
			return Entities.All(x => x.Walkable);
		}

		public Vector2 ToVector2()
		{
			return new Vector2(X, Y);
		}
	}
}
