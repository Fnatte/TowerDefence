﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.PathFinding
{
	class SearchNode
	{
		public SearchNode Parent { get; set; }
		public bool InOpenList { get; set; }
		public bool InClosedList { get; set; }
		public float DistanceToGoal { get; set; }
		public float DistanceTraveled { get; set; }
		public Tile Tile { get; set; }
		public SearchNode[] Neighbors { get; set; }
		public bool Walkable { get; set; }
	}
}
