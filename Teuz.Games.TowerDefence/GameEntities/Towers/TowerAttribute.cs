using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.GameEntities.Towers
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	sealed class TowerAttribute : Attribute
	{
		readonly string name;
		readonly int cost;

		public TowerAttribute(string name, int cost)
		{
			this.name = name;
			this.cost = cost;
		}

		public string Name
		{
			get { return name; }
		}

		public int Cost
		{
			get { return cost; }
		}
	}
}
