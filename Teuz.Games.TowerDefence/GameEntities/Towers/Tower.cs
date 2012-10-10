using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.GameEntities.Projectiles;
using Ninject;
using Teuz.Games.TowerDefence.PathFinding;

namespace Teuz.Games.TowerDefence.GameEntities.Towers
{
	class Tower : AnimatedSprite, IPlacable
	{
		public bool IsPlaced { get; private set; }
		protected IWorld World { get; private set; }
		protected Player Player { get; private set; }
		public event EventHandler Placed;
		public int TotalCost { get; protected set; }

		private int level;

		public int Level
		{
			get { return level; }
			set { level = value; OnLevelChanged(); }
		}

		private TowerAttribute towerAttribute;
		private PathProvider pathProvider;

		public Tower(ICamera camera, IContentProvider content, IWorld world, PathProvider pathProvider, Player player)
			: base(camera, content)
		{
			this.Player = player;
			this.World = world;
			this.pathProvider = pathProvider;
			this.Level = 1;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// if (!IsPlaced) return;
		}

		public bool TryPlace()
		{
			if (IsPlaced) throw new Exception("Tower cannot be placed multiple times.");

			// Test path
			pathProvider.FindPaths();
			if (pathProvider.TestAllPaths())
			{
				this.IsPlaced = true;
				OnPlaced();
				return true;
			}

			return false;
		}

		protected virtual void OnPlaced()
		{
			// Take cost money from player
			var attr = GetTowerAttribute();
			Player.Cash -= attr.Cost;
			TotalCost += attr.Cost;

			// Execute event
			EventHandler temp = Placed;
			if (temp != null)
			{
				temp(this, EventArgs.Empty);
			}
		}

		public virtual int GetUpgradeCost()
		{
			return TotalCost * 2 + (int)(20 * Math.Pow(Level, 2.6));
		}

		public virtual int GetSellPrice()
		{
			return TotalCost / 2;
		}

		public virtual void Upgrade()
		{
			int upgradeCost = GetUpgradeCost();
			if (Player.Cash >= upgradeCost)
			{
				Level++;
				Player.Cash -= upgradeCost;
			}
		}

		public virtual void Sell()
		{
			this.Tile = null;
			this.IsPlaced = false;
			this.Player.Cash += GetSellPrice();
		}

		public TowerAttribute GetTowerAttribute()
		{
			if(towerAttribute == null)
			{
				towerAttribute = (TowerAttribute)this.GetType().GetCustomAttributes(typeof(TowerAttribute), true).FirstOrDefault();
			}
			return towerAttribute;
		}

		protected virtual void OnLevelChanged()
		{
			
		}
	}
}
