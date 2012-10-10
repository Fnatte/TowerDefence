using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Ninject;
using Teuz.Games.TowerDefence.GameEntities.Towers;
using SharpDX.Direct2D1;
using Teuz.Games.TowerDefence.States;
using System.Collections;
namespace Teuz.Games.TowerDefence.UI
{
	class TowerShopControl : UIControl
	{
		private TowerShopTooltip<ArrowTower> arrowTowerTooltip;
		private ImageButton arrowTowerButton;

		private TowerShopTooltip<RockTower> rockTowerTooltip;
		private ImageButton rockTowerButton;

		private TowerShopTooltip<BlockTower> blockTowerTooltip;
		private ImageButton blockTowerButton;

		private TowerShopTooltip<IceTower> iceTowerTooltip;
		private ImageButton iceTowerButton;

		private const int buttonPadding = 5;
		private GameInputContext inputContext;
		private const int tooltipMargin = 5;

		private readonly ArrayList tooltips = new ArrayList();
		private readonly List<ImageButton> buttons = new List<ImageButton>();

		private Player player;
		private GameStateManager gameStateManager;
		private IContentProvider contentProvider;

		public TowerShopControl(Player player, GameInputContext gameInputContext, GameStateManager gameStateManager, IContentProvider contentProvider)
			:base(contentProvider)
		{
			this.player = player;
			this.inputContext = gameInputContext;
			this.gameStateManager = gameStateManager;
			this.contentProvider = contentProvider;

			// Arrow tower
			arrowTowerButton = new ImageButton("ArrowTowerButtonNormal", "ArrowTowerButtonHover", "ArrowTowerButtonPressed");
			arrowTowerButton.Width = 32;
			arrowTowerButton.Height = 32;
			arrowTowerButton.Click += button_Click;
			arrowTowerButton.Tag = typeof(ArrowTower);

			arrowTowerTooltip = NinjectFactory.Kernel.Get<TowerShopTooltip<ArrowTower>>();
			arrowTowerButton.SetTooltip(arrowTowerTooltip);

			this.Children.Add(arrowTowerTooltip);
			this.Children.Add(arrowTowerButton);

			// Rock tower
			rockTowerButton = new ImageButton("RockTowerButtonNormal", "RockTowerButtonHover", "RockTowerButtonPressed");
			rockTowerButton.Width = 32;
			rockTowerButton.Height = 32;
			rockTowerButton.Click += button_Click;
			rockTowerButton.Tag = typeof(RockTower);

			rockTowerTooltip = NinjectFactory.Kernel.Get<TowerShopTooltip<RockTower>>();
			rockTowerButton.SetTooltip(rockTowerTooltip);

			this.Children.Add(rockTowerTooltip);
			this.Children.Add(rockTowerButton);

			// Block tower
			blockTowerButton = new ImageButton("BlockTowerButtonNormal", "BlockTowerButtonHover", "BlockTowerButtonPressed");
			blockTowerButton.Width = 32;
			blockTowerButton.Height = 32;
			blockTowerButton.Click += button_Click;
			blockTowerButton.Tag = typeof(BlockTower);

			blockTowerTooltip = NinjectFactory.Kernel.Get<TowerShopTooltip<BlockTower>>();
			blockTowerButton.SetTooltip(blockTowerTooltip);

			this.Children.Add(blockTowerTooltip);
			this.Children.Add(blockTowerButton);

			// Ice tower
			iceTowerButton = new ImageButton("IceTowerButtonNormal", "IceTowerButtonHover", "IceTowerButtonPressed");
			iceTowerButton.Width = 32;
			iceTowerButton.Height = 32;
			iceTowerButton.Click += button_Click;
			iceTowerButton.Tag = typeof(IceTower);

			iceTowerTooltip = NinjectFactory.Kernel.Get<TowerShopTooltip<IceTower>>();
			iceTowerButton.SetTooltip(iceTowerTooltip);

			this.Children.Add(iceTowerTooltip);
			this.Children.Add(iceTowerButton);
		}

		public override void LoadContent()
		{
			base.LoadContent();
		}

		void button_Click(object sender, ButtonClickEventArgs e)
		{
			if (inputContext.State == GameInputContextState.None ||
				inputContext.State == GameInputContextState.Placing)
			{
				// Get tower type
				Type towerType = (Type)((UIElement)sender).Tag;

				// Can the player afford it?
				TowerAttribute attr = (TowerAttribute)towerType.GetCustomAttributes(typeof(TowerAttribute), true).FirstOrDefault();
				if (player.Cash >= attr.Cost)
				{
					var tower = (Tower)NinjectFactory.Kernel.Get(towerType);
					tower.Initialize();
					tower.LoadContent();
					inputContext.State = GameInputContextState.Placing;
					inputContext.CurrentSprite = tower;
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			var padding = new Vector2(buttonPadding);

			arrowTowerButton.Position = Position + padding;
			arrowTowerTooltip.Position =
				arrowTowerButton.Position +
				new Vector2(arrowTowerButton.Width / 2 - arrowTowerTooltip.Width / 2, arrowTowerButton.Height + tooltipMargin);

			rockTowerButton.Position = Position + padding + new Vector2(40, 0);
			rockTowerTooltip.Position =
				rockTowerButton.Position +
				new Vector2(rockTowerButton.Width / 2 - rockTowerTooltip.Width / 2, rockTowerButton.Height + tooltipMargin);

			blockTowerButton.Position = Position + padding + new Vector2(80, 0);
			blockTowerTooltip.Position =
				blockTowerButton.Position +
				new Vector2(blockTowerButton.Width / 2 - blockTowerTooltip.Width / 2, blockTowerButton.Height + tooltipMargin);

			iceTowerButton.Position = Position + padding + new Vector2(0, 40);
			iceTowerTooltip.Position =
				iceTowerButton.Position +
				new Vector2(iceTowerButton.Width / 2 - iceTowerTooltip.Width / 2, iceTowerButton.Height + tooltipMargin);

			base.Update(gameTime);

			this.Enabled = gameStateManager.CurrentState is ShoppingState;
		}
	}
}
