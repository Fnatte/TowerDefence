using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence
{
	interface IPlacable
	{
		bool IsPlaced { get; }
		bool TryPlace();
		event EventHandler Placed;
	}
}
