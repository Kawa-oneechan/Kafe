using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kawa.Json;

/* Cancels and Fallthroughs
 * ------------------------
 * XML
 * ---
 * missing: doesn't apply
 * zero: go to stdNormal
 * 
 * JSON
 * ----
 * zero: go to stdNormal
 * null: doesn't apply
 */

namespace Kafe
{
	class Character
	{
		private const int PaletteSize = 4;

		private static Dictionary<string, Texture2D> sheets = new Dictionary<string, Texture2D>();
		private static Dictionary<string, Texture2D> palettes = new Dictionary<string, Texture2D>();

		private Texture2D sheet, palette;

		private JsonObj json;
		private JsonObj animations;
		private JsonObj animation;
		private List<JsonObj> frames;
		private int currentFrame, totalFrames;
		private int posX, posY;

		//private string currentAnimName;

		private static Texture2D shadow, square;

		public Rectangle Image { get; set; }
		public int ColorSwap { get; set; }
		public Vector2 Position { get; set; }
		public Vector2 CelOffset { get; set; }
		public Vector2 Velocity { get; set; }
		public bool FacingLeft { get; set; }
		public Character Opponent { get; set; }

		public bool EditMode { get; set; }
		public bool ShowBoxes { get; set; }

		public Character(string jsonFile, int palIndex)
		{
			if (shadow == null)
				shadow = Mix.GetTexture("shadow");
			if (square == null)
				square = Mix.GetTexture("square");

			Reload(jsonFile, palIndex, false);
		}

		public void Reload(string jsonFile, int palIndex, bool refresh)
		{
			json = Mix.GetJson("fighters\\" + jsonFile, false) as JsonObj;
			var baseName = json["base"] as string;

			if (sheets.ContainsKey(baseName) && !refresh)
			{
				sheet = sheets[baseName];
				if (palettes.ContainsKey(baseName))
					palette = palettes[baseName];
			}
			else
			{
				sheet = Mix.GetTexture("fighters\\" + baseName);
				sheets[baseName] = sheet;

				if (Mix.FileExists("fighters\\" + baseName + "-pal.png"))
				{
					palette = Mix.GetTexture("fighters\\" + baseName + "-pal.png");
					palettes[baseName] = palette;
					var numPals = palette.Height / PaletteSize;
					var paletteData = new Color[palette.Width * palette.Height];
					palette.GetData(paletteData);
					var spriteData = new Color[sheet.Width * sheet.Height];
					sheet.GetData(spriteData);
					for (var j = 0; j < spriteData.Length; j++)
					{
						for (var p = 0; p < palette.Width / PaletteSize; p++)
						{
							var cFrom = paletteData[p * PaletteSize];
							var cTo = new Color(p, 0, 0, 16);
							if (spriteData[j] == cFrom)
								spriteData[j] = cTo;
						}
					}
					sheet.SetData(spriteData);

					if (palIndex >= numPals)
						palIndex %= numPals;
				}
			}

			ColorSwap = palIndex;

			animations = (JsonObj)json["animations"];
			var idx = 0;
			foreach (var a in animations)
			{
				((JsonObj)a.Value)["index"] = idx++;
				((JsonObj)a.Value)["name"] = a.Key;
			}
			if (!refresh)
				animation = animations.ElementAt(0).Value as JsonObj;
			else
				animation = animations[animation["name"] as string] as JsonObj;
			frames = ((List<object>)animation["frames"]).Cast<JsonObj>().ToList();
			totalFrames = frames.Count;
			if (!refresh || currentFrame >= totalFrames)
				currentFrame = 0;
			Position = new Vector2(160, 160);
			var image = ((List<object>)frames[currentFrame]["image"]).Select(x => (int)(double)x).ToArray();
			var offset = ((List<object>)frames[currentFrame]["offset"]).Select(x => (int)(double)x).ToArray();
			Image = new Rectangle(image[0], image[1], image[2], image[3]);
			CelOffset = new Vector2(offset[0], offset[1]);
		}

		private void SwitchTo(object anim)
		{
			if (anim == null)
				anim = 0.0;
			if (anim is double)
				anim = animations.ElementAt((int)(double)anim).Key;
			var newAnimation = (JsonObj)animations[(string)anim];
			if (newAnimation != animation)
			{
				animation = newAnimation;
				frames = ((List<object>)animation["frames"]).Cast<JsonObj>().ToList();
				currentFrame = 0;
			}
			totalFrames = frames.Count;
			if (currentFrame >= totalFrames)
				currentFrame = 0;
		}

		public void CycleAnims(int direction)
		{
			var currentAnim = (int)animation["index"];
			var nextAnim = currentAnim + direction;
			if (nextAnim == animations.Count)
				nextAnim = 0;
			else if (nextAnim == -1)
				nextAnim = animations.Count - 1;
			SwitchTo((double)nextAnim);
			var image = ((List<object>)frames[currentFrame]["image"]).Select(x => (int)(double)x).ToArray();
			var offset = ((List<object>)frames[currentFrame]["offset"]).Select(x => (int)(double)x).ToArray();
			Image = new Rectangle(image[0], image[1], image[2], image[3]);
			CelOffset = new Vector2(offset[0], offset[1]);
		}

		public void Update()
		{
			var advance = (Input.Right && !FacingLeft) || (Input.Left && FacingLeft);
			var retreat = (Input.Left && !FacingLeft) || (Input.Right && FacingLeft);

			currentFrame++;
			if (currentFrame >= totalFrames)
			{
				var fallTos = ((List<object>)animation["fallTo"]);
				if (EditMode)
				{
					if (fallTos[0] != null)
						SwitchTo(fallTos[0]);
				}
				else
				{
					if (Input.A && fallTos[1] != null)
						SwitchTo(fallTos[1]);
					else if (Input.B && fallTos[2] != null)
						SwitchTo(fallTos[2]);
					else if (Input.C && fallTos[3] != null)
						SwitchTo(fallTos[3]);
					else if (Input.D && fallTos[4] != null)
						SwitchTo(fallTos[4]);
					else if (Input.E && fallTos[5] != null)
						SwitchTo(fallTos[5]);
					else if (Input.F && fallTos[6] != null)
						SwitchTo(fallTos[6]);
					else if (advance && fallTos[7] != null)
						SwitchTo(fallTos[7]);
					else if (retreat && fallTos[8] != null)
						SwitchTo(fallTos[8]);
					else if (Input.Up && fallTos[9] != null)
						SwitchTo(fallTos[10]);
					else if (Input.Down && fallTos[9] != null)
						SwitchTo(fallTos[10]);
					else if (fallTos[0] != null)
						SwitchTo(fallTos[0]);
				}
			}

			if (!EditMode)
			{
				var cancelTos = ((List<object>)animation["cancelTo"]);
				if (Input.A && cancelTos[1] != null)
					SwitchTo(cancelTos[1]);
				else if (Input.B && cancelTos[2] != null)
					SwitchTo(cancelTos[2]);
				else if (Input.C && cancelTos[3] != null)
					SwitchTo(cancelTos[3]);
				else if (Input.D && cancelTos[4] != null)
					SwitchTo(cancelTos[4]);
				else if (Input.E && cancelTos[5] != null)
					SwitchTo(cancelTos[5]);
				else if (Input.F && cancelTos[6] != null)
					SwitchTo(cancelTos[6]);
				else if (advance && cancelTos[7] != null)
					SwitchTo(cancelTos[7]);
				else if (retreat && cancelTos[8] != null)
					SwitchTo(cancelTos[8]);
				else if (Input.Up && cancelTos[9] != null)
					SwitchTo(cancelTos[9]);
				else if (Input.Down && cancelTos[10] != null)
					SwitchTo(cancelTos[10]);
				else if (!Input.Anything && cancelTos[0] != null)
					SwitchTo(cancelTos[0]);
			}

			if (currentFrame >= frames.Count)
				currentFrame = 0;

			var image = ((List<object>)frames[currentFrame]["image"]).Select(x => (int)(double)x).ToArray();
			var offset = ((List<object>)frames[currentFrame]["offset"]).Select(x => (int)(double)x).ToArray();
			Image = new Rectangle(image[0], image[1], image[2], image[3]);
			CelOffset = new Vector2(offset[0], offset[1]);
			if (frames[currentFrame].ContainsKey("impulse"))
			{
				var impulse = ((List<object>)frames[currentFrame]["impulse"]).Select(x => (float)(double)x).ToArray();
				Velocity += new Vector2(impulse[0], impulse[1]);
			}

			if ((int)animation["index"] == 0)
			{
				Velocity = new Vector2(0, Velocity.Y); //grind to a halt
				if (Opponent != null && !EditMode)
					FacingLeft = (Position.X > Opponent.Position.X);
			}

			Position = new Vector2(Position.X, Position.Y + Velocity.Y);
			if (Position.Y < Kafe.Ground)
			{
				Velocity += new Vector2(0, 4);
			}
			else
			{
				if (Velocity.Y > 20)
					Velocity = new Vector2(Velocity.X, -Velocity.Y / 3);
				else
					Velocity = new Vector2(Velocity.X, 0);
				Position = new Vector2(Position.X, Kafe.Ground);
			}

			Position = new Vector2(Position.X + (FacingLeft ? -Velocity.X : Velocity.X), Position.Y);
			if (Velocity.X > 5)
				Velocity -= new Vector2(2.5f, 0);
			else if (Velocity.X > 1)
				Velocity -= new Vector2(1, 0);
			else if (Velocity.X > 0)
				Velocity -= new Vector2(0.5f, 0);
			if (Velocity.X < -5)
				Velocity += new Vector2(2.5f, 0);
			else if (Velocity.X < -1)
				Velocity += new Vector2(1, 0);
			else if (Velocity.X < 0)
				Velocity += new Vector2(0.5f, 0);
		}

		public void DrawShadow(SpriteBatch batch)
		{
			batch.Draw(shadow, new Vector2(Position.X - 32, Kafe.Ground - 4), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
		}

		public void Draw(SpriteBatch batch)
		{
			if (palette != null)
			{
				batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, Kafe.ClutEffect);
				Kafe.ClutEffect.Parameters["PALETTE"].SetValue(palette);
				Kafe.ClutEffect.Parameters["PALETTES"].SetValue((float)palette.Height / PaletteSize);
				Kafe.ClutEffect.Parameters["COLORS"].SetValue((float)palette.Width / PaletteSize);
				Kafe.ClutEffect.Parameters["INDEX"].SetValue((float)ColorSwap);
			}
			else
				batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);

			posX = (int)Position.X;
			posY = (int)(Position.Y - CelOffset.Y);
			var fx = SpriteEffects.None;
			if (!FacingLeft)
			{
				posX -= (int)CelOffset.X;
			}
			else
			{
				posX += (int)CelOffset.X;
				posX -= Image.Width;
				fx = SpriteEffects.FlipHorizontally;
			}

			batch.Draw(sheet, new Vector2(posX, posY), Image, Color.White, 0.0f, Vector2.Zero, 1.0f, fx, 0.0f);
			batch.End();
		}

		public void DrawEditStuff(SpriteBatch batch)
		{
			if (ShowBoxes && frames[currentFrame].ContainsKey("boxes"))
			{
				var src = new Rectangle(0, 0, 32, 32);
				foreach (var box in ((List<object>)frames[currentFrame]["boxes"]).Cast<JsonObj>())
				{
					var rect = ((List<object>)box["rect"]).Select(x => (int)(double)x).ToArray();
					batch.Draw(square, new Rectangle(rect[0] + posX, rect[1] + posY, rect[2], rect[3]), src, ((bool)box["hit"]) ? Color.Blue : Color.Red);
				}
			}

			if (EditMode)
			{
				var fallTo0 = ((List<object>)animation["fallTo"])[0];
				var fallToA = default(JsonObj);
				if (fallTo0 == null)
					fallTo0 = 0.0;
				if (fallTo0 is double)
					fallToA = animations.ElementAt((int)(double)fallTo0).Value as JsonObj;
				else
					fallToA = animations[fallTo0 as string] as JsonObj;
				var fallTo = string.Format("{0} \"{1}\"", fallToA["index"], fallToA["name"]);
				Text.Draw(batch, 0,
					string.Format("anim {0} \"{1}\", color {2}\nframe {3} of {4}\nfall to {5}",
					animation["index"], animation["name"], ColorSwap, currentFrame, totalFrames, fallTo),
					2, 2);
			}
		}
	}
}
