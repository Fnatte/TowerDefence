using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ninject;

namespace Teuz.Games.TowerDefence.UI
{
	abstract class Button : UIElement
	{
		private InputManager input;
		public ButtonState State { get; private set; }
		public UIElement Tooltip { get; private set; }

		public event EventHandler<ButtonClickEventArgs> Click;

		public Button() : base()
		{
			this.input = NinjectFactory.Kernel.Get<InputManager>();
		}

		public override void Update(GameTime gameTime)
		{
			if (!Enabled) return;

			//int halfWidth = Width / 2;
			//int halfHeight = Height / 2;
			//if (input.MousePoint.X >= this.Position.X - halfWidth && input.MousePoint.X <= this.Position.X + halfWidth &&
			//	input.MousePoint.Y >= this.Position.Y - halfHeight && input.MousePoint.Y <= this.Position.Y + halfHeight)
			//if(input.MousePoint.X >= this.Position.X && input.MousePoint.X <= this.Position.X + Width &&
			//	input.MousePoint.Y >= this.Position.Y && input.MousePoint.Y <= this.Position.Y + Height)
			if(GetRectangle().Contains(input.MousePoint.ToVector2()))
			{
				if (input.MouseReleased.HasFlag(MouseButtons.Left)) OnClick();
				State = input.MousePressed.HasFlag(MouseButtons.Left) ? ButtonState.Pressed : ButtonState.Hover;
			}
			else
			{
				State = ButtonState.Normal;
			}

			if (Tooltip != null)
			{
				if (State == ButtonState.Hover)
				{
					Tooltip.Visible = true;
					Tooltip.Enabled = true;
				}
				else
				{
					Tooltip.Enabled = false;
					Tooltip.Visible = false;
				}
			}
		}

		protected virtual void OnClick()
		{
			EventHandler<ButtonClickEventArgs> temp = Click;
			if (temp != null)
			{
				temp(this, new ButtonClickEventArgs(this));
			}
		}

		public virtual void SetTooltip(UIElement element)
		{
			Tooltip = element;
			Tooltip.Enabled = false;
			Tooltip.Visible = false;
		}
	}

	class ButtonClickEventArgs : EventArgs
	{
		public Button Button { get; set; }

		public ButtonClickEventArgs(Button button)
		{
			this.Button = button;
		}
	}

	enum ButtonState
	{
		Normal, Hover, Pressed
	}
}
