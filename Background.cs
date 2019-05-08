using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
}
