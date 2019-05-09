using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Kawa.Json;

namespace Kafe
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Kafe : Game
	{
		private RenderTarget2D rawScreen; //, effectTargetA, effectTargetB;

		GraphicsDeviceManager graphics;

		public static SpriteBatch SpriteBatch;

		public static GraphicsDevice GfxDev { get; private set; }
		public static Kafe Me { get; private set; }
		public static Input Input { get; private set; }

		public const int ScreenWidth = 384, ScreenHeight = 224;
		public const int Scale = 2;
		public const int CrtWidth = ScreenWidth * Scale, CrtHeight = ScreenHeight * 2;
		public const int Speed = 45;

		public const int Ground = 194, LeftStart = 300, RightStart = 470;
		public static Vector2 Camera;

		public Kafe() : base()
		{
			Kafe.Me = this;
			Mix.Initialize();
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = CrtWidth;
			graphics.PreferredBackBufferHeight = CrtHeight;
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			Kafe.GfxDev = graphics.GraphicsDevice;
			Input = new global::Kafe.Input(this); 
			Components.Add(Input); 
			base.Initialize();
		}

		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			rawScreen = new RenderTarget2D(GraphicsDevice, ScreenWidth, ScreenHeight);
			//effectTargetA = new RenderTarget2D(GraphicsDevice, ScreenWidth, ScreenHeight);
			//effectTargetB = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

			var felicia = new Character("felicia.json", 0);
			var sakura = new Character("felicia.json", 1);

			//var arena = new Arena("locales\\vegas.json", felicia, sakura);
			var arena = new Editor("locales\\vegas.json", "morrigan.json");
			Components.Add(arena);
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (Input.IsHeld(Keys.Q)) Camera.X -= 4;
			if (Input.IsHeld(Keys.W)) Camera.X += 4;
			if (Camera.X < 80) Camera.X = 80;
			if (Camera.X > 432) Camera.X = 432;
			Window.Title = Camera.X.ToString();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(rawScreen);
			GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(gameTime);
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);
			SpriteBatch.Begin(SpriteSortMode.Immediate, null, /* BilinearBlurEnabled ? SamplerState.LinearClamp : */ SamplerState.PointClamp, null, null, /* ScanEffectEnabled ? scanlineEffect : */ null);
			SpriteBatch.Draw(rawScreen, new Rectangle(0, 0, CrtWidth, CrtHeight), new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
			SpriteBatch.End();
		}
	}

#if WINDOWS || LINUX
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			using (var game = new Kafe())
				game.Run();
		}
	}
#endif
}
