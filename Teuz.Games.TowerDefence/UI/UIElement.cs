using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SharpDX;
using SharpDX.Direct2D1;
using FactoryDWrite = SharpDX.DirectWrite.Factory;

namespace Teuz.Games.TowerDefence.UI
{
	abstract class UIElement : IGameModel, IDrawable, ITextRenderer, INotifyPropertyChanged
	{
		protected TowerDefenceWindow Window { get; private set; }

		public RenderTarget RenderTarget { get { return Window.RenderTarget; } }
		public FactoryDWrite FactoryDWrite { get { return Window.FactoryDWrite; } }

		public int DrawOrder { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private int width;

		public int Width
		{
			get { return width; }
			set { width = value; OnPropertyChanged("Width"); }
		}

		private int height;

		public int Height
		{
			get { return height; }
			set { height = value; OnPropertyChanged("Height"); }
		}

		private Vector2 position;

		public Vector2 Position
		{
			get { return position; }
			set { position = value; OnPropertyChanged("Position"); }
		}

		private bool visible;

		public bool Visible
		{
			get { return visible; }
			set { visible = value; OnPropertyChanged("Visible"); }
		}

		private bool enabled;

		public bool Enabled
		{
			get { return enabled; }
			set { enabled = value; OnPropertyChanged("Enabled"); }
		}



		private UIElement parent;

		protected IContentProvider ContentProvider { get; private set; }

		public UIElement Parent
		{
			get { return parent; }
			set { parent = value; OnPropertyChanged("Parent"); }
		}

		private object tag;

		public object Tag
		{
			get { return tag; }
			set { tag = value; OnPropertyChanged("Tag"); }
		}


		public UIElement()
		{
			this.Window = NinjectFactory.Kernel.Get<TowerDefenceWindow>();
			this.ContentProvider = NinjectFactory.Kernel.Get<IContentProvider>();
			this.Visible = true;
			this.Enabled = true;
		}

		public abstract void Initialize();
		public abstract void LoadContent();
		public abstract void UnloadContent();
		public abstract void Draw(GameTime gameTime);
		public abstract void Update(GameTime gameTime);

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler temp = PropertyChanged;
			if (temp != null)
			{
				temp(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected virtual RectangleF GetRectangle()
		{
			return new RectangleF(this.Position.X, this.Position.Y, Width, Height);
		}

		public RootControl FindRootControl()
		{
			if (Parent == null) return null;
			if (Parent is RootControl) return (RootControl)Parent;

			var node = Parent;
			while (node.Parent != null)
			{
				node = node.Parent;
				if (node is RootControl) break;
			}

			return (RootControl)node;
		}
	}
}
