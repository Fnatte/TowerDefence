using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence
{
	interface IGameModel : IDrawable
	{
		void Initialize();
		void LoadContent();
		void UnloadContent();

		// void Draw(GameTime gameTime);
		void Update(GameTime gameTime);

	}
}
