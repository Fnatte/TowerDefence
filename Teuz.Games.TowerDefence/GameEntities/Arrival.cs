using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.GameEntities
{
	class Arrival : Sprite, IBorder
	{
		public DirectionType Direction { get; set; }

		public Arrival(ICamera camera, IContentProvider contentProvider)
			: base(camera, contentProvider)
		{

		}

		public override void LoadContent()
		{
			string key = "Arrival";
			if (Direction != DirectionType.Middle) key += Direction.ToString();

			SetTextureMap(ContentProvider.LoadTexture(key));
		}
	}
}
