using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{
	public class Kafe : Game
	{
		public string[] Args { get; set; }
		private RenderTarget2D rawScreen;

		GraphicsDeviceManager graphics;

		public static SpriteBatch SpriteBatch;
		public static Effect ClutEffect;
		public static Effect TransitionEffect;

		private static Texture2D transitionImage;
		private static TildeConsole tildeConsole;

		public static GraphicsDevice GfxDev { get; private set; }
		public static Kafe Me { get; private set; }
		public static Input Input { get; private set; }

		public static Character[] Characters { get; private set; }

		public const int ScreenWidth = 480, ScreenHeight = 270;
		public const int Scale = 2;
		public const int CrtWidth = ScreenWidth * Scale, CrtHeight = ScreenHeight * Scale;
		public const int Speed = 10;

		public static int Ground = 240, LeftStart = 100, RightStart = 100;
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
			Console.Prepare();
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

			tildeConsole = new TildeConsole(this);
			tildeConsole.Visible = false;
			tildeConsole.OnCommand += new TildeConsole.OnCommandEventHandler(TildeConsole_OnCommand);
			Components.Add(tildeConsole);

			Input = new global::Kafe.Input(this); 
			Components.Add(Input); 
			base.Initialize();
		}

		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			rawScreen = new RenderTarget2D(GraphicsDevice, ScreenWidth, ScreenHeight);

			ClutEffect = GetEffect("clut");
			TransitionEffect = GetEffect("transition");
			transitionImage = Mix.GetTexture("transition");

			var fighterFiles = Mix.GetFilesWithPattern("fighters\\*.char.json");
			var fighters = new List<string>();
			foreach (var f in fighterFiles)
			{
				fighters.Add(f.Substring(f.IndexOf('\\') + 1));
			}
			var arenaFiles = Mix.GetFilesWithPattern("locales\\*.bg.json");
			var arenas = new List<string>();
			foreach (var a in arenaFiles)
			{
				arenas.Add(a.Substring(a.IndexOf('\\') + 1));
			}

			if (Args.Length >= 3 && Args[0] == "/edit")
			{
				LoadingScreen.Start(() =>
				{
					Components.Add(new Editor("locales\\" + Args[1] + ".bg.json", Args[2] + ".char.json"));
				});
			}
			else if (Args.Length > 0 && Args[0] == "/quick")
			{
				var names = new string[2];
				var colors = new int[2];
				var rand = new Random();
				for (var i = 0; i < 2; i++)
					names[i] = fighters[rand.Next(fighters.Count)];
				var numNames = (Args.Length > 2) ? 2 : 1;
				for (var i = 0; i < numNames; i++)
				{
					names[i] = Args[i + 1].ToLowerInvariant();
					colors[i] = 1;
					if (names[i].Contains(","))
					{
						colors[i] = int.Parse(names[i].Substring(names[i].IndexOf(',') + 1));
						names[i] = names[i].Remove(names[i].IndexOf(','));
					}
					if (!names[i].EndsWith(".char.json"))
						names[i] += ".char.json";
				}
				//Ensure mirror matches have distinct colors
				if (names[0] == names[1] && colors[0] == colors[1])
					colors[1]++;
				var arena = "locales\\" + ((Args.Length > 3) ? Args[3] + ".bg.json" : arenas[rand.Next(arenas.Count)]);
				LoadingScreen.Start(() =>
				{
					var left = new Character(names[0], colors[0]);
					var right = new Character(names[1], colors[1]);
					Components.Add(new Arena(arena, left, right));
				});
			}
			else
			{
				LoadingScreen.Start(() =>
				{
					var fighterList = new List<Character>();
					foreach (var f in fighters)
						fighterList.Add(new Character(f, 0));
					Kafe.Characters = fighterList.ToArray();
					Components.Add(new TitleBackground());
					Components.Add(new TitleScreen(false));
				});
			}
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			if (tildeConsole.Visible)
			{
				tildeConsole.Update(gameTime);
				Input.Update(gameTime);
				return;
			}

			if (TransitionDelta > -1)
			{
				TransitionDelta += 0.05f;
				if (TransitionDelta >= 1.0f)
				{
					TransitionDelta = -1;
					if (OnTransitionFinish != null)
						OnTransitionFinish();
				}
				base.Update(gameTime);
				return;
			}
			base.Update(gameTime);

			if (Input.WasJustReleased(Keys.OemTilde))
			{
				tildeConsole.Visible = true;
				Input.Flush();
			}
			if (Input.WasJustPressed(Keys.F12))
				TakeScreenshot();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(rawScreen);
			GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(gameTime);
			if (TransitionDelta > -1)
			{
				TransitionEffect.Parameters["TransitionDelta"].SetValue(transitionIn ? TransitionDelta : 1 - TransitionDelta);
				SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, TransitionEffect);
				SpriteBatch.Draw(transitionImage, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
				SpriteBatch.End();
			}
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);
			SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);
			SpriteBatch.Draw(rawScreen, new Rectangle(0, 0, CrtWidth, CrtHeight), new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
			SpriteBatch.End();

			if (tildeConsole.Visible)
				tildeConsole.Draw(gameTime);
		}

		public static float TransitionDelta = -1;
		private static bool transitionIn;
		public static Action OnTransitionFinish;

		public static void DoTransition(bool goIn, Action onFinish)
		{
			TransitionDelta = 0;
			transitionIn = goIn;
			OnTransitionFinish = onFinish;
		}

		public static void PauseAll(bool enabled, GameComponent but)
		{
			foreach (var component in Kafe.Me.Components)
			{
				if (but != null && component == but)
					continue;
				if (component == Kafe.Input)
					continue;
				if (component is TitleBackground)
					continue;
				((GameComponent)component).Enabled = enabled;
			}
		}

		public static void AskToQuit()
		{
			ConfirmScreen.Ask("Are you sure you want to exit?", () => { Kafe.Me.Exit(); }, () => { Input.Flush(); });
		}

		public void TakeScreenshot()
		{
			var newThread = new System.Threading.Thread(ScreenshotThread);
			newThread.Start();
		}

		private void ScreenshotThread()
		{
			using (var stream = System.IO.File.OpenWrite(DateTime.Now.Ticks.ToString() + ".png"))
				rawScreen.SaveAsPng(stream, Kafe.ScreenWidth, Kafe.ScreenHeight);
		}

		private void TildeConsole_OnCommand(TildeConsole sender, string command)
		{
			command = command.Trim();
			if (string.IsNullOrWhiteSpace(command))
				return;
			var bits = command.ToLowerInvariant().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			switch (bits[0])
			{
				case "quit":
					this.Exit();
					return;
				case "screenshot":
					Console.WriteLine("Screenshot taken.");
					TakeScreenshot();
					return;
			}
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

			Kafe.PauseAll(false, me);
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

			var control = Input.Controls[0];
			if (Input.WasJustReleased(Keys.Y))
			{
				Kafe.PauseAll(true, null);
				Kafe.Me.Components.Remove(this);
				onYes();
			}
			else if (Input.WasJustReleased(Keys.N) || Input.WasJustReleased(Keys.Escape))
			{
				Kafe.PauseAll(true, null);
				Kafe.Me.Components.Remove(this);
				onNo();
			}
			else if (Input.WasJustReleased(Keys.Enter))
			{
				Kafe.PauseAll(true, null);
				Kafe.Me.Components.Remove(this);
				if (!onRight)
					onYes();
				else
					onNo();
			}
			else if (control.TrgLeft || control.TrgRight)
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
		static void Main(string[] args)
		{
			using (var game = new Kafe())
			{
				game.Args = args;
				game.Run();
			}
		}
	}
#endif
}
