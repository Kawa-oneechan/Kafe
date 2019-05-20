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

		public const int ScreenWidth = 480, ScreenHeight = 270;
		public const int Scale = 2;
		public const int CrtWidth = ScreenWidth * Scale, CrtHeight = ScreenHeight * Scale;
		public const int Speed = 10;

		public static int Ground = 240, LeftStart = 300, RightStart = 470;
		public static Vector2 Camera;
		public static bool Paused, ExitConfirm;

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
			//LoadingScreen.Start(() => { Components.Add(new Editor("locales\\mci_corridor.json", "sakura.json")); });
			LoadingScreen.Start(() => {
				Components.Add(new TitleBackground());
				Components.Add(new TitleScreen());
			});
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Input.WasJustReleased(Keys.Escape) && !ExitConfirm)
			{
				//Exit();
				ExitConfirm = true;
				ConfirmScreen.Ask("Are you sure you want to exit?", () => { Exit(); }, () => { ExitConfirm = Paused = false; Input.Flush(); });
			}
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
		private static LoadingScreen me;
		private Action task;
		private bool beenDrawn;

		public LoadingScreen(Game game) : base(game) { }

		public static void Start(Action task)
		{
			if (me == null)
				me = new LoadingScreen(Kafe.Me);

			me.task = task;
			me.beenDrawn = false;
			Kafe.Me.Components.Add(me);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (task == null)
			{
				Kafe.Me.Components.Remove(this);
				return;
			}
			if (beenDrawn)
			{
				task();
				task = null;
				Kafe.Me.Components.Remove(this);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			Kafe.GfxDev.Clear(Color.Black);
			Kafe.SpriteBatch.Begin();
			Text.Draw(Kafe.SpriteBatch, 1, "Loading...", 8, Kafe.ScreenHeight - 24);
			Kafe.SpriteBatch.End();
			beenDrawn = true;
		}
	}

	public class ConfirmScreen : DrawableGameComponent
	{
		private static ConfirmScreen me;
		private string prompt;
		private Action onYes, onNo;
		private Texture2D bits;
		private int animTime;
		private bool onRight;

		public ConfirmScreen(Game game) : base(game) { }

		protected override void LoadContent()
		{
			bits = Mix.GetTexture("confirm.png");
			base.LoadContent();
		}

		public static void Ask(string prompt, Action onYes, Action onNo)
		{
			if (me == null)
				me = new ConfirmScreen(Kafe.Me);

			me.prompt = prompt;
			me.onYes = onYes;
			me.onNo = onNo;
			me.animTime = 0;
			me.onRight = false;

			Kafe.Paused = true;
			Kafe.Me.Components.Add(me);
		}

		public static void Ask(string prompt, Action onYes)
		{
			Ask(prompt, onYes, () => { return; });
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (animTime < 8)
			{
				animTime++;
				return;
			}

			if (Input.WasJustReleased(Keys.Y))
			{
				Kafe.Me.Components.Remove(this);
				onYes();
			}
			else if (Input.WasJustReleased(Keys.N) || Input.WasJustReleased(Keys.Escape))
			{
				Kafe.Me.Components.Remove(this);
				onNo();
			}
			else if (Input.WasJustReleased(Keys.Enter))
			{
				Kafe.Me.Components.Remove(this);
				if (!onRight)
					onYes();
				else
					onNo();
			}
			else if (Input.TrgLeft || Input.TrgRight)
				onRight = !onRight;
		}

		public override void Draw(GameTime gameTime)
		{
			var batch = Kafe.SpriteBatch;
			batch.Begin();

			var height = animTime * 4;
			var top = (Kafe.ScreenHeight / 2) - (height / 2);
			var bottom = top  + height;
			var width = Kafe.ScreenWidth;

			var buttonDelta = 32;
			var yesPos = width - 16 - (buttonDelta * 2);

			var src = new Rectangle(1, 1, 15, 13);
			var dst = new Rectangle(0, top - 6, width, 13);
			batch.Draw(bits, dst, src, Color.White);
			dst.Offset(0, height);
			batch.Draw(bits, dst, src, Color.White);

			src = new Rectangle(17, 17, 14, 14);
			dst = new Rectangle(0, top, width, height);			
			batch.Draw(bits, dst, src, Color.White);

			if (animTime >= 8)
			{
				Text.Draw(batch, 0, prompt, 32, top + 8);
				src = new Rectangle(17, 1, 14, 14);
				dst = new Rectangle(yesPos + (onRight ? buttonDelta : 0), top + 12, 32, 16);
				batch.Draw(bits, dst, src, Color.White);
				Text.Draw(batch, 0, "YES", yesPos + 4, top + 16);
				Text.Draw(batch, 0, " NO", yesPos + 4 + buttonDelta, top + 16);
			}
			batch.End();
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
