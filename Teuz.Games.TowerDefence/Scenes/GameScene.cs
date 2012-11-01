using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SharpDX;
using SharpDX.Direct2D1;
using Teuz.Games.TowerDefence.GameEntities;
using Teuz.Games.TowerDefence.GameEntities.Towers;
using Teuz.Games.TowerDefence.Levels;
using Teuz.Games.TowerDefence.States;
using Teuz.Games.TowerDefence.UI;
using FactoryDWrite = SharpDX.DirectWrite.Factory;

namespace Teuz.Games.TowerDefence.Scenes
{
	class GameScene : Scene, ICamera, ITextRenderer
	{
		public World World { get; private set; }
		private const int tileSize = 32;
		private const int offsetX = 5;
		private const int offsetY = 7;
		private readonly Random random = new Random();
		private IKernel kernel;
		private RectangleF backgroundRectangle;
		private SolidColorBrush backgroundBrush;
		public GameInputContext InputContext { get; private set; }
		public SpawnProvider SpawnProvider { get; private set; }

		private RootControl rootControl;
		private TowerPanelItem towerPanelItem;

		public Spawn SpawnLeft { get; private set; }
		public Spawn SpawnRight { get; private set; }
		public Arrival Arrival { get; private set; }

		public GameStateManager GameStateManager { get; private set; }

		private LevelProvider levelProvider;
		private Player player;

		// public RenderTarget RenderTarget { get { return GameWindow.RenderTarget2D; } }

		public GameScene(IGraphicsRenderer graphicsRenderer, ITextRenderer textRenderer, LevelProvider levelProvider, Player player, GameStateManager gameStateManager)
			: base(graphicsRenderer, textRenderer)
		{
			kernel = NinjectFactory.Kernel;
			this.levelProvider = levelProvider;
			this.player = player;
			this.GameStateManager = gameStateManager;
			World = new World();
			GameStateManager.StateChanged += GameStateManager_StateChanged;

			Console.WriteLine("WorldScene constructed.");
		}

		void GameStateManager_StateChanged(object sender, EventArgs e)
		{
			if (GameStateManager.CurrentState is DefendingState)
			{
				// See if there is another level
				if (player.CurrentLevelId <= levelProvider.GetLastLevelId())
				{
					// Load new level into SpawnProvider
					Level level = levelProvider.Get(player.CurrentLevelId);
					SpawnProvider.Load(level);
				}
				else
				{
					// There is no other level, goto end credits state
					GameStateManager.ReplaceState<EndCreditsState>();
				}
			}
		}

		public override void Initialize()
		{
			InputContext = kernel.Get<GameInputContext>();
			InputContext.CurrentSpriteChanged += InputContext_CurrentSpriteChanged;
			InputContext.StateChanged += InputContext_StateChanged;

			InitializePanel();

			CreateGrass();
			CreateArrival();
			CreateSpawn();
			CreateStones();

			InitializeSpawnProvider();

			backgroundRectangle = new RectangleF(0, 0, tileSize * World.SizeX + 2 * offsetX, tileSize * World.SizeY + 2 * offsetY + 1);

			rootControl.Position = new Vector2(backgroundRectangle.Width, 0);
			rootControl.Width = GraphicsRenderer.Width - (int)backgroundRectangle.Width;
			rootControl.Height = GraphicsRenderer.Height;
			rootControl.Initialize();

			foreach (Sprite sprite in World.Sprites()) sprite.Initialize();
		}

		void InputContext_StateChanged(object sender, EventArgs e)
		{
			UpdateTowerPanel();
		}

		void InputContext_CurrentSpriteChanged(object sender, EventArgs e)
		{
			UpdateTowerPanel();
		}

		private void UpdateTowerPanel()
		{
			if (GameStateManager.CurrentState is ShoppingState &&
				InputContext.State == GameInputContextState.None)
			{
				if (InputContext.CurrentSprite != null)
				{
					((TowerControl)towerPanelItem.Content).Tower = (Tower)InputContext.CurrentSprite;
					towerPanelItem.Enabled = true;
					towerPanelItem.Visible = true;
				}
				else
				{
					towerPanelItem.Enabled = false;
					towerPanelItem.Visible = false;
				}
			}
		}

		private void InitializePanel()
		{
			// Create panel and add some panel items
			var panel = kernel.Get<Panel>();
			panel.Children.Add(kernel.Get<InfoPanelItem>());
			panel.Children.Add(kernel.Get<TowerShopPanelItem>());
			panel.Children.Add(kernel.Get<LevelControlPanelItem>());
			
			// TowerPanel
			panel.Children.Add((towerPanelItem = kernel.Get<TowerPanelItem>()));

			// Create root control
			rootControl = kernel.Get<RootControl>();
			rootControl.Children.Add(panel);
		}

		private void InitializeSpawnProvider()
		{
			SpawnProvider = kernel.Get<SpawnProvider>();

			SpawnProvider.Spawns.Add(SpawnLeft);
			SpawnProvider.Spawns.Add(SpawnRight);
		}

		public override void LoadContent()
		{
			backgroundBrush = new SolidColorBrush(RenderTarget, Color.Black);
			foreach(var sprite in World.Sprites()) sprite.LoadContent();
			rootControl.LoadContent();
			InputContext.LoadContent();
		}

		public override void UnloadContent()
		{
			foreach (var sprite in World.Sprites()) sprite.UnloadContent();
			rootControl.UnloadContent();
			InputContext.UnloadContent();
		}

		private void CreateGrass()
		{
			Console.Write("Creating grass... ");
			for (int x = 0; x < World.SizeX; x++)
			{
				for (int y = 0; y < World.SizeY; y++)
				{
					Sprite sprite = kernel.Get<Grass>();
					sprite.Tile = World.Tiles.Get(x, y);
					// sprite.Position = GetPosition(x, y);
					// this.World.Add(sprite);
				}
			}
			Console.WriteLine("done.");
		}

		private void CreateStones()
		{
			Console.Write("Creating stones... ");
			var random = kernel.Get<Random>();
			int tiles = World.SizeX * World.SizeY;
			int stones = random.Next(tiles/5, tiles/3);
			var pathfinder = new PathFinding.Pathfinder(World);
			IList<Tile> path;

			for (int n = 0; n < stones; n++)
			{
				Sprite sprite = kernel.Get<Stone>();

				do
				{
					// Find tile
					Tile tile;
					do
					{
						tile = World.Tiles.Get(random.Next(World.SizeX), random.Next(World.SizeY));
					} while (tile.Entities.Count() > 1);

					// Set tile
					sprite.Tile = tile;

				} while ((path = pathfinder.FindPath(SpawnLeft.Tile, Arrival.Tile)) == null);
			}
			Console.WriteLine(" done.");
		}

		private void CreateArrival()
		{
			Console.Write("Creating arrival... ");

			var random = kernel.Get<Random>();
			int centerX = random.Next(1, World.SizeX - 2);

			SpriteFactory factory = kernel.Get<SpriteFactory>();
			var sprites = factory.CreateSprites<Arrival>(centerX - 1, 0, centerX + 1, 2);

			Arrival = sprites.First(x => x.Direction == DirectionType.Bottom);

			Console.WriteLine("done.");
		}

		private void CreateSpawn()
		{
			Console.Write("Creating spawn... ");

			var random = kernel.Get<Random>();
			int x = random.Next(1, World.SizeX - 2);

			Spawn spawnLeft = kernel.Get<Spawn>();
			spawnLeft.Tile = World.Tiles.Get(x, World.SizeY - 1);
			spawnLeft.Direction = DirectionType.Left;

			Spawn spawnRight = kernel.Get<Spawn>();
			spawnRight.Tile = World.Tiles.Get(x+1, World.SizeY - 1);
			spawnRight.Direction = DirectionType.Right;

			SpawnLeft = spawnLeft;
			SpawnRight = spawnRight;

			Console.WriteLine("done.");
		}

		public virtual DrawingPointF GetPosition(int x, int y)
		{
			return new DrawingPointF(offsetX + tileSize * x, offsetY + tileSize * y);
		}

		public virtual DrawingPointF GetPosition(Tile tile)
		{
			return GetPosition(tile.X, tile.Y);
		}

		public virtual int GetTileSize()
		{
			return tileSize;
		}

		public virtual Tile GetTileFromPosition(DrawingPointF position)
		{
			int x = (int)((position.X - offsetX) / tileSize);
			int y = (int)((position.Y - offsetY) / tileSize);

			if (x >= 0 && y >= 0 && x < World.SizeX && y < World.SizeY)
				return World.Tiles.Get(x, y);
			else
				return null;
		}

		public override void Draw(GameTime gameTime)
		{
			// Draw background
			RenderTarget.FillRectangle(backgroundRectangle, backgroundBrush);

			// Draw all sprites
			foreach(var sprite in World.Entities().OfType<Sprite>().OrderBy(x => x.DrawOrder).ThenBy(x => x.Tile.Entities.Count))
			{
				sprite.Draw(gameTime);
			}

			rootControl.Draw(gameTime);
			InputContext.Draw(gameTime);
		}

		public override void Update(GameTime gameTime)
		{
			InputContext.Update(gameTime);

			if (SpawnProvider != null)
			{
				SpawnProvider.Update(gameTime);
			}

			foreach (var entity in World.Entities())
			{
				entity.Update(gameTime);
			}

			rootControl.Update(gameTime);
		}

		
	}
}
