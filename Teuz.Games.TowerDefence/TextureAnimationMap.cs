using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Teuz.Games.TowerDefence
{
	class TextureAnimationMap : TextureMap
	{
		private readonly List<RectangleF> sourceRectangles = new List<RectangleF>();
		public List<RectangleF> SourceRectangles { get { return sourceRectangles; } }

		public void AddTextureMap(TextureMap textureMap)
		{
			if (Bitmap != textureMap.Bitmap) throw new Exception("Bitmap must be the same.");

			this.SourceRectangles.Add(textureMap.SourceRectangle.Value);
		}
	}
}
