using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.GameEntities;
using Teuz.Games.TowerDefence.GameEntities.Attackers;

namespace Teuz.Games.TowerDefence.Levels
{
	class SpawnInfo : ICloneable
	{
		public Type Type { get; set; }
		public TimeSpan Interval { get; set; }
		public int Count { get; set; }

		public SpawnInfo()
		{
			Type = typeof(Bug);
			Interval = TimeSpan.FromMilliseconds(1500);
			Count = 1;
		}

		public object Clone()
		{
			SpawnInfo spawnInfo = new SpawnInfo();
			spawnInfo.Type = Type;
			spawnInfo.Interval = Interval;
			spawnInfo.Count = Count;
			return spawnInfo;
		}
	}
}
