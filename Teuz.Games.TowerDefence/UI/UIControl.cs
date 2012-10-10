using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace Teuz.Games.TowerDefence.UI
{
	abstract class UIControl : UIElement
	{
		private readonly ObservableCollection<UIElement> children = new ObservableCollection<UIElement>();
		public ObservableCollection<UIElement> Children { get { return children; } }

		public SolidColorBrush DisabledBrush { get; private set; }

		private IContentProvider contentProvider;

		public UIControl(IContentProvider contentProvider) : base()
		{
			this.contentProvider = contentProvider;
			Children.CollectionChanged += Children_CollectionChanged;
		}

		void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var element in e.NewItems.Cast<UIElement>())
				{
					UIControl parent = element.Parent as UIControl;
					if (parent != null) parent.Children.Remove(element);
					element.Parent = this;
				}
			}
		}

		public override void Initialize()
		{
			foreach (var child in children) child.Initialize();
		}

		public override void LoadContent()
		{
			foreach (var child in children) child.LoadContent();

			DisabledBrush = contentProvider.LoadSolidColorBrush(
				new Color(0, 0, 0, 0.5f)
			);
		}

		public override void UnloadContent()
		{
			foreach (var child in children) child.UnloadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			if (!Visible) return;
			foreach (var child in children.Where(x => x.Visible)) child.Draw(gameTime);

			if (!Enabled)
			{
				RenderTarget.FillRectangle(GetRectangle(), DisabledBrush);
			}
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var child in children.OrderBy(x => x.DrawOrder))
			{
				child.Update(gameTime);
			}
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == "Enabled")
			{
				foreach (var child in children) child.Enabled = Enabled;
			}
		}
	}
}
