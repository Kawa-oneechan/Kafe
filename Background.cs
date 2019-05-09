using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Kawa.Json;

/*
	private JsonObj background;
	private Texture2D backgroundTexture;
	private BackgroundTypes backgroundType;
	private int backgroundFloor;
 */

namespace Kafe
{
	public enum BackgroundTypes { Still, Loop, Parallax, Parallax2, SimpleScroll };

	class Background : DrawableGameComponent
	{
		private JsonObj json;
		public Texture2D Sheet { get; private set; }
		public BackgroundTypes Type { get; private set; }
		public int ParallaxFloor { get; private set; }

		public Background(string file) : base(Kafe.Me)
		{
			json = Mix.GetJson(file) as JsonObj;
			Sheet = Mix.GetTexture("locales\\" + (json["base"] as string));
			Type = (BackgroundTypes)Enum.Parse(typeof(BackgroundTypes), json["type"] as string, true);

			if (Type == BackgroundTypes.Parallax)
				ParallaxFloor = (int)(double)json["floor"];
		}

		public override void Draw(GameTime gameTime)
		{
			var batch = Kafe.SpriteBatch;
			batch.Begin();
			var dest = new Rectangle(0, 0, Kafe.ScreenWidth, Kafe.ScreenHeight);
			switch (Type)
			{
				case BackgroundTypes.Still:
					{
						var src = new Rectangle((int)Kafe.Camera.X, 0, 384, 224);
						batch.Draw(Sheet, dest, src, Color.White);
					}
					break;
				case BackgroundTypes.Parallax:
					{
						var src = new Rectangle((int)(Kafe.Camera.X / 8), 224, 448, 224);
						batch.Draw(Sheet, Vector2.Zero, src, Color.White);

						src = new Rectangle((int)Kafe.Camera.X, 0, Kafe.ScreenWidth, Kafe.ScreenHeight);
						batch.Draw(Sheet, dest, src, Color.White);
					}
					break;
			}
			batch.End();
		}
	}

	class Arena : Background
	{
		private const int StartingDistance = 80;

		private int timer = 0;
		public Character[] Characters { get; set; }

		public Arena(string file, Character left, Character right) : base(file)
		{
			left.Position = new Vector2(StartingDistance, Kafe.Ground);
			right.Position = new Vector2(Kafe.ScreenWidth - StartingDistance, Kafe.Ground);
			right.FacingLeft = true;
			left.Opponent = right;
			right.Opponent = left;
			Characters = new[] { left, right };
		}

		public override void Update(GameTime gameTime)
		{
			timer += (int)gameTime.ElapsedGameTime.Milliseconds;
			if (timer > Kafe.Speed)
			{
				timer = 0;
				if (Input.Left)
					Game.Window.Title = "!";
				foreach (var c in Characters)
					c.Update();
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			var batch = Kafe.SpriteBatch;
			batch.Begin();
			foreach (var c in Characters)
				c.Draw(batch);
			batch.End();
		}
	}

	class Editor : Background
	{
		public Character Subject { get; set; }
		private System.IO.FileSystemWatcher watcher;
		private string topMessage = string.Empty;
		private string keyScrollerText = "(S)tep anim   (C)enter   (B)oxes   up/down cycle animations";
		private int keyScroller = Kafe.ScreenWidth + 8;

		public Editor(string file, string charFile) : base(file)
		{
			try
			{
				var path = System.IO.Path.Combine("data", "fighters", charFile);
				if (System.IO.File.Exists(path))
				{
					watcher = new System.IO.FileSystemWatcher(System.IO.Path.GetDirectoryName(path), charFile);
					watcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
					watcher.Changed += (sender, e) =>
					{
					tryAgain:
						try
						{
							if (Subject == null)
								Subject = new Character(charFile, 0);
							else
								Subject.Reload(charFile, 0, true);
							SetupSubject();
						}
						catch (System.IO.IOException)
						{
							goto tryAgain;
						}
						catch (JsonException jEx)
						{
							topMessage = jEx.Message;
							Subject = null;
						}
					};
					watcher.EnableRaisingEvents = true;
				}
				Subject = new Character(charFile, 0);
				SetupSubject();
			}
			catch (JsonException jEx)
			{
				topMessage = jEx.Message;
				Subject = null;
			}
		}

		private void SetupSubject()
		{
			if (Subject == null)
				return;
			Subject.Position = new Vector2(Kafe.ScreenWidth / 2, Kafe.Ground);
			Subject.EditMode = true;
			Subject.ShowBoxes = true;
			topMessage = string.Empty;
		}

		public override void Update(GameTime gameTime)
		{
			if (Subject != null)
			{
				if (Input.WasJustReleased(Keys.S))
					Subject.Update();
				if (Input.WasJustReleased(Keys.C))
					Subject.Position = new Vector2(Kafe.ScreenWidth / 2, Kafe.Ground);
				if (Input.WasJustReleased(Keys.B))
					Subject.ShowBoxes = !Subject.ShowBoxes;
				if (Input.WasJustReleased(Keys.Down))
					Subject.CycleAnims(1);
				if (Input.WasJustReleased(Keys.Up))
					Subject.CycleAnims(-1);
			}
			keyScroller -= gameTime.ElapsedGameTime.Milliseconds / 10;
			if (keyScroller < 0 - (keyScrollerText.Length * 8))
				keyScroller = Kafe.ScreenWidth + 8;
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			var batch = Kafe.SpriteBatch;
			batch.Begin();
			if (Subject != null)
			Subject.Draw(batch);
			Text.Draw(batch, 1, "Edit mode", 4, Kafe.ScreenHeight - 4 - 24);
			Text.Draw(batch, 0, keyScrollerText, keyScroller, Kafe.ScreenHeight - 4 - 8);
			if (!string.IsNullOrWhiteSpace(topMessage))
				Text.Draw(batch, 0, topMessage, 4, 4, Color.Red);
			batch.End();
		}
	}
}
