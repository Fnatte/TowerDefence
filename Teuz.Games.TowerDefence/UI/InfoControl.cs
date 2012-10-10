using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Ninject;

namespace Teuz.Games.TowerDefence.UI
{
	class InfoControl : UIControl
	{
		private TextBlock tbLevel, tbAttackersLeft, tbCash;

		private const string levelFormat = "Level: {0}";
		private const string attackersLeftFormat = "Attackers Left: {0}";
		private const string cashFormat = "Cash: {0}";

		private Player player;

		public InfoControl(IContentProvider contentProvider) : base(contentProvider)
		{
			this.player = NinjectFactory.Kernel.Get<Player>();
			this.player.CashChanged += player_CashChanged;
			this.player.CurrentLevelIdChanged += player_CurrentLevelIdChanged;
			this.Height = 150;

			this.tbLevel = new TextBlock(String.Format(levelFormat, player.CurrentLevelId));
			this.tbAttackersLeft = new TextBlock(String.Format(attackersLeftFormat, 0));
			this.tbCash = new TextBlock(String.Format(cashFormat, player.Cash));

			this.Children.Add(tbLevel);
			this.Children.Add(tbAttackersLeft);
			this.Children.Add(tbCash);
		}

		void player_CurrentLevelIdChanged(object sender, EventArgs e)
		{
			tbLevel.Text = String.Format(levelFormat, player.CurrentLevelId);
		}

		void player_CashChanged(object sender, EventArgs e)
		{
			tbCash.Text = String.Format(cashFormat, player.Cash);
		}

		public override void Update(GameTime gameTime)
		{
			if(Parent != null) Width = Parent.Width;
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			this.tbLevel.Position = Position + new Vector2(5, 0);
			this.tbAttackersLeft.Position = Position + new Vector2(5, 25);
			this.tbCash.Position = Position + new Vector2(5, 50);

			base.Draw(gameTime);
		}
	}
}
