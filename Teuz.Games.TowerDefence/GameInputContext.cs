using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ninject;
using SharpDX.Direct2D1;
using Teuz.Games.TowerDefence.GameEntities;
using Teuz.Games.TowerDefence.GameEntities.Towers;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence
{
	class GameInputContext : IGameModel
	{
		private GameInputContextState state;

		public GameInputContextState State
		{
			get { return state; }
			set
			{
				state = value;
				OnStateChanged();
			}
		}

		private InputManager input;
		private ICamera camera;
		private GameStateManager gameStateManager;
		private IContentProvider contentProvider;

		private Sprite currentSprite;

		public Sprite CurrentSprite
		{
			get { return currentSprite; }
			set { currentSprite = value; OnCurrentSpriteChanged(); }
		}

		public RenderTarget RenderTarget { get; private set; }

		public event EventHandler CurrentSpriteChanged;
		public event EventHandler StateChanged;

		private TextureMap textureSelected;

		private Sprite selectedSprite;

		public Sprite SelectedSprite
		{
			get { return selectedSprite; }
			set { selectedSprite = value; }
		}


		public GameInputContext(InputManager inputManager, ICamera camera, GameStateManager gameStateManager, RenderTarget renderTarget, IContentProvider contentProvider)
		{
			this.contentProvider = contentProvider;
			this.input = inputManager;
			this.camera = camera;
			this.gameStateManager = gameStateManager;
			this.RenderTarget = renderTarget;

			Console.WriteLine("GameInputContext constructed.");
		}

		protected virtual void OnStateChanged()
		{
			EventHandler temp = StateChanged;
			if (temp != null)
			{
				temp(this, EventArgs.Empty);
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			if (gameStateManager.CurrentState is ShoppingState)
			{
				SelectedSprite = CurrentSprite;

				switch (State)
				{
					case GameInputContextState.None:
						UpdateNone(gameTime);
						break;
					case GameInputContextState.Placing:
						UpdatePlacing(gameTime);
						break;
				}
			}
			else
			{
				SelectedSprite = null;
			}
		}

		protected virtual void UpdateNone(GameTime gameTime)
		{
			if (input.MouseReleased.HasFlag(MouseButtons.Left))
			{
				var tile = camera.GetTileFromPosition(input.MousePoint.ToVector2());
				if (tile != null)
				{
					CurrentSprite = (Tower)tile.Entities.FirstOrDefault(x => x is Tower);
				}
			}
		}

		protected virtual void UpdatePlacing(GameTime gameTime)
		{
			UpdateCurrentSprite(gameTime);

			if (input.MouseReleased.HasFlag(MouseButtons.Left) && CurrentSprite.Tile != null)
			{
				var placable = CurrentSprite as IPlacable;
				if (placable != null)
				{
					if (placable.TryPlace())
					{
						State = GameInputContextState.None;
					}
				}
				else
				{
					CurrentSprite = null;
					State = GameInputContextState.None;
				}
			}
			else if (input.MouseReleased.HasFlag(MouseButtons.Right))
			{
				CurrentSprite.Tile = null;
				CurrentSprite = null;
				State = GameInputContextState.None;
			}
		}

		protected virtual void UpdateCurrentSprite(GameTime gameTime)
		{
			if (CurrentSprite == null) return;

			var tile = camera.GetTileFromPosition(input.MousePoint.ToVector2());
			if(tile == null || tile.CanWalk())
				CurrentSprite.Tile = tile;
		}

		protected virtual void OnCurrentSpriteChanged()
		{
			EventHandler temp = CurrentSpriteChanged;
			if (temp != null)
			{
				temp(this, EventArgs.Empty);
			}
		}

		public void Initialize()
		{
			
		}

		public void LoadContent()
		{
			textureSelected = contentProvider.LoadTexture("Selected");
		}

		public void UnloadContent()
		{
			
		}

		public void Draw(GameTime gameTime)
		{
			if (SelectedSprite != null && SelectedSprite.Tile != null)
			{
				var dest = SelectedSprite.GetDestinationRectangle();
				RenderTarget.DrawBitmap(textureSelected.Bitmap, dest, 1.0f, BitmapInterpolationMode.Linear, textureSelected.SourceRectangle);
			}
		}
	}

	enum GameInputContextState
	{
		None,
		Placing
	}
}
