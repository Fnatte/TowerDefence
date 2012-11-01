using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX;
using Device = SharpDX.Direct3D10.Device;
using Device1 = SharpDX.Direct3D10.Device1;
using DriverType = SharpDX.Direct3D10.DriverType;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;


namespace Teuz.Games.TowerDefence
{
	class GameWindow : IDisposable
	{
		private readonly GameTime clock = new GameTime();
		private Form form;
		private bool disposed;
		private bool initialized;
		private float frameAccumulator;
		private int frameCount;
		private bool isFormClosed;
		private bool isFormResizing;

		public float FrameDelta { get; private set; }
		public float FramesPerSecond { get; private set; }

		public string Title { get; set; }

		// Graphics Interface
		public Texture2D BackBuffer { get; private set; }
		public RenderTargetView BackBufferView { get; private set; }
		private SwapChain swapChain;
		private Device1 device;

		// DX2D
		public Factory Factory2D { get; private set; }
		public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
		public RenderTarget RenderTarget { get; private set; }
		public SolidColorBrush SceneColorBrush { get; private set; }

		protected IntPtr DisplayHandle
		{
			get { return form.Handle; }
		}

		public Form Form { get { return form; } }

		public Device1 Device { get { return device; } }

		~GameWindow()
		{
			if (!disposed)
			{
				Dispose();
				disposed = true;
			}

			GC.SuppressFinalize(this);
		}

		public GameWindow()
		{
			this.Title = "GameWindow";
		}

		public void Dispose()
		{
			if (form != null) form.Dispose();
		}

		protected virtual void CreateForm()
		{
			if (form != null) throw new Exception("Form is already created.");
			form = new RenderForm(Title);
			form.MouseClick += (o, e) => { OnMouseClick(e); };
			form.KeyDown += (o, e) => { OnKeyDown(e); };
			form.KeyUp += (o, e) => { OnKeyUp(e); };
			form.FormClosed += (o, e) => { isFormClosed = true; };
			form.ResizeBegin += (o, e) => { isFormResizing = true; };
			form.ResizeEnd += (o, e) => { isFormResizing = false; };
		}

		protected virtual void InitializeGraphicsInterface()
		{
			var desc = new SwapChainDescription()
			{
				BufferCount = 1,
				ModeDescription = new ModeDescription(form.Width, form.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
				IsWindowed = true,
				OutputHandle = DisplayHandle,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.Discard,
				Usage = Usage.RenderTargetOutput
			};

			// Create device and swapchain
			Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc, FeatureLevel.Level_10_0, out device, out swapChain);

			// Ignore all windows events
			SharpDX.DXGI.Factory factory = swapChain.GetParent<SharpDX.DXGI.Factory>();
			factory.MakeWindowAssociation(DisplayHandle, WindowAssociationFlags.IgnoreAll);

			// New RenderTargetView from the backbuffer
			BackBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
			BackBufferView = new RenderTargetView(device, BackBuffer);
		}

		protected virtual void Initialize2D()
		{
#if DEBUG
			Factory2D = new SharpDX.Direct2D1.Factory(FactoryType.SingleThreaded, DebugLevel.Information);
#else
			Factory2D = new SharpDX.Direct2D1.Factory(FactoryType.SingleThreaded);
#endif
			using (var surface = BackBuffer.QueryInterface<Surface>())
			{
				RenderTarget = new RenderTarget(Factory2D, surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
			}
			RenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;

			FactoryDWrite = new SharpDX.DirectWrite.Factory();

			SceneColorBrush = new SolidColorBrush(RenderTarget, Color.White);
		}

		public void InitializeWindow()
		{
			CreateForm();
			InitializeGraphicsInterface();
			Initialize2D();
			Initialize();
			LoadContent();

			initialized = true;
		}


		public void Run()
		{
			if (!initialized) throw new Exception("GameWindow must be initialized before runned.");

			clock.Start();
			BeginRun();

			RenderLoop.Run(form, () =>
			{
				if (isFormClosed)
					return;

				FrameDelta = (float)clock.Update().TotalSeconds;
				Update(clock);

				if (!isFormResizing) Render();
			});

			UnloadContent();
			EndRun();

			Dispose();
		}

		private void Render()
		{
			frameAccumulator += FrameDelta;
			frameCount++;
			if (frameAccumulator >= 1.0f)
			{
				FramesPerSecond = frameCount / frameAccumulator;
				frameAccumulator = 0.0f;
				frameCount = 0;
			}

			BeginDraw();

			Device.Rasterizer.SetViewports(new Viewport(0, 0, form.Width, form.Height));
			Device.OutputMerger.SetTargets(BackBufferView);
			RenderTarget.BeginDraw();
			RenderTarget.Clear(SceneColorBrush.Color);

			Draw(clock);

			RenderTarget.EndDraw();
			swapChain.Present(0, PresentFlags.None);

			EndDraw();
		}

		protected virtual void OnMouseClick(MouseEventArgs e) { }
		protected virtual void OnKeyDown(KeyEventArgs e) { }
		protected virtual void OnKeyUp(KeyEventArgs e) { }

		protected virtual void Initialize() { }
		protected virtual void LoadContent() {}
		protected virtual void UnloadContent() {}
		protected virtual void BeginRun() {}
		protected virtual void EndRun() {}
		protected virtual void Update(GameTime gameTime) {}
		protected virtual void BeginDraw() {}
		protected virtual void Draw(GameTime gameTime) {}
		protected virtual void EndDraw() {}
	}
}
