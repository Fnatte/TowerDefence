using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.GameEntities
{
	class Stone : Sprite
	{
		public Stone(ICamera camera, IContentProvider content)
			: base(camera, content)
		{
			this.Walkable = false;
		}

		public override void LoadContent()
		{
			base.LoadContent();

			SetTextureMap(ContentProvider.LoadTexture("Stone1"));
		}
	}
}
