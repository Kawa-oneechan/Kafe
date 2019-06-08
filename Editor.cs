using System;
using System.IO;
using Kawa.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{
	class Editor : Background
	{
		public Character Subject
		{
			get { return Characters[0]; }
			set { Characters[0] = value; }
		}
		private FileSystemWatcher watcher;
		private string topMessage = string.Empty;
		private bool stepMode = true;
		private int timer = 0;

		public Editor(string file, string charFile) : base(file)
		{
			Kafe.Camera.Y = 0;
			Characters = new Character[1];
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
			Subject.ShowBoxes = false;
			Subject.Controls = Input.Controls[0];
			topMessage = string.Empty;
		}

		public override void Update(GameTime gameTime)
		{
			if (Subject != null)
			{
				if (!stepMode)
				{
					timer += (int)gameTime.ElapsedGameTime.Milliseconds;
					if (timer > Kafe.Speed)
					{
						timer = 0;
						Subject.Update();
					}
				}

				if (Input.WasJustReleased(Keys.Tab))
					stepMode = !stepMode;
				if (Input.IsHeld(Keys.Q))
				{
					if (Input.WasJustReleased(Keys.Down))
						Subject.CycleAnims(1);
					else if (Input.WasJustReleased(Keys.Up))
						Subject.CycleAnims(-1);
				}
				if (Input.WasJustReleased(Keys.W))
				{
					stepMode = true;
					Subject.FrameDelay = 0;
					Subject.Update();
				}

				if (Input.WasJustReleased(Keys.E))
					Subject.Position = new Vector2(Kafe.ScreenWidth / 2, Kafe.Ground);
				if (Input.WasJustReleased(Keys.R))
					Subject.ShowBoxes = !Subject.ShowBoxes;
				
				if (Input.WasJustReleased(Keys.OemPlus) && Subject.ColorSwap < Subject.ColorSwaps)
					Subject.ColorSwap++;				
				else if (Input.WasJustReleased(Keys.OemMinus) && Subject.ColorSwap > 1)
					Subject.ColorSwap--;
				
				if (stepMode && Input.IsHeld(Keys.LeftAlt))
					Subject.HandleOffsetEdit();
				else if (stepMode && Input.IsHeld(Keys.RightAlt))
					Subject.HandleBoxEdit();

				if (Input.IsHeld(Keys.D1))
				{
					var shift = Input.IsHeld(Keys.LeftControl) ? 16 : 8;
					if (Input.IsHeld(Keys.LeftShift))
					{
						if (Input.WasJustReleased(Keys.Left)) Kafe.Camera.X -= shift;
						if (Input.WasJustReleased(Keys.Right)) Kafe.Camera.X += shift;
					}
					else
					{
						if (Input.IsHeld(Keys.Left)) Kafe.Camera.X -= shift;
						if (Input.IsHeld(Keys.Right)) Kafe.Camera.X += shift;
					}
					Kafe.Me.Window.Title = Kafe.Camera.X.ToString();
				}

				if (Input.WasJustReleased(Keys.Escape))
					Kafe.AskToQuit();
			}
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
				Subject.DrawEditStuff(batch, stepMode);
				batch.End();
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			var batch = Kafe.SpriteBatch;
			batch.Begin();

			if (stepMode && Input.IsHeld(Keys.LeftAlt))
			{
				Text.Draw(batch, 1, "Offset mode", 4, Kafe.ScreenHeight - 4 - 24);
				Text.Draw(batch, 0, "Arrow keys to shift.", 4, Kafe.ScreenHeight - 4 - 8);
			}
			else if (stepMode && Input.IsHeld(Keys.RightAlt))
			{
				Text.Draw(batch, 1, "Box mode", 4, Kafe.ScreenHeight - 4 - 24 - 8);
				Text.Draw(batch, 0, "PgUp/Dn to select, Ins/Del to add/remove\nArrows to move, RShift-arrows to size.", 4, Kafe.ScreenHeight - 4 - 8 - 8);
			}
			else if (Input.IsHeld(Keys.OemTilde))
			{
				Text.Draw(batch, 1, "Scroll mode", 4, Kafe.ScreenHeight - 4 - 24);
				Text.Draw(batch, 0, "Arrow keys to scroll. Hold Ctrl for large increments, LShift to step.", 4, Kafe.ScreenHeight - 4 - 8);
			}
			else if (Input.IsHeld(Keys.Q))
			{
				Text.Draw(batch, 1, "Animation mode", 4, Kafe.ScreenHeight - 4 - 24);
				Text.Draw(batch, 0, "Up/down to switch animations.", 4, Kafe.ScreenHeight - 4 - 8);
			}
			else
			{
				Text.Draw(batch, 1, "Edit mode", 4, Kafe.ScreenHeight - 4 - 24 - 8);
				Text.Draw(batch, 0, "Q anim   LAlt offsets   RAlt boxes  1 scroll\nTab play   W step   E reset   R boxes   +/- colors", 4, Kafe.ScreenHeight - 4 - 8 - 8);
			}

			if (!string.IsNullOrWhiteSpace(topMessage))
				Text.Draw(batch, 0, topMessage, 4, 4, Color.Red);
			batch.End();
		}
	}
}
