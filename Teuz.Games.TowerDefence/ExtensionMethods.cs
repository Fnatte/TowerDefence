using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Teuz.Games.TowerDefence
{
	static class ExtensionMethods
	{
		// Drawing.Point
		public static Vector2 ToVector2(this System.Drawing.Point point)
		{
			return new Vector2(point.X, point.Y);
		}

		// Size2
		public static Vector2 ToVector2(this Size2 point)
		{
			return new Vector2(point.Width, point.Height);
		}

		// RectangleF
		public static bool Contains(this RectangleF rec, Vector2 point)
		{
			return 
				point.X >= rec.Left && point.X <= rec.Right &&
				point.Y >= rec.Top && point.Y <= rec.Bottom;
		}
		public static bool Intersects(this RectangleF rec1, RectangleF rec2)
		{
			return
				rec1.Contains(new Vector2(rec2.Left, rec2.Top)) ||
				rec1.Contains(new Vector2(rec2.Left, rec2.Bottom)) ||
				rec1.Contains(new Vector2(rec2.Right, rec2.Top)) ||
				rec1.Contains(new Vector2(rec2.Right, rec2.Bottom));
		}
	}
}
