using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teuz.Games.TowerDefence
{
	class InputManager : IGameModel
	{
		public List<Keys> KeysPressed { get; private set; }
        public List<Keys> KeysReleased { get; private set; }
        public List<Keys> KeysDown { get; private set; }

        public MouseButtons MousePressed { get; private set; }
        public MouseButtons MouseReleased { get; private set; }
        public MouseButtons MouseDown { get; private set; }
        public Point MousePoint { get; private set; }
        public int MouseWheelDelta { get; private set; }

        Form form;
		float convertX, convertY;

        public InputManager(TowerDefenceWindow window)
        {
            this.form = window.Form;
            
            KeysDown = new List<Keys>();
            KeysPressed = new List<Keys>();
            KeysReleased = new List<Keys>();
            MouseWheelDelta = 0;
			MousePoint = new Point(0, 0);
        }

		public void Initialize()
		{
			Rectangle r = form.RectangleToScreen(form.ClientRectangle);
			int h = r.Top - form.Top;
			this.convertX = (float)form.Width / r.Width;
			this.convertY = (float)form.Height / r.Height;

			form.KeyDown += new KeyEventHandler(form_KeyDown);
			form.KeyUp += new KeyEventHandler(form_KeyUp);
			form.MouseDown += new MouseEventHandler(form_MouseDown);
			form.MouseMove += new MouseEventHandler(form_MouseMove);
			form.MouseUp += new MouseEventHandler(form_MouseUp);
			form.MouseWheel += new MouseEventHandler(form_MouseWheel);

			MousePoint = form.PointToClient(Cursor.Position);
		}

        void form_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyData & ~Keys.Shift;

            if (KeysDown.Contains(key) == false)
            {
                KeysPressed.Add(key);
                KeysDown.Add(key);
            }
        }

        void form_KeyUp(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyData & ~Keys.Shift;

            KeysReleased.Add(key);
            KeysDown.Remove(key);
        }

        public void ClearKeyCache()
        {
            KeysPressed.Clear();
            KeysReleased.Clear();
            MousePressed = MouseButtons.None;
            MouseReleased = MouseButtons.None;
            MouseWheelDelta = 0;
        }

        void form_MouseDown(object sender, MouseEventArgs e)
        {
			MousePoint = ConvertPoint(e.Location);

            if (e.Button == MouseButtons.None)
                return;

            // Don't consider mouse clicks outside of the client area
            if (form.Focused == false || form.ClientRectangle.Contains(e.Location) == false)
                return;

            if (e.Button == MouseButtons.Left)
            {
                if ((MouseDown & MouseButtons.Left) != MouseButtons.Left)
                {
                    MouseDown |= MouseButtons.Left;
                    MousePressed |= MouseButtons.Left;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if ((MouseDown & MouseButtons.Right) != MouseButtons.Right)
                {
                    MouseDown |= MouseButtons.Right;
                    MousePressed |= MouseButtons.Right;
                }
            }
        }

        void form_MouseMove(object sender, MouseEventArgs e)
        {
            MousePoint = ConvertPoint(e.Location);
        }

		private Point ConvertPoint(Point p)
		{
			return new Point((int)(p.X * convertX), (int)(p.Y * convertY));
		}

        void form_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDown &= ~MouseButtons.Left;
				MousePressed &= ~MouseButtons.Left;
                MouseReleased |= MouseButtons.Left;
            }

            if (e.Button == MouseButtons.Right)
            {
                MouseDown &= ~MouseButtons.Right;
				MousePressed &= ~MouseButtons.Right;
                MouseReleased |= MouseButtons.Right;
            }
        }

        void form_MouseWheel(object sender, MouseEventArgs e)
        {
            MouseWheelDelta = e.Delta;
        }

		public void Update(GameTime gameTime)
		{
			MouseReleased = MouseButtons.None;
			KeysReleased.Clear();
		}

		public void LoadContent()
		{
			throw new NotImplementedException();
		}

		public void UnloadContent()
		{
			throw new NotImplementedException();
		}

		public SharpDX.Direct2D1.RenderTarget RenderTarget
		{
			get { throw new NotImplementedException(); }
		}

		public void Draw(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
	}
}
