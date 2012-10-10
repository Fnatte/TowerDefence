using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.GameEntities
{
	class PathLight : Sprite
	{
		public PathLightColor Color { get; set; }

		public PathLight(ICamera camera, IContentProvider contentProvider)
			: base(camera, contentProvider)
		{

		}

		public override void LoadContent()
		{
			SetTextureMap(ContentProvider.LoadTexture("PathLight" + Color.ToString()));
		}
	}

	enum PathLightColor
	{
		Red
	}
}
