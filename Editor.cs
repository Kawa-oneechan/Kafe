using System;
using System.IO;
using Kawa.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{

	class Editor : Background
	{
		public Character Subject { get; set; }
		private FileSystemWatcher watcher;
		private string topMessage = string.Empty;
		private string keyScrollerText = "(S)tep anim   (C)enter   (B)oxes   up/down cycle animations";
		private int keyScroller = Kafe.ScreenWidth + 8;

		public Editor(string file, string charFile) : base(file)
		{
			Kafe.Camera.Y = 16;
			try
			{
				var path = Path.Combine("data", "fighters", charFile);
				if (File.Exists(path))
				{
					watcher = new FileSystemWatcher(Path.GetDirectoryName(path), charFile);
					watcher.NotifyFilter = NotifyFilters.LastWrite;
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
						catch (IOException)
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
				if (Input.WasJustReleased(Keys.OemPlus))
					Subject.ColorSwap++;
				if (Input.WasJustReleased(Keys.OemMinus) && Subject.ColorSwap > 0)
					Subject.ColorSwap--;
			}
			keyScroller -= gameTime.ElapsedGameTime.Milliseconds / 10;
			if (keyScroller < 0 - (keyScrollerText.Length * 8))
				keyScroller = Kafe.ScreenWidth + 8;
			base.Update(gameTime);
		}

		public override void DrawPlayers(GameTime gameTime)
		{
			var batch = Kafe.SpriteBatch;
			if (Subject != null)
			{
				Subject.PreDraw();
				batch.Begin();
				Subject.DrawShadow(batch);
				batch.End();
				Subject.Draw(batch);
				batch.Begin();
				Subject.DrawEditStuff(batch);
				batch.End();
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			var batch = Kafe.SpriteBatch;
			batch.Begin();
			Text.Draw(batch, 1, "Edit mode", 4, Kafe.ScreenHeight - 4 - 24);
			Text.Draw(batch, 0, keyScrollerText, keyScroller, Kafe.ScreenHeight - 4 - 8);
			if (!string.IsNullOrWhiteSpace(topMessage))
				Text.Draw(batch, 0, topMessage, 4, 4, Color.Red);
			batch.End();
		}
	}
}
