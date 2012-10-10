using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.GameEntities
{
	interface ISpriteFactory
	{
		T CreateSprite<T>() where T : Sprite;
	}
}
