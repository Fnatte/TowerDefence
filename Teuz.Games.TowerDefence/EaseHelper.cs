using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Teuz.Games.TowerDefence
{
	static class EaseHelper
	{
		public static float Cubic(double value, double power = 3, float min = 0, float max = 1)
		{
			return MathUtil.Clamp((float)Math.Pow(value, power), min, max);
		}
	}
}
