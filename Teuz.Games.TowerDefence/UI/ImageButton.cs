using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace Teuz.Games.TowerDefence.UI
{
	class ImageButton : Button
	{
		private TextureMap textureMapNormal, textureMapHover, textureMapPressed;
		private string keyNormal, keyHover, keyPressed;

		public ImageButton(string keyNormal = null, string keyHover= null, string keyPressed=null): base()
		{
			this.keyNormal = keyNormal;
			this.keyHover = keyHover;
			this.keyPressed = keyPressed;
		}

		public override void LoadContent()
		{
			if (keyNormal != null) textureMapNormal = ContentProvider.LoadTexture(keyNormal);
			if (keyHover != null) textureMapHover = ContentProvider.LoadTexture(keyHover);
			if (keyPressed != null) textureMapPressed = ContentProvider.LoadTexture(keyPressed);
		}

		public override void Initialize()
		{
			
		}

		public override void UnloadContent()
		{
			
		}

		public override void Draw(GameTime gameTime)
		{
			TextureMap current = null;
			switch (State)
			{
				case ButtonState.Pressed:
					current = textureMapPressed;
					break;
				case ButtonState.Hover:
					current = textureMapHover;
					break;
			}

			if (current == null) current = textureMapNormal;

			RenderTarget.DrawBitmap(
				current.Bitmap,
				GetRectangle(),
				1.0f, BitmapInterpolationMode.Linear,
				current.SourceRectangle
			);
		}
	}
}
