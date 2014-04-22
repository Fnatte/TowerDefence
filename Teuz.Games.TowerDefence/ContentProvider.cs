using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace Teuz.Games.TowerDefence
{
	class ContentProvider : IContentProvider
	{
		private RenderTarget renderTarget;
		public string RootFolder { get; set; }
		public string BitmapFolder { get; set; }
		private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();

		private readonly Dictionary<Color, SolidColorBrush> solidColorBrushes = new Dictionary<Color, SolidColorBrush>();

		private Bitmap atlas;
		private XDocument atlasXml;

		public ContentProvider(RenderTarget renderTarget)
		{
			this.RootFolder = "Content";
			this.BitmapFolder = "Textures";
			this.renderTarget = renderTarget;

			Console.WriteLine("ContentProvider constructed.");
		}

		#region Textures

		private void LoadAtlasXml()
		{
			atlasXml = XDocument.Load(
				Path.Combine(RootFolder, BitmapFolder, "map.xml")
			);
		}

		private void LoadAtlasBitmap()
		{
			var fileName = atlasXml.Root.Attribute("imagePath");
			if (fileName == null) throw new Exception("Invaild XML atlas");

			atlas = LoadBitmapFile(fileName.Value);
		}

		public TextureMap LoadTexture(string key)
		{
			if (key.Contains("."))
			{
				TextureMap textureMap = new TextureMap();
				textureMap.Bitmap = LoadBitmapByKey(key);
				return textureMap;
			}
			else
			{
				if (atlasXml == null) LoadAtlasXml();

				var element = atlasXml.Root.Elements("sprite").FirstOrDefault(x => x.Attribute("n").Value == key);
				if (element != null)
				{
					if (atlas == null) LoadAtlasBitmap();
					TextureMap textureMap = new TextureMap();
					textureMap.Bitmap = atlas;
					int x = int.Parse(element.Attribute("x").Value);
					int y = int.Parse(element.Attribute("y").Value);
					int w = int.Parse(element.Attribute("w").Value);
					int h = int.Parse(element.Attribute("h").Value);
					textureMap.SourceRectangle = new RectangleF(x, y, w, h);
					return textureMap;
				}
			}

			throw new Exception("Could not find texture. Key was: \"" + key + "\"");
		}

		public TextureAnimationMap LoadTextureAnimation(string key)
		{
			TextureAnimationMap animationMap = new TextureAnimationMap();
			for (int n = 1; true; n++)
			{
				TextureMap textureMap;
				try
				{
					textureMap = LoadTexture(key.Replace("*", n.ToString()));
				}
				catch (Exception e)
				{
					if (n == 1) throw e;
					break;
				}

				if (n == 1) animationMap.Bitmap = textureMap.Bitmap;
				animationMap.AddTextureMap(textureMap);
			}

			animationMap.SourceRectangle = animationMap.SourceRectangles.First();

			return animationMap;
		}

		protected Bitmap LoadBitmapByKey(string key)
		{
			if (bitmaps.ContainsKey(key)) return bitmaps[key];
			Bitmap bitmap = LoadBitmapFile(key);
			bitmaps.Add(key, bitmap);
			return bitmap;
		}

		protected Bitmap LoadBitmapFile(string fileName)
		{
			// Loads from file using System.Drawing.Image
			using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path.Combine(RootFolder, BitmapFolder, fileName)))
			{
				var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
				var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
				var size = new Size2(bitmap.Width, bitmap.Height);

				// Transform pixels from BGRA to RGBA
				int stride = bitmap.Width * sizeof(int);
				using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
				{
					// Lock System.Drawing.Bitmap
					var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

					// Convert all pixels 
					for (int y = 0; y < bitmap.Height; y++)
					{
						int offset = bitmapData.Stride * y;
						for (int x = 0; x < bitmap.Width; x++)
						{
							// Not optimized 
							byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
							byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
							byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
							byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
							int rgba = R | (G << 8) | (B << 16) | (A << 24);
							tempStream.Write(rgba);
						}

					}
					bitmap.UnlockBits(bitmapData);
					tempStream.Position = 0;

					return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
				}
			}
		}

		#endregion

		#region SolidColorBrushes

		public SolidColorBrush LoadSolidColorBrush(Color color)
		{
			if (solidColorBrushes.ContainsKey(color)) return solidColorBrushes[color];

			SolidColorBrush brush = new SolidColorBrush(renderTarget, color);
			solidColorBrushes.Add(color, brush);

			return brush;
		}

		#endregion
	}
}
