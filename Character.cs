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
		private const int MaxPalettes = 4;

		private Texture2D sheet;

		private Texture2D[] sheets;

		private JsonObj json;
		private JsonObj animations;
		private JsonObj animation;
		private List<JsonObj> frames;
		private int currentFrame, totalFrames;

		//private string currentAnimName;

		private static Texture2D shadow;

		public Rectangle Image { get; set; }
		public Vector2 Position { get; set; }
		public Vector2 CelOffset { get; set; }
		public Vector2 Velocity { get; set; }
		public bool FacingLeft { get; set; }
		public Character Opponent { get; set; }

		public Character(string jsonFile, int palIndex)
		{
			if (palIndex >= MaxPalettes)
				palIndex = MaxPalettes - 1;

			json = Mix.GetJson("fighters\\" + jsonFile) as JsonObj;
			var baseName = json["base"] as string;
			//sheet = Mix.GetTexture("fighters\\" + baseName);

			sheets = new Texture2D[MaxPalettes];
			for (var i = 0; i < MaxPalettes; i++)
				sheets[i] = Mix.GetTexture("fighters\\" + baseName);
			sheet = sheets[palIndex];

			if (Mix.FileExists("fighters\\" + baseName + "-pal.png"))
			{
				var palettes = Mix.GetTexture("fighters\\" + baseName + "-pal.png");
				var numPals = palettes.Height;
				var paletteData = new Color[palettes.Width * palettes.Height];
				palettes.GetData(paletteData);
				var spriteData = new Color[sheet.Width * sheet.Height];
				for (var i = 1; i < numPals; i++)
				{
					sheets[i].GetData(spriteData);
					for (var j = 0; j < spriteData.Length; j++)
					{
						for (var p = 0; p < palettes.Width; p++)
						{
							var cFrom = paletteData[p];
							var cTo = paletteData[(i * palettes.Width) + p];
							if (spriteData[j] == cFrom)
								spriteData[j] = cTo;
						}
					}
					sheets[i].SetData(spriteData);
				}

				if (palIndex >= numPals)
					palIndex = numPals - 1;
			}

			animations = (JsonObj)json["animations"];
			var idx = 0;
			foreach (var a in animations)
				((JsonObj)a.Value)["index"] = idx++;
			animation = animations.ElementAt(0).Value as JsonObj;
			frames = ((List<object>)animation["frames"]).Cast<JsonObj>().ToList();
			totalFrames = frames.Count;
			currentFrame = 0;
			Position = new Vector2(160, 160);
			var image = ((List<object>)frames[currentFrame]["image"]).Select(x => (int)(double)x).ToArray();
			var offset = ((List<object>)frames[currentFrame]["offset"]).Select(x => (int)(double)x).ToArray();
			Image = new Rectangle(image[0], image[1], image[2], image[3]);
			CelOffset = new Vector2(offset[0], offset[1]);

			if (shadow == null)
				shadow = Mix.GetTexture("shadow");
		}

		private void SwitchTo(object anim)
		{
			if (anim == null)
			{
				//currentFrame = 0;
				//return;
				anim = 0.0;
			}
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

		public void Update()
		{
			var advance = (Input.Right && !FacingLeft) || (Input.Left && FacingLeft);
			var retreat = (Input.Left && !FacingLeft) || (Input.Right && FacingLeft);

			currentFrame++;
			if (currentFrame >= totalFrames)
			{
				var fallTos = ((List<object>)animation["fallTo"]);
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

		public void Draw(SpriteBatch batch)
		{
			batch.Draw(shadow, new Vector2(Position.X - 32, Kafe.Ground - 4), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
			//batch.Draw(sheet, Position.ToInteger() - CelOffset, Image, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

			var posX = (int)Position.X;
			var posY = (int)(Position.Y - CelOffset.Y);
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
		}
	}
}
