using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
	public enum StandardAnims
	{
		Idle,
		Advance,
		Retreat,
		Turn,
		JumpUp,
		JumpAdv,
		JumpRet,
		CrouchIn,
		Crouch,
		CrouchOut,
		CrouchTurn,
		Intro,
		Defeat,
		Time,
		Victory,
		HitGrdMid,
		HitGrdHigh,
		HitGrdLow,
		HitAir,
		Select,
	};

	public class Character
	{
		private const int PaletteSize = 4;

		private static Dictionary<string, Texture2D> sheets = new Dictionary<string, Texture2D>();
		private static Dictionary<string, Texture2D> palettes = new Dictionary<string, Texture2D>();
		private static Dictionary<string, Texture2D> icons = new Dictionary<string, Texture2D>();

		private Texture2D sheet, palette, icon;

		private JsonObj json;
		private List<JsonObj> animations;
		private JsonObj animation;
		private List<JsonObj> frames;
		private StandardAnims currentAnim;
		private int currentFrame, totalFrames;
		private int posX, posY;
		//keep these in sync yo
		private List<Rectangle> boxes;
		private List<bool> boxTypes;

		private static Texture2D shadow, editGreebles;
		private int editBox;
		private string copiedBoxes;

		public string Name { get; set; }

		public Rectangle Image { get; set; }
		public int ColorSwap { get; set; }
		public Vector2 Position { get; set; }
		public Vector2 CelOffset { get; set; }
		public Vector2 Velocity { get; set; }
		public bool FacingLeft { get; set; }
		public int FrameDelay { get; set; }
		public Character Opponent { get; set; }

		public bool EditMode { get; set; }
		public bool SelectMode { get; set; }
		public bool ShowBoxes { get; set; }

		public ControlSet Controls { get; set; }

		public Character(string jsonFile, int palIndex)
		{
			if (shadow == null)
				shadow = Mix.GetTexture("shadow");
			if (editGreebles == null)
				editGreebles = Mix.GetTexture("editor");

			boxes = new List<Rectangle>();
			boxTypes = new List<bool>();

			Reload(jsonFile, palIndex, false);
			Controls = Input.Controls[0];
		}

		public void Reload(string jsonFile, int palIndex, bool refresh)
		{
			json = Mix.GetJson("fighters\\" + jsonFile, false) as JsonObj;
			Name = json["name"] as string;
			var baseName = json["base"] as string;

			var keys = new string[json.Keys.Count];
			json.Keys.CopyTo(keys, 0);
			foreach (var key in keys)
			{
				if (json[key] is string && ((string)json[key]).StartsWith("import://"))
					json[key] = Mix.GetJson("fighters\\" + ((string)json[key]).Substring(9));
			}

			if (sheets.ContainsKey(baseName) && !refresh)
			{
				sheet = sheets[baseName];
				if (palettes.ContainsKey(baseName))
					palette = palettes[baseName];
				if (icons.ContainsKey(baseName))
					icon = icons[baseName];
			}
			else
			{
				sheet = Mix.GetTexture("fighters\\" + baseName);
				sheets[baseName] = sheet;

				if (Mix.FileExists("fighters\\" + baseName + "-pal.png"))
				{
					palette = Mix.GetTexture("fighters\\" + baseName + "-pal.png");
					palettes[baseName] = palette;

					//var timer1 = DateTime.Now;
					//Console.WriteLine("Palette start at {0}", timer1.ToLongTimeString());

					var numPals = palette.Height / PaletteSize;
					var paletteData = new int[palette.Width * palette.Height];
					palette.GetData(paletteData);
					var spriteData = new int[sheet.Width * sheet.Height];
					sheet.GetData(spriteData);
					for (var j = 0; j < spriteData.Length; j++)
					{
						for (var p = 0; p < palette.Width / PaletteSize; p++)
						{
							if (spriteData[j] == paletteData[p * PaletteSize])
								spriteData[j] = 0x10000000 + p;
						}
					}
					sheet.SetData(spriteData);

					if (palIndex >= numPals)
						palIndex %= numPals;

					//var timer2 = DateTime.Now;
					//Console.WriteLine("Palette end at {0}", timer2.ToLongTimeString());
					//var timer3 = timer2 - timer1;
					//Console.WriteLine("Delta {0}", timer3);
				}

				if (Mix.FileExists("fighters\\" + baseName + "-icon.png"))
				{
					icon = Mix.GetTexture("fighters\\" + baseName + "-icon.png");
					icons[baseName] = icon;
				}
			}

			ColorSwap = palIndex;

			animations = new List<JsonObj>();
			foreach (var a in (List<object>)json["animations"])
			{
				animations.Add((JsonObj)a);
			}
			if (!refresh)
				animation = animations[0] as JsonObj;
			else
				animation = animations[(int)currentAnim] as JsonObj;
			Position = new Vector2(160, 160);
			SetupFrames();
			if (!refresh || currentFrame >= totalFrames)
				currentFrame = 0;
			SetupImage();
		}

		public void SetupFrames()
		{
			frames = new List<JsonObj>();
			foreach (var f in (List<object>)animation["frames"])
				if (f is JsonObj)
					frames.Add((JsonObj)f);
			totalFrames = frames.Count;
			boxes.Clear();
			boxTypes.Clear();
		}

		public void SetupImage()
		{
			var cF = frames[currentFrame];
			if (cF.ContainsKey("img"))
			{
				var img = ((List<object>)cF["img"]);
				var spriteIndex = (int)(double)img[0];
				var image = (List<object>)((List<object>)json["sprites"])[spriteIndex];
				Image = new Rectangle((int)(double)image[0], (int)(double)image[1], (int)(double)image[2], (int)(double)image[3]);
				CelOffset = new Vector2((int)(double)img[1], (int)(double)img[2]);
				FrameDelay = (int)(double)img[3];

				if (cF.ContainsKey("boxes"))
				{
					boxes.Clear();
					boxTypes.Clear();
					foreach (var b in (List<object>)cF["boxes"])
					{
						var box = (List<object>)b;
						boxTypes.Add((bool)box[0]);
						boxes.Add(new Rectangle((int)(double)box[1], (int)(double)box[2], (int)(double)box[3], (int)(double)box[4]));
					}
				}
			}
			else
			{
				var image = ((List<object>)cF["image"]);
				var offset = ((List<object>)cF["offset"]);
				Image = new Rectangle((int)(double)image[0], (int)(double)image[1], (int)(double)image[2], (int)(double)image[3]);
				CelOffset = new Vector2((int)(double)offset[0], (int)(double)offset[1]);
				FrameDelay = 5;

			}
		}

		public void SwitchTo(object anim)
		{
			var newAnim = 0;
			if (anim == null)
				newAnim = 0;
			else if (anim is string)
				for (var i = 0; i < animations.Count; i++)
				{
					if ((string)animations[i]["name"] == (string)anim)
					{
						newAnim = i;
						break;
					}
				}
			else if (anim is double)
				newAnim = (int)(double)anim;
			else if (anim is StandardAnims)
				newAnim = (int)anim;
			else if (anim is int)
				newAnim = (int)anim;
			if (newAnim == -1)
				newAnim = (int)currentAnim;
			if (newAnim != (int)currentAnim)
			{
				currentAnim = (StandardAnims)newAnim;
				animation = animations[(int)currentAnim];
				SetupFrames();
				currentFrame = 0;
			}
			totalFrames = frames.Count;
			if (currentFrame >= totalFrames)
				currentFrame = 0;
		}

		public void CycleAnims(int direction)
		{
			var nextAnim = (int)currentAnim - direction;
			if (nextAnim == animations.Count)
				nextAnim = 0;
			else if (nextAnim == -1)
				nextAnim = animations.Count - 1;
			SwitchTo((double)nextAnim);
			SetupImage();
		}

		public void DecideNextAnim()
		{
			if (SelectMode)
				return;
			var advance = (Controls.Right && !FacingLeft) || (Controls.Left && FacingLeft);
			var retreat = (Controls.Left && !FacingLeft) || (Controls.Right && FacingLeft);
			var oldFacing = FacingLeft;
			if (Opponent != null && !EditMode)
				FacingLeft = (Position.X > Opponent.Position.X);

			switch ((StandardAnims)currentAnim)
			{
				case StandardAnims.Idle:
					if (FacingLeft != oldFacing) SwitchTo(StandardAnims.Turn);
					else if (Controls.Up)
					{
						if (advance) SwitchTo(StandardAnims.JumpAdv);
						else if (retreat) SwitchTo(StandardAnims.JumpRet);
						else SwitchTo(StandardAnims.JumpUp);
					}
					else if (advance) SwitchTo(StandardAnims.Advance);
					else if (retreat) SwitchTo(StandardAnims.Retreat);
					else if (Controls.Down) SwitchTo(StandardAnims.CrouchIn);
					break;
				case StandardAnims.Advance:
					if (!advance) SwitchTo(StandardAnims.Idle);
					else if (Controls.Up) SwitchTo(StandardAnims.JumpAdv);
					else if (Controls.Down) SwitchTo(StandardAnims.CrouchIn);
					break;
				case StandardAnims.Retreat:
					if (!retreat) SwitchTo(StandardAnims.Idle);
					else if (Controls.Up) SwitchTo(StandardAnims.JumpRet);
					else if (Controls.Down) SwitchTo(StandardAnims.CrouchIn);
					break;
				case StandardAnims.Turn:
					SwitchTo(StandardAnims.Idle);
					break;
				case StandardAnims.JumpUp:
				case StandardAnims.JumpAdv:
				case StandardAnims.JumpRet:
					if (Position.Y < Kafe.Ground)
					{
						//TODO: allow loop points
						currentFrame--;
					}
					else
						SwitchTo(StandardAnims.Idle);
					break;
				case StandardAnims.CrouchIn:
					SwitchTo(StandardAnims.Crouch);
					break;
				case StandardAnims.Crouch:
					if (!Controls.Down) SwitchTo(StandardAnims.CrouchOut);
					else if (FacingLeft != oldFacing) SwitchTo(StandardAnims.CrouchTurn);
					break;
				case StandardAnims.CrouchOut:
					SwitchTo(StandardAnims.Idle);
					break;
				default:
					{
						if (animation.ContainsKey("fallTo"))
						{
							if (animation["fallTo"] is List<object>)
							{
								var fallTos = ((List<object>)animation["fallTo"]);
								if (EditMode)
								{
									if (fallTos[0] != null)
										SwitchTo(fallTos[0]);
								}
								else
								{
									if (Controls.A && fallTos[1] != null)
										SwitchTo(fallTos[1]);
									else if (Controls.B && fallTos[2] != null)
										SwitchTo(fallTos[2]);
									else if (Controls.C && fallTos[3] != null)
										SwitchTo(fallTos[3]);
									else if (Controls.D && fallTos[4] != null)
										SwitchTo(fallTos[4]);
									else if (Controls.E && fallTos[5] != null)
										SwitchTo(fallTos[5]);
									else if (Controls.F && fallTos[6] != null)
										SwitchTo(fallTos[6]);
									else if (advance && fallTos[7] != null)
										SwitchTo(fallTos[7]);
									else if (retreat && fallTos[8] != null)
										SwitchTo(fallTos[8]);
									else if (Controls.Up && fallTos[9] != null)
										SwitchTo(fallTos[10]);
									else if (Controls.Down && fallTos[9] != null)
										SwitchTo(fallTos[10]);
									else if (fallTos[0] != null)
										SwitchTo(fallTos[0]);
								}
							}
							else
								SwitchTo(animation["fallTo"]);
						}
						else
						{
							//Do nothing, as if fallTo = -1.
						}
						break;
					}
			}
		}

		public void DecideCancelAnim()
		{
			if (SelectMode)
				return;
			var advance = (Controls.Right && !FacingLeft) || (Controls.Left && FacingLeft);
			var retreat = (Controls.Left && !FacingLeft) || (Controls.Right && FacingLeft);

			switch ((StandardAnims)currentAnim)
			{
				case StandardAnims.Idle:
					if (Controls.Up)
					{
						if (advance) SwitchTo(StandardAnims.JumpAdv);
						else if (retreat) SwitchTo(StandardAnims.JumpRet);
						else SwitchTo(StandardAnims.JumpUp);
					}
					else if (advance) SwitchTo(StandardAnims.Advance);
					else if (retreat) SwitchTo(StandardAnims.Retreat);
					else if (Controls.Down) SwitchTo(StandardAnims.CrouchIn);
					break;
				case StandardAnims.Advance:
					if (!advance) SwitchTo(StandardAnims.Idle);
					else if (Controls.Down) SwitchTo(StandardAnims.CrouchIn);
					break;
				case StandardAnims.Retreat:
					if (!retreat) SwitchTo(StandardAnims.Idle);
					else if (Controls.Down) SwitchTo(StandardAnims.CrouchIn);
					break;
				case StandardAnims.Turn:
					SwitchTo(StandardAnims.Idle);
					break;
				case StandardAnims.CrouchIn:
					SwitchTo(StandardAnims.Crouch);
					break;
				case StandardAnims.Crouch:
					if (!Controls.Down) SwitchTo(StandardAnims.CrouchOut);
					break;
				case StandardAnims.CrouchOut:
					SwitchTo(StandardAnims.Idle);
					break;
				default:
					{
						if (animation.ContainsKey("cancelTo"))
						{
							var cancelTos = ((List<object>)animation["cancelTo"]);
							if (Controls.A && cancelTos[1] != null)
								SwitchTo(cancelTos[1]);
							else if (Controls.B && cancelTos[2] != null)
								SwitchTo(cancelTos[2]);
							else if (Controls.C && cancelTos[3] != null)
								SwitchTo(cancelTos[3]);
							else if (Controls.D && cancelTos[4] != null)
								SwitchTo(cancelTos[4]);
							else if (Controls.E && cancelTos[5] != null)
								SwitchTo(cancelTos[5]);
							else if (Controls.F && cancelTos[6] != null)
								SwitchTo(cancelTos[6]);
							else if (advance && cancelTos[7] != null)
								SwitchTo(cancelTos[7]);
							else if (retreat && cancelTos[8] != null)
								SwitchTo(cancelTos[8]);
							else if (Controls.Up && cancelTos[9] != null)
								SwitchTo(cancelTos[9]);
							else if (Controls.Down && cancelTos[10] != null)
								SwitchTo(cancelTos[10]);
							else if (!Controls.Anything && cancelTos[0] != null)
								SwitchTo(cancelTos[0]);
						}
						break;
					}
			}
		}

		public void Update()
		{
			var advance = (Controls.Right && !FacingLeft) || (Controls.Left && FacingLeft);
			var retreat = (Controls.Left && !FacingLeft) || (Controls.Right && FacingLeft);

			if (FrameDelay-- <= 0)
			{
				currentFrame++;
				if (currentFrame >= totalFrames)
					DecideNextAnim();
				DecideCancelAnim();

				if (currentFrame >= frames.Count)
					currentFrame = 0;

				SetupImage();
				if (frames[currentFrame].ContainsKey("impulse"))
				{
					var impulse = ((List<object>)frames[currentFrame]["impulse"]);
					Velocity += new Vector2((float)(double)impulse[0], (float)(double)impulse[1]);
				}
				else if (frames[currentFrame].ContainsKey("velocity"))
				{
					var velocity = ((List<object>)frames[currentFrame]["velocity"]);
					Velocity = new Vector2((float)(double)velocity[0], (float)(double)velocity[1]);
				}
			}

			if (animation.ContainsKey("halt") && (bool)animation["halt"])
				Velocity = new Vector2(0, Velocity.Y); //grind to a halt

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
			/*
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
			*/
		}

		public void PreDraw()
		{
			posX = (int)Position.X - (Image.Width / 2);
			posY = (int)(Position.Y - Image.Height - CelOffset.Y);
			if (!FacingLeft)
			{
				posX += (int)CelOffset.X;
			}
			else
			{
				//TODO: something's fucky here
				posX -= (int)CelOffset.X;
			}
		}

		public void DrawShadow(SpriteBatch batch)
		{
			batch.Draw(shadow, new Rectangle(posX, Kafe.Ground - 4, Image.Width, 8), null, Color.White);
		}

		public void StartBatch(SpriteBatch batch)
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
		}

		public void Draw(SpriteBatch batch)
		{
			StartBatch(batch);
			batch.Draw(sheet, new Vector2(posX, posY), Image, Color.White, 0.0f, Vector2.Zero, 1.0f, FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
			batch.End();
		}

		public void DrawEditStuff(SpriteBatch batch)
		{
			if (ShowBoxes)
			{
				var src = new Rectangle(0, 0, 32, 32);
				for (var i = 0; i < boxes.Count; i++)
				{
					var box = boxes[i];
					batch.Draw(editGreebles, new Rectangle((int)(Position.X + box.X), (int)(Position.Y + box.Y), box.Width, box.Height),
						src, i == editBox ? (boxTypes[i] ? Color.CornflowerBlue : Color.Salmon) : boxTypes[i] ? Color.Blue : Color.Red);
				}
			}

			batch.Draw(editGreebles, Position - new Vector2(4), new Rectangle(132, 0, 9, 9), Color.Yellow);

			var fallTo0 = default(object);
			if (!animation.ContainsKey("fallTo"))
				fallTo0 = -1.0;
			else if (animation["fallTo"] is double)
				fallTo0 = (double)animation["fallTo"];
			else if (animation["fallTo"] is string)
				fallTo0 = (string)animation["fallTo"];
			else
				fallTo0 = ((List<object>)animation["fallTo"])[0];
			var fallToI = 0;
			if (fallTo0 == null)
				fallToI = 0;
			else if (fallTo0 is double)
				fallToI = (int)(double)fallTo0;
			else if (fallTo0 is string)
			{
				for (var i = 0; i < animations.Count; i++)
				{
					if ((string)animations[i]["name"] == (string)fallTo0)
					{
						fallToI = i;
						break;
					}
				}
			}
			var fallTo = "???";
			if (fallToI == -1)
				fallTo = "same";
			else
			{
				var fallToA = animations[fallToI] as JsonObj;
				fallTo = string.Format("{0} \"{1}\"", fallToI, fallToA["name"]);
			}
			var info = string.Format("anim {0} \"{1}\", color {2}\nframe {3} of {4}\nfall to {5}\noffset {6}",
				(int)currentAnim, animation["name"], ColorSwap, currentFrame, totalFrames, fallTo, CelOffset);
			if (Input.IsHeld(Keys.RightAlt))
			{
				if (!frames[currentFrame].ContainsKey("boxes"))
					info += "\n|c4|* Using previous boxset *|c0|";
				else
					info += "\n";
				for (var i = 0; i < boxes.Count; i++)
					info += string.Format("\n|c{0}|{1} {2}", (i == editBox ? 3 : 8), boxes[i], (boxTypes[i] ? 'H' : 'A'));
				if (!string.IsNullOrWhiteSpace(copiedBoxes))
					Text.Draw(batch, 2, copiedBoxes, Kafe.ScreenWidth / 2, 2);
			}

			Text.Draw(batch, 0, info, 2, 2);
		}

		public void DrawIcon(SpriteBatch batch, Rectangle position, bool recolor = true, bool flip = false)
		{
			if (recolor)
				StartBatch(batch);
			batch.Draw(icon, position, null, Color.White, 0.0f, Vector2.Zero, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
			if (recolor)
				batch.End();
		}

		public void HandleOffsetEdit()
		{
			var cF = frames[currentFrame];
			var control = Input.Controls[0];
			if (cF.ContainsKey("img"))
			{
				var offset = ((List<object>)cF["img"]);
				if (control.TrgUp) offset[2] = (double)offset[2] + 1;
				else if (control.TrgDown) offset[2] = (double)offset[2] - 1;
				else if (control.TrgLeft) offset[1] = (double)offset[1] - 1;
				else if (control.TrgRight) offset[1] = (double)offset[1] + 1;
				CelOffset = new Vector2((int)(double)offset[1], (int)(double)offset[2]);
			}
			else
			{
				var offset = ((List<object>)cF["offset"]);
				if (control.TrgUp) offset[1] = (double)offset[1] + 1;
				else if (control.TrgDown) offset[1] = (double)offset[1] - 1;
				else if (control.TrgLeft) offset[0] = (double)offset[0] - 1;
				else if (control.TrgRight) offset[0] = (double)offset[0] + 1;
				CelOffset = new Vector2((int)(double)offset[0], (int)(double)offset[1]);
			}
		}

		public void HandleBoxEdit()
		{
			var cF = frames[currentFrame];

			if (Input.WasJustReleased(Keys.V) && !string.IsNullOrWhiteSpace(copiedBoxes))
			{
				cF["boxes"] = Json5.Parse(copiedBoxes);
				SetupImage();
				return;
			}
			else if (Input.WasJustReleased(Keys.C) && cF.ContainsKey("boxes"))
			{
				copiedBoxes = cF["boxes"].Stringify();
				System.IO.File.WriteAllText("boxes.json", copiedBoxes); //consider having a full frame save key
				return;
			}
			else if (Input.WasJustReleased(Keys.X) && cF.ContainsKey("boxes"))
			{
				cF.Remove("boxes");
				SetupImage();
				return;
			}

			if (!cF.ContainsKey("boxes"))
				return;

			var boxes = cF["boxes"] as List<object>;
			if (Input.WasJustReleased(Keys.Insert)) //add box
			{
				((List<object>)cF["boxes"]).Add(new List<object>() { true, 0.0, -8.0, 8.0, 8.0 });
				SetupImage();
			}
			if (Input.WasJustReleased(Keys.Delete))
			{
				boxes.RemoveAt(editBox);
				if (editBox >= boxes.Count)
					editBox--;
				if (boxes.Count == 0)
					cF.Remove("boxes");
				SetupImage();
			}
			else if (Input.WasJustReleased(Keys.PageUp))
			{
				editBox++;
				if (editBox >= this.boxes.Count)
					editBox = 0;
			}
			else if (Input.WasJustReleased(Keys.PageDown))
			{
				if (editBox == 0)
					editBox = this.boxes.Count;
				editBox--;
			}

			if (boxes == null)
				return;

			var rect = ((List<object>)boxes[editBox]);

			if (Input.WasJustReleased(Keys.Up))
			{
				if (Input.IsHeld(Keys.RightShift))
					rect[4] = (double)rect[4] - 1;
				else
					rect[2] = (double)rect[2] - 1;
				SetupImage();
			}
			else if (Input.WasJustReleased(Keys.Down))
			{
				if (Input.IsHeld(Keys.RightShift))
					rect[4] = (double)rect[4] + 1;
				else
					rect[2] = (double)rect[2] + 1;
				SetupImage();
			}
			else if (Input.WasJustReleased(Keys.Left))
			{
				if (Input.IsHeld(Keys.RightShift))
					rect[3] = (double)rect[3] - 1;
				else
					rect[1] = (double)rect[1] - 1;
				SetupImage();
			}
			else if (Input.WasJustReleased(Keys.Right))
			{
				if (Input.IsHeld(Keys.RightShift))
					rect[3] = (double)rect[3] + 1;
				else
					rect[1] = (double)rect[1] + 1;
				SetupImage();
			}
		}
	}
}
