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
		// Vector2
		public static DrawingPointF ToDrawingPointF(this Vector2 vector)
		{
			return new DrawingPointF(vector.X, vector.Y);
		}

		// Drawing.Point
		public static DrawingPointF ToDrawingPointF(this System.Drawing.Point point)
		{
			return new DrawingPointF(point.X, point.Y);
		}
		public static Vector2 ToVector2(this System.Drawing.Point point)
		{
			return new Vector2(point.X, point.Y);
		}

		// DrawingPointF
		public static Vector2 ToVector2(this DrawingPointF point)
		{
			return new Vector2(point.X, point.Y);
		}

		// DrawingSize
		public static Vector2 ToVector2(this DrawingSize point)
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
