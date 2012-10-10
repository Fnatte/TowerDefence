using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using SharpDX;
using SharpDX.Direct2D1;
using FactoryDWrite = SharpDX.DirectWrite.Factory;

namespace Teuz.Games.TowerDefence.GameEntities
{
	class Sprite : Entity, IGameModel
	{
		public Bitmap Bitmap { get; private set; }
		public RectangleF? SourceRectangle { get; set; }
		public DrawingSize Size { get; set; }

		protected ICamera Camera { get; private set; }
		public IContentProvider ContentProvider { get; private set; }

		public Matrix Transformation { get; protected set; }
		protected Vector2 PositionOffset { get; set; }

		public float MovementSpeed { get; set; }

		public RenderTarget RenderTarget { get { return Camera.RenderTarget; } }

		public int DrawOrder { get; set; }

		public Sprite(ICamera camera, IContentProvider contentProvider)
		{
			this.Camera = camera;
			this.MovementSpeed = 1.0f;
			this.ContentProvider = contentProvider;
		}

		public override void MoveTo(Tile tile)
		{
			if (!IsMoving)
			{
				if (tile.CanWalk())
				{
					var point1 = Camera.GetPosition(tile);
					var point2 = Camera.GetPosition(Tile);
					PositionOffset = new Vector2(point2.X - point1.X, point2.Y - point1.Y);
					Tile = tile;
					IsMoving = true;
				}
			}
		}

		protected virtual void UpdateMovement(GameTime gameTime)
		{
			if (IsMoving)
			{
				float moveAmount = MovementSpeed * (float)gameTime.Elapsed.TotalMilliseconds * 0.05f;
				if (Math.Abs(PositionOffset.X) <= moveAmount &&
					Math.Abs(PositionOffset.Y) <= moveAmount)
				{
					IsMoving = false;
					PositionOffset = Vector2.Zero;
				}
				else
				{
					PositionOffset += new Vector2(
						PositionOffset.X > 0 ? -moveAmount : moveAmount,
						PositionOffset.Y > 0 ? -moveAmount : moveAmount
					);
				}
			}
		}

		protected void SetTextureMap(TextureMap textureMap)
		{
			if (textureMap.SourceRectangle.HasValue)
			{
				Bitmap = textureMap.Bitmap;
				SourceRectangle = textureMap.SourceRectangle;
				Size = new DrawingSize((int)SourceRectangle.Value.Width, (int)SourceRectangle.Value.Height);
			}
			else
			{
				SetBitmap(textureMap.Bitmap);
			}
		}

		protected void SetBitmap(Bitmap bitmap)
		{
			this.Bitmap = bitmap;

			if(Size.Width == 0 && Size.Height == 0)
				Size = new DrawingSize((int)bitmap.Size.Width, (int)bitmap.Size.Height);
		}

		public virtual RectangleF GetDestinationRectangle()
		{
			var point = GetPosition();
			return new RectangleF(
				point.X,
				point.Y,
				Size.Width + point.X,
				Size.Height + point.Y
			);
		}

		public virtual void Initialize()
		{

		}

		public virtual void LoadContent()
		{
			
		}

		public virtual void UnloadContent()
		{
			
		}

		public virtual void Draw(GameTime gameTime)
		{
			if (Bitmap == null) throw new Exception("Bitmap cannot be null! Did you forget to load content?");

			if(Transformation != Matrix.Zero) Camera.RenderTarget.Transform = Transformation;

			Camera.RenderTarget.DrawBitmap(
				Bitmap, GetDestinationRectangle(), 1.0f, BitmapInterpolationMode.Linear, SourceRectangle
			);

			Camera.RenderTarget.Transform = Matrix3x2.Identity;
		}

		public override void Update(GameTime gameTime)
		{
			UpdateMovement(gameTime);
		}

		protected virtual void Transform(float rotation, Vector2 scaling)
		{
			var point = GetPosition();
			var halfSize = Size.ToVector2() / 2;
			var center = point + halfSize;
			Transformation = Matrix.Transformation2D(center, 0, scaling, center, rotation, Vector2.Zero);
		}

		public Vector2 GetPosition()
		{
			return Camera.GetPosition(Tile).ToVector2() + PositionOffset;
		}
	}
}
