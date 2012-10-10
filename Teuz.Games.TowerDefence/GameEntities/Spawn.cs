using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Teuz.Games.TowerDefence.GameEntities
{
	class Spawn : Sprite, IBorder
	{
		public DirectionType Direction
		{
			get;
			set;
		}

		public Spawn(ICamera camera, IContentProvider contentProvider)
			: base(camera, contentProvider)
		{

		}

		public override void LoadContent()
		{
			SetTextureMap(ContentProvider.LoadTexture("Spawn" + Direction.ToString()));
		}
	}
}
