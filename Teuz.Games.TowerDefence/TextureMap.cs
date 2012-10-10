using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace Teuz.Games.TowerDefence
{
	class TextureMap
	{
		public Bitmap Bitmap { get; set; }
		public RectangleF? SourceRectangle { get; set; }
	}
}
