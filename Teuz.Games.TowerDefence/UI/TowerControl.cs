using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.GameEntities.Towers;
using Ninject;
using SharpDX;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.UI
{
	class TowerControl : UIControl
	{
		private Tower tower;

		public Tower Tower
		{
			get { return tower; }
			set { tower = value; OnPropertyChanged("Tower"); }
		}

		TextButton btnSell, btnUpgrade;
		TextBlock tbName, tbLevel, tbSellWorth, tbUpgradeCost;

		private GameInputContext inputContext;
		private GameStateManager gameStateManager;

		public TowerControl(IContentProvider contentProvder, GameInputContext inputContext, GameStateManager gameStateManager)
			: base(contentProvder)
		{
			this.inputContext = inputContext;
			this.gameStateManager = gameStateManager;

			btnSell = new TextButton("Sell");
			btnUpgrade = new TextButton("Upgrade");
			tbName = new TextBlock() { Color = Color.Black, FontSize = 15 };
			tbLevel = new TextBlock() { Color = Color.Black, FontSize = 15 };
			tbSellWorth = new TextBlock() { Color = Color.Black, FontSize = 15 };
			tbUpgradeCost = new TextBlock() { Color = Color.Black, FontSize = 15 };

			btnSell.Click += btnSell_Click;
			btnUpgrade.Click += btnUpgrade_Click;

			Children.Add(btnSell);
			Children.Add(btnUpgrade);
			Children.Add(tbName);
			Children.Add(tbLevel);
			Children.Add(tbSellWorth);
			Children.Add(tbUpgradeCost);
		}

		void btnUpgrade_Click(object sender, ButtonClickEventArgs e)
		{
			if (Tower != null)
			{
				Tower.Upgrade();
				inputContext.CurrentSprite = Tower;
			}
		}

		void btnSell_Click(object sender, ButtonClickEventArgs e)
		{
			if (Tower != null)
			{
				Tower.Sell();
				inputContext.CurrentSprite = null;
			}
		}

		public override void Update(GameTime gameTime)
		{
			this.Enabled = gameStateManager.CurrentState is ShoppingState;

			base.Update(gameTime);

			tbName.Position = Position + new Vector2(2, 0);
			tbLevel.Position = Position + new Vector2(2, 15);
			tbSellWorth.Position = Position + new Vector2(2, 30);
			tbUpgradeCost.Position = Position + new Vector2(2, 45);

			btnSell.Position = Position + new Vector2(25, 65);
			btnUpgrade.Position = Position + new Vector2(25, 90);
		}


		protected override void OnPropertyChanged(string propertyName)
		{
			if (propertyName == "Tower" && Tower != null)
			{
				// Get tower attribute
				TowerAttribute attr = (TowerAttribute)Tower.GetType().GetCustomAttributes(typeof(TowerAttribute), true).FirstOrDefault();
				tbName.Text = attr.Name;
				tbLevel.Text = "Level: " + Tower.Level.ToString();
				tbSellWorth.Text = "Sell Worth: " + Tower.GetSellPrice();
				tbUpgradeCost.Text = "Upgrade Cost: " + Tower.GetUpgradeCost();
			}

			base.OnPropertyChanged(propertyName);
		}
	}
}
