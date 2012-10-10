using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence.GameEntities
{
	class SpriteFactory : ISpriteFactory
	{
		private IKernel kernel;
		private IWorld world;

		public SpriteFactory(IWorld world)
		{
			kernel = NinjectFactory.Kernel;
			this.world = world;
		}

		public T CreateSprite<T>() where T : Sprite
		{
			return kernel.Get<T>();
		}

		public IEnumerable<T> CreateSprites<T>(int fromX, int fromY, int toX, int toY) where T : Sprite
		{
			List<T> sprites = new List<T>();

			for (int x = fromX; x <= toX; x++)
			{
				for (int y = fromY; y <= toY; y++)
				{
					T sprite = kernel.Get<T>();

					IBorder border = sprite as IBorder;
					if (border != null)
					{
						// Check corners
						if (y == fromY)
						{
							if (x == fromX) border.Direction = DirectionType.TopLeft;
							else if (x == toX) border.Direction = DirectionType.TopRight;
							else border.Direction = DirectionType.Top;
						}
						else if (y == toY)
						{
							if (x == fromX) border.Direction = DirectionType.BottomLeft;
							else if (x == toX) border.Direction = DirectionType.BottomRight;
							else border.Direction = DirectionType.Bottom;
						}
						else if (x == fromX)
						{
							border.Direction = DirectionType.Left;
						}
						else if (x == toX)
						{
							border.Direction = DirectionType.Right;
						}
						else
						{
							border.Direction = DirectionType.Middle;
						}
					}

					sprite.Tile = world.Tiles.Get(x, y);
					sprites.Add(sprite);
				}
			}

			return sprites;
		}
	}
}
