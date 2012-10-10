using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Ninject;

namespace Teuz.Games.TowerDefence.UI
{
	class TextBlock : UIElement
	{
		private TextLayout textLayout;
		private TextFormat textFormat;
		private SolidColorBrush colorBrush;
		private Color color;
		private string text;
		private string fontFamilyName;
		private int fontSize;

		public bool AutoSize { get; set; }

		public Color Color
		{
			get { return color; }
			set
			{
				color = value;
				if (ContentProvider != null)
					colorBrush = ContentProvider.LoadSolidColorBrush(color);
			}
		}

		public string FontFamilyName
		{
			get { return fontFamilyName; }
			set
			{
				fontFamilyName = value;
				if (contentLoaded)
				{
					UpdateTextFormat();
					UpdateTextLayout();
				}
			}
		}

		public int FontSize
		{
			get { return fontSize; }
			set {
				fontSize = value;
				if (contentLoaded)
				{
					UpdateTextFormat();
					UpdateTextLayout();
				}
			}
		}

		public string Text
		{
			get { return text; }
			set { text = value; UpdateTextLayout(); }
		}

		private TextAlignment textAlignment;

		public TextAlignment TextAlignment
		{
			get { return textAlignment; }
			set {
				textAlignment = value;
				if (ContentProvider != null)
				{
					UpdateTextFormat();
					UpdateTextLayout();
				}
			}
		}

		private bool contentLoaded;


		public TextBlock(string text = "")
		{
			Text = text;
			AutoSize = true;
			Color = Color.White;
			FontFamilyName = "Calibri";
			FontSize = 18;
		}

		public override void Initialize()
		{
			
		}

		public override void LoadContent()
		{
			colorBrush = ContentProvider.LoadSolidColorBrush(color);
			UpdateTextFormat();
			UpdateTextLayout();
			contentLoaded = true;
		}

		public override void UnloadContent()
		{
			if (textLayout != null) textLayout.Dispose();
		}

		public override void Draw(GameTime gameTime)
		{
			if (textLayout != null && Visible)
				RenderTarget.DrawTextLayout(
					Position.ToDrawingPointF(),
					textLayout, colorBrush);
		}

		public override void Update(GameTime gameTime)
		{
			
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case "Parent":
					if(AutoSize) UpdateTextLayout();
					Parent.PropertyChanged += Parent_PropertyChanged;
					break;
			}
		}

		void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "Width":
				case "Height":
					if (AutoSize) UpdateTextLayout();
					break;
			}
		}

		protected void UpdateSize()
		{
			if (Parent != null)
			{
				Width = Parent.Width;
				Height = Parent.Height;
			}
		}

		protected void UpdateTextFormat()
		{
			textFormat = new TextFormat(FactoryDWrite, FontFamilyName, FontSize);
			textFormat.TextAlignment = TextAlignment;
		}

		protected void UpdateTextLayout()
		{
			if (String.IsNullOrWhiteSpace(Text)) textLayout = null;
			else 
			{
				UpdateSize();
				if (textFormat != null)
				{
					if (textLayout != null) textLayout.Dispose();
					textLayout = new TextLayout(FactoryDWrite, Text, textFormat, Width, Height);
				}
			}
		}
	}
}
