using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using Teuz.Games.TowerDefence.States;

namespace Teuz.Games.TowerDefence.UI
{
	class LevelControl : UIControl
	{
		private TextButton button;
		private GameStateManager gameStateManager;
		private IContentProvider contentProvider;

		public LevelControl(IContentProvider contentProvider, GameStateManager gameStateManager) : base(contentProvider)
		{
			this.gameStateManager = gameStateManager;
			this.contentProvider = contentProvider;

			button = new TextButton("Next Level");
			button.Click += button_Click;

			Children.Add(button);
		}

		void button_Click(object sender, ButtonClickEventArgs e)
		{
			if (!Enabled) return;

			if (gameStateManager.CurrentState is ShoppingState)
				gameStateManager.PushState<DefendingState>();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			this.Enabled = gameStateManager.CurrentState is ShoppingState;

			button.Position = Position + new Vector2(5f);
		}
	}
}
