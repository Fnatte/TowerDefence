using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teuz.Games.TowerDefence.GameEntities
{
	class AnimatedSprite : Sprite
	{
		private readonly List<RectangleF> sourceRectangles = new List<RectangleF>();
		public List<RectangleF> SourceRectangles { get { return sourceRectangles; } }
		public int CurrentSourceRectangleIndex { get; private set; }
		public TimeSpan AnimationInterval { get; protected set; }
		protected TimeSpan AnimationClock { get; private set; }

		protected string[] textureKeys;

		public AnimatedSprite(ICamera camera, IContentProvider contentProvider) : base(camera, contentProvider)
		{

		}

		protected void SetTextureAnimationMap(TextureAnimationMap textureAnimationMap)
		{
			SetTextureMap(textureAnimationMap);

			foreach (var rec in textureAnimationMap.SourceRectangles)
				SourceRectangles.Add(rec);
		}

		public override void LoadContent()
		{
			foreach (string key in textureKeys)
			{
				SetTextureMap(ContentProvider.LoadTexture(key));
				SourceRectangles.Add(SourceRectangle.Value);
			}

			SourceRectangle = SourceRectangles.First();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			AnimationClock += gameTime.Elapsed;
			if (AnimationClock >= AnimationInterval)
			{
				AnimationClock = TimeSpan.Zero;
				Animate();
			}
		}

		protected void Animate()
		{
			CurrentSourceRectangleIndex++;
			if (CurrentSourceRectangleIndex >= SourceRectangles.Count) CurrentSourceRectangleIndex = 0;
			SourceRectangle = SourceRectangles[CurrentSourceRectangleIndex];
		}
	}
}
