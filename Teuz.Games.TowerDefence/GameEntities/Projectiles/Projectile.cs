using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.GameEntities.Attackers;

namespace Teuz.Games.TowerDefence.GameEntities.Projectiles
{
	class Projectile : AnimatedSprite
	{
		protected IWorld World { get; private set; }
		protected float FlatRotation { get; set; }

		public Vector2 Direction { get; set; }
		public Vector2 Velocity { get; set; }
		public int Damage { get; set; }
		public int BaseDamage { get; set; }

		public float TravelDistance { get; private set; }
		public float MaximumTravelDistance { get; set; }

		private int level;

		public int Level
		{
			get { return level; }
			set { level = value; OnLevelChanged(); }
		}


		public Projectile(ICamera camera, IContentProvider content, IWorld world) : base(camera, content)
		{
			this.DrawOrder = 10;
			this.World = world;
			this.Velocity = Vector2.One;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Move projectile and add to travel distance
			Vector2 moveVector = Direction * Velocity;
			PositionOffset += moveVector;
			TravelDistance += moveVector.Length();

			// If the projectile has moved longer than it is allowed to, remove it.
			if (MaximumTravelDistance != 0 && TravelDistance >= MaximumTravelDistance)
			{
				Tile = null;
				return;
			}

			// Change tile if the position offset is out of the current tile.
			ChangeTile();

			// Tile might be null if it is out of the map.
			// If so, just return.
			if (Tile == null) return;

			// See if we hit a attacker.
			var hit = GetHitAttacker();
			if (hit != null)
			{
				OnHit(hit);
				return;
			}

			// Rotate the sprite
			Rotate();
		}

		protected virtual void OnHit(Attacker attacker)
		{
			attacker.Health -= Damage;
			Destory();
		}

		protected virtual void OnLevelChanged()
		{
			Damage = DefaultDamangeScaling(BaseDamage, Level);
		}

		protected Attacker GetHitAttacker()
		{
			var pos = GetPosition();
			var attacker = World.Tiles.NeighborsTo(Tile)
				.Concat(new[] { Tile })
				.SelectMany(x => x.Entities.OfType<Attacker>())
				.FirstOrDefault(x => GetDestinationRectangle().Intersects(x.GetDestinationRectangle()));

			var tiles = World.Tiles.NeighborsTo(Tile);

			return attacker;
		}

		protected void ChangeTile()
		{
			int tileSize = Camera.GetTileSize();

			if (PositionOffset.X > tileSize)
			{
				PositionOffset = new Vector2(PositionOffset.X - tileSize, PositionOffset.Y);
				if (Tile.X + 1 < World.SizeX)
					Tile = World.Tiles.Get(Tile.X + 1, Tile.Y);
				else
					Tile = null;
			}
			else if (PositionOffset.X < tileSize)
			{
				PositionOffset = new Vector2(PositionOffset.X + tileSize, PositionOffset.Y);
				if (Tile.X - 1 >= 0)
					Tile = World.Tiles.Get(Tile.X - 1, Tile.Y);
				else
					Tile = null;
			}

			if (Tile == null) return;

			if (PositionOffset.Y > tileSize)
			{
				PositionOffset = new Vector2(PositionOffset.X, PositionOffset.Y - tileSize);
				if (Tile.Y + 1 < World.SizeY)
					Tile = World.Tiles.Get(Tile.X, Tile.Y + 1);
				else
					Tile = null;
			}
			else if (PositionOffset.Y < tileSize)
			{
				PositionOffset = new Vector2(PositionOffset.X, PositionOffset.Y + tileSize);
				if (Tile.Y - 1 >= 0)
					Tile = World.Tiles.Get(Tile.X, Tile.Y - 1);
				else
					Tile = null;
			}
		}

		protected virtual void Rotate()
		{
			Transform((float)Math.Atan2(Direction.Y, Direction.X) + FlatRotation, Vector2.One);
		}

		protected virtual void Destory()
		{
			Tile = null;
		}

		protected static int DefaultDamangeScaling(int damage, int level)
		{
			return damage + (int)((level-1) * Math.Pow(damage, 1.75));
		}
	}
}
