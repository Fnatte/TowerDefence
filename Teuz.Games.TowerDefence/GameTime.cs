using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence
{
	public class GameTime
	{
		private Stopwatch stopwatch;
		public TimeSpan LastUpdate { get; private set; }
		public TimeSpan Elapsed { get; private set; }

		public GameTime()
		{
			stopwatch = new Stopwatch();
		}

		public void Start()
		{
			LastUpdate = TimeSpan.Zero;
			stopwatch.Start();
		}

		public void Stop()
		{
			stopwatch.Stop();
		}

		public TimeSpan Update()
		{
			Elapsed = stopwatch.Elapsed - LastUpdate;
			LastUpdate = stopwatch.Elapsed;
			return Elapsed;
		}
		
	}
}
