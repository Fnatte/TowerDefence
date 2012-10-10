using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Teuz.Games.TowerDefence.GameEntities.Attackers.Effects;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.GameEntities.Attackers
{
	class Attacker : AnimatedSprite
	{
		public IList<Tile> Path { get; set; }
		private int pathIndex = 0;

		private int health;

		public int Health
		{
			get { return health; }
			set
			{
				health = value;
				OnHealthChanged();
			}
		}

		private int maxHealth;

		public int MaxHealth
		{
			get { return maxHealth; }
			set
			{
				maxHealth = value;
				OnMaxHealthChanged();
			}
		}

		/// <summary>
		/// Amount of cash to reward player for a kill.
		/// </summary>
		public int Worth { get; protected set; }

		private float rotation;

		private readonly ObservableCollection<Effect> effects = new ObservableCollection<Effect>();
		public ObservableCollection<Effect> Effects { get { return effects; } }

		protected GameStateManager GameStateManager { get; private set; }

		public Attacker(ICamera camera, IContentProvider contentProvider, GameStateManager gameStateManager) : base(camera, contentProvider)
		{
			DrawOrder = 5;
			GameStateManager = gameStateManager;
			Health = 1;

			effects.CollectionChanged += effects_CollectionChanged;
		}

		void effects_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch(e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (var effect in e.NewItems.Cast<Effect>())
					{
						effect.Attacker = this;

						// If there was a effect of this type already and it's not stackable,
						// we remove it.
						var previousNonStackable = Effects.FirstOrDefault(x =>
							x.GetType() == effect.GetType() &&
							x != effect &&
							!x.StacksWith(effect));
						if (previousNonStackable != null)
						{
							previousNonStackable.Remove();
							Effects.Remove(previousNonStackable);
						}
					}
					break;
				case NotifyCollectionChangedAction.Remove:
				case NotifyCollectionChangedAction.Reset:
					foreach (var effect in e.OldItems.Cast<Effect>())
					{
						if (effect.Attacker == this) effect.Attacker = null;
					}
					break;
			}
		}

		protected virtual void OnMaxHealthChanged()
		{
			if (MaxHealth < 0) maxHealth = 0;
			if (Health > MaxHealth)
			{
				health = MaxHealth;
			}
		}

		protected virtual void OnHealthChanged()
		{
			if (Health <= 0) Kill();
			else if (Health > MaxHealth)
			{
				health = MaxHealth;
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Tile == null) return;

			// Movement
			if (!IsMoving)
			{
				if (Tile != Path.Last())
				{
					pathIndex++;
					MoveTo(Path[pathIndex]);
				}
				else
				{
					// Last tile reached
					var defendingState = GameStateManager.CurrentState as DefendingState;
					if (defendingState != null)
					{
						defendingState.Health--;
						Worth = 0;
						Kill();
						return;
					}
				}
			}

			// Effects
			for(int i = 0; i < Effects.Count; i++)
			{
				Effects[i].Update(gameTime);

				// Remove effects that has ended
				if (Effects[i].Ended)
				{
					Effects.RemoveAt(i);
					i--;
				}
			}

			// Update rotation
			Transform(rotation, Vector2.One);
		}

		public override void MoveTo(Tile tile)
		{
			var tileA = Tile;
			base.MoveTo(tile);
			var tileB = Tile;

			if (tileA != tileB)
			{
				int diffX = tileB.X - tileA.X;
				if (diffX != 0)
				{
					if (diffX > 0)
					{
						rotation = 0;
					}
					else if (diffX < 0)
					{
						rotation = MathUtil.Pi;
					}
				}
				else
				{
					int diffY = tileB.Y - tileA.Y;
					if (diffY > 0)
					{
						rotation = MathUtil.PiOverTwo;
					}
					else if (diffY < 0)
					{
						rotation = MathUtil.PiOverTwo * 3;
					}
				}
			}
		}

		public bool IsDead()
		{
			return Health == 0;
		}

		public virtual void Kill()
		{
			this.Tile = null;
			health = 0;

			var defendingState = GameStateManager.CurrentState as DefendingState;
			if (defendingState != null)
			{
				defendingState.EarnedCash += Worth;
			}
		}
	}
}
