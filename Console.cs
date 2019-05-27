using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{
	public static class Console
	{
		private static StreamWriter fileLog;

		public static TildeConsole TildeConsole;

		public static void Prepare()
		{
			fileLog = new StreamWriter("console.txt");
			fileLog.AutoFlush = true;
		}
		public static void Shutdown()
		{
			fileLog.Flush();
			fileLog.Close();
		}

		public static void WriteLine(string text)
		{
			if (TildeConsole != null)
				TildeConsole.WriteLine(text);
			fileLog.WriteLine(text);
		}

		public static void WriteLine(string format, params object[] arg)
		{
			fileLog.WriteLine(format, arg);
		}
	}

	public class TildeConsole : DrawableGameComponent
	{
		private SpriteBatch batch;
		private Texture2D background;
		//private Texture2D edge;
		private List<string> buffer;
		private string command;
		private int width, height;
		public int ScrollOffset { get; set; }

		public delegate void OnCommandEventHandler(TildeConsole sender, string command);
		public event OnCommandEventHandler OnCommand;
		public void RaiseOnCommandEvent(string command)
		{
			if (this.OnCommand != null)
				OnCommand(this, command);
		}

		#region Keymaps
		private static char[] lowers = new char[]
		{
		//	 0   1   2   3   4   5   6   7   8   9   A   B   C   D   E   F
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 0
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 1
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 2
			'0','1','2','3','4','5','6','7','8','9',' ',' ',' ',' ',' ',' ', // 3
			' ','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o', // 4
			'p','q','r','s','t','u','v','w','x','y','z',' ',' ',' ',' ',' ', // 5
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','.',' ', // 6
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 7
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 8
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 9
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // A
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',';','=',',','-','.','/', // B
			'`',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // C
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','[','\\',']','\'',' ',//D
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // E
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // F
		};
		private static char[] uppers = new char[]
		{
		//	 0   1   2   3   4   5   6   7   8   9   A   B   C   D   E   F
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 0
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 1
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 2
			')','!','@','#','$','%','^','&','*','(',' ',' ',' ',' ',' ',' ', // 3
			' ','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O', // 4
			'P','Q','R','S','T','U','V','W','X','Y','Z',' ',' ',' ',' ',' ', // 5
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','.',' ', // 6
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 7
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 8
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // 9
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // A
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',':','+','<','_','>','?', // B
			'~',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // C
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','{','|','}','"',' ', //D
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // E
			' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', // F
		};
		#endregion

		public TildeConsole(Game game) : base(game)
		{
			buffer = new List<string>();
			command = string.Empty;
		}

		public override void Initialize()
		{
			base.Initialize();
			Console.TildeConsole = this;
		}

		protected override void LoadContent()
		{
			batch = new SpriteBatch(GraphicsDevice);
			background = Mix.GetTexture("menu_back.png");
			//edge = Mix.GetTexture("Console edge");
			width = GraphicsDevice.PresentationParameters.BackBufferWidth;
			height = GraphicsDevice.PresentationParameters.BackBufferHeight / 2;
			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			if (!Visible)
				return;

			if (Input.WasJustPressed(Keys.PageUp) && ScrollOffset < buffer.Count - 1)
				ScrollOffset++;
			else if (Input.WasJustPressed(Keys.PageDown) && ScrollOffset > 0)
				ScrollOffset--;
			else if (Input.WasJustPressed(Keys.Back) && command.Length > 0)
				command = command.Remove(command.Length - 1);
			else if (Input.WasJustReleased(Keys.OemTilde))
			{
				command = string.Empty;
				Visible = false;
				Input.Flush();
			}
			else if (Input.WasJustPressed(Keys.Enter))
			{
				WriteLine("-> " + command);
				RaiseOnCommandEvent(command);
				command = "";
			}
			else
			{
				var ch = GetCharacter();
				if (ch != 0)
					command += ch;
			}

			if (command.StartsWith("`"))
				command = command.Substring(1);

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			batch.Begin();
			batch.Draw(background, new Rectangle(0, 0, width, height - 16), Color.White);
			//batch.Draw(edge, new Rectangle(0, height - 16, width, 32), Color.Black);

			var x = 2;
			var y = height - 24;
			Text.Draw(batch, 0, ">" + command + (gameTime.TotalGameTime.Milliseconds % 500 < 300 ? "_" : ""), x, y, Color.Yellow);
			int start = buffer.Count - ScrollOffset;
			for (int i = 0; i < 15; i++)
			{
				y -= 8;
				start--;
				if (start < 0)
					break;
				Text.Draw(batch, 0, buffer[start], x, y, Color.White);
			}

			batch.End();

			base.Draw(gameTime);
		}

		public void WriteLine(string text)
		{
			buffer.Add(text.Trim());
			if (buffer.Count > 255)
				buffer.RemoveAt(0);
		}

		public void WriteLine(string format, params object[] parms)
		{
			WriteLine(string.Format(format, parms));
		}

		private char GetCharacter()
		{
			for (int a = 0; a < lowers.Length; a++)
			{
				var key = (Keys)a;
				if (Input.WasJustReleased(key) && (lowers[a] != ' ' || a == 0x20))
				{
					if (Input.IsHeld(Keys.LeftShift) || Input.IsHeld(Keys.RightShift))
						return uppers[a];
					else
						return lowers[a];
				}
			}
			return (char)0;
		}
	}
}