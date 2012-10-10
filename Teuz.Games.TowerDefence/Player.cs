using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence
{
	class Player
	{
		private int cash;

		public int Cash
		{
			get { return cash; }
			set { cash = value; OnCashChanged(); }
		}

		private int currentLevelId;

		public int CurrentLevelId
		{
			get { return currentLevelId; }
			set { currentLevelId = value; OnCurrentLevelIdChanged(); }
		}


		public event EventHandler CashChanged;
		public event EventHandler CurrentLevelIdChanged;

		public Player()
		{
			Cash = 200;
			CurrentLevelId = 1;
		}

		protected virtual void OnCurrentLevelIdChanged()
		{
			EventHandler temp = CurrentLevelIdChanged;
			if (temp != null)
			{
				temp(this, EventArgs.Empty);
			}
		}

		protected virtual void OnCashChanged()
		{
			EventHandler temp = CashChanged;
			if (temp != null)
			{
				temp(this, EventArgs.Empty);
			}
		}
	}
}
