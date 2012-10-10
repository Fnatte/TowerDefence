using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace Teuz.Games.TowerDefence
{
	interface IContentProvider
	{
		TextureMap LoadTexture(string key);
		TextureAnimationMap LoadTextureAnimation(string key);
		SolidColorBrush LoadSolidColorBrush(Color color);
	}
}
