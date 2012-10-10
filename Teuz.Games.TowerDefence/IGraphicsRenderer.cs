using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;

namespace Teuz.Games.TowerDefence
{
	interface IGraphicsRenderer
	{
		RenderTarget RenderTarget { get; }
		int Width { get; }
		int Height { get; }
	}
}
