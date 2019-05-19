using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{
	public class Kafe : Game
	{
		private RenderTarget2D rawScreen;

		GraphicsDeviceManager graphics;

		public static SpriteBatch SpriteBatch;
		public static Effect ClutEffect;

		public static GraphicsDevice GfxDev { get; private set; }
		public static Kafe Me { get; private set; }
		public static Input Input { get; private set; }

		public static LoadingScreen LoadingScreen { get; private set; }

		public const int ScreenWidth = 480, ScreenHeight = 270;
		public const int Scale = 2;
		public const int CrtWidth = ScreenWidth * Scale, CrtHeight = ScreenHeight * Scale;
		public const int Speed = 10;

		public static int Ground = 240, LeftStart = 300, RightStart = 470;
		public static Vector2 Camera;

		public Kafe() : base()
		{
			Kafe.Me = this;
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = CrtWidth;
			graphics.PreferredBackBufferHeight = CrtHeight;
			if (CrtWidth == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width && CrtHeight == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
			{
				Window.IsBorderless = true;
				Window.Position = new Point(0, 0);
			}
			Mix.Initialize();
			IsMouseVisible = true;
		}

		public static Effect GetEffect(string assetName)
		{
			if (!assetName.Contains("."))
				assetName += ".fxb";
			return new Effect(GfxDev, Mix.GetBytes(assetName));
		}

		protected override void Initialize()
		{
			Kafe.GfxDev = graphics.GraphicsDevice;
			SoundEngine.Initialize();
			Input = new global::Kafe.Input(this); 
			Components.Add(Input); 
			base.Initialize();
		}

		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			rawScreen = new RenderTarget2D(GraphicsDevice, ScreenWidth, ScreenHeight);

			ClutEffect = GetEffect("clut");

			//var felicia = new Character("felicia.json", 3);
			//var sakura = new Character("sakura.json", 1);

			//var arena = new Arena("locales\\ryu_street.json", felicia, sakura);
			//var arena = new Editor("locales\\mci_corridor.json", "sakura.json");
			//Components.Add(arena);
			Kafe.LoadingScreen = new LoadingScreen(Me);
			Kafe.LoadingScreen.Start(() => { Components.Add(new Editor("locales\\mci_corridor.json", "sakura.json")); });
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(rawScreen);
			GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(gameTime);
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);
			SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);
			SpriteBatch.Draw(rawScreen, new Rectangle(0, 0, CrtWidth, CrtHeight), new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
			SpriteBatch.End();
		}
	}

	public class LoadingScreen : DrawableGameComponent
	{
		private Action task;
		private int state = 0;

		public LoadingScreen(Game game) : base(game) { }

		public void Start(Action task)
		{
			this.task = task;
			state = 0;
			Kafe.Me.Components.Add(this);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (state == 1 && task != null)
			{
				task();
				task = null;
				state++;
			}
			else if (state == 2)
			{
				Kafe.Me.Components.Remove(this);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			Kafe.GfxDev.Clear(Color.Black);
			Kafe.SpriteBatch.Begin();
			Text.Draw(Kafe.SpriteBatch, 1, "Loading...", 8, Kafe.ScreenHeight - 24);
			Kafe.SpriteBatch.End();
			if (state == 0)
				state++;
		}
	}

#if WINDOWS || LINUX
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new Kafe())
				game.Run();
		}
	}
#endif
}
