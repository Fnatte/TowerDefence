using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.GameEntities
{
	public class Entity
	{
		private Tile tile;

		public Tile Tile
		{
			get { return tile; }
			set
			{
				if (tile == value) return;
				
				Tile old = tile;
				tile = value;

				if (old != null)
				{
					old.Entities.Remove(this);
				}

				if (tile != null)
				{
					tile.Entities.Add(this);
				}
			}
		}

		public bool Walkable { get; protected set; }
		public bool IsMoving { get; protected set; }

		public Entity()
		{
			Walkable = true;
		}

		public virtual void MoveTo(Tile tile)
		{
			if (!IsMoving) Tile = tile;
		}

		public virtual void Update(GameTime gameTime)
		{

		}
	}
}
