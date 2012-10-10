using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using FactoryDWrite = SharpDX.DirectWrite.Factory;

namespace Teuz.Games.TowerDefence
{
	interface IDrawable
	{
		RenderTarget RenderTarget { get; }

		void Draw(GameTime gameTime);
	}
}
