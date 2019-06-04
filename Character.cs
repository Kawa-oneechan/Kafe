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

	public enum BoxTypes
	{
		Vulnerable,
		Attack,
		Push,
		ProjectileVulnerable,
		ProjectileAttack,
		Throw,
		Throwable,
	};

	public class Character
	{
		private static Color[] BoxColors = new[]
		{
			Color.Blue, Color.Red, Color.Lime, Color.MediumBlue, Color.MediumVioletRed, Color.Magenta, Color.DarkMagenta,
			Color.CornflowerBlue, Color.Salmon, Color.GreenYellow, Color.MediumSlateBlue, Color.DarkRed, Color.Violet, Color.DarkViolet
		};

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
		private List<BoxTypes> boxTypes;

		private static Texture2D shadow, editGreebles, editPixel;
		private int editBox;
		private string copiedBoxes;
		private string inputSequence;
		private int inputTimer;

		public string Name { get; set; }

		public Rectangle Image { get; set; }
		public int ColorSwap { get; set; }
		public int ColorSwaps { get; private set; }
		public Vector2 Position { get; set; }
		public Vector2 CelOffset { get; set; }
		public Vector2 Velocity { get; set; }
		public bool FacingLeft { get; set; }
		public int FrameDelay { get; set; }
		public bool Locked { get; set; }
		public Character Opponent { get; set; }
		public Vector4 MultiplyColor { get; set; }
		public Vector4 AddColor { get; set; }

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
			if (editPixel == null)
			{
				editPixel = new Texture2D(Kafe.GfxDev, 1, 1, false, SurfaceFormat.Color);
				editPixel.SetData(new[] { Color.White });
			}

			boxes = new List<Rectangle>();
			boxTypes = new List<BoxTypes>();

			Reload(jsonFile, palIndex, false);
			Controls = Input.Controls[0];
		}

		public void Reload(string jsonFile, int palIndex, bool refresh)
		{
			json = Mix.GetJson("fighters\\" + jsonFile, false) as JsonObj;
			Name = json.Path<string>("/name");
			var baseName = json.Path<string>("/base");

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

					var numPals = palette.Height;
					var paletteData = new int[palette.Width * palette.Height];
					palette.GetData(paletteData);
					var spriteData = new int[sheet.Width * sheet.Height];
					sheet.GetData(spriteData);
					for (var j = 0; j < spriteData.Length; j++)
					{
						for (var p = 0; p < palette.Width; p++)
						{
							if (spriteData[j] == paletteData[p])
								spriteData[j] = 0x10000000 + p;
						}
					}
					sheet.SetData(spriteData);

					ColorSwaps = numPals;
					if (palIndex >= numPals)
						palIndex %= numPals;
					if (palIndex == 0)
						palIndex++;

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

			animations = json.Path<List<JsonObj>>("/animations");
			if (!refresh)
				animation = animations[0];
			else
				animation = animations[(int)currentAnim];
			Position = new Vector2(160, 160);
			SetupFrames();
			if (!refresh || currentFrame >= totalFrames)
				currentFrame = 0;
			SetupImage();

			MultiplyColor = new Vector4(1);
			AddColor = new Vector4(0);

			inputSequence = string.Empty;
		}

		public void SetupFrames()
		{
			frames = animation.Path<List<JsonObj>>("/frames");
			totalFrames = frames.Count;
			boxes.Clear();
			boxTypes.Clear();
		}

		public void SetupImage()
		{
			var cF = frames[currentFrame];
			if (cF.ContainsKey("img"))
			{
				var img = cF.Path<int[]>("/img");
				var spriteIndex = img[0];
				var image = json.Path<int[]>("/sprites/" + spriteIndex);
				Image = new Rectangle(image[0], image[1], image[2], image[3]);
				CelOffset = new Vector2(img[1], img[2]);
				FrameDelay = img[3];

				if (cF.ContainsKey("boxes"))
				{
					boxes.Clear();
					boxTypes.Clear();
					foreach (var b in (List<object>)cF["boxes"])
					{
						var box = (List<object>)b;
						boxTypes.Add((BoxTypes)(int)(double)box[0]);

						var bX = (int)(double)box[1];
						var bY = (int)(double)box[2];
						var bW = (int)(double)box[3];
						var bH = (int)(double)box[4];
						if (FacingLeft)
							bX = -(bW + bX); //this was harder to figure out than you'd think.
						boxes.Add(new Rectangle(bX, bY, bW, bH));
					}
				}
			}
			else
			{
				//var image = cF.Path<int[]>("/image");
				//var offset = cF.Path<int[]>("/offset");
				//Image = new Rectangle((int)(double)image[0], (int)(double)image[1], (int)(double)image[2], (int)(double)image[3]);
				//CelOffset = new Vector2((int)(double)offset[0], (int)(double)offset[1]);
				Image = cF.Path<Rectangle>("/image");
				CelOffset = cF.Path<Vector2>("/offset");
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
								var fallTos = animation.Path<object[]>("/fallTo");
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
			if (SelectMode || Locked)
				return;
			var advance = (Controls.Right && !FacingLeft) || (Controls.Left && FacingLeft);
			var retreat = (Controls.Left && !FacingLeft) || (Controls.Right && FacingLeft);
			
			var oldFacing = FacingLeft;
			if (Opponent != null && !EditMode)
			{
				switch ((StandardAnims)currentAnim)
				{
					case StandardAnims.Idle:
					case StandardAnims.Crouch:
						FacingLeft = (Position.X > Opponent.Position.X);
						break;
				}
			}

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
					if (FacingLeft != oldFacing) SwitchTo(StandardAnims.Turn);
					else if (!Controls.Down) SwitchTo(StandardAnims.CrouchOut);
					break;
				case StandardAnims.CrouchOut:
					SwitchTo(StandardAnims.Idle);
					break;
				default:
					{
						if (animation.ContainsKey("cancelTo"))
						{
							var cancelTos = animation.Path<object[]>("/cancelTo");
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
			var trgAdvance = (Controls.TrgRight && !FacingLeft) || (Controls.TrgLeft && FacingLeft);
			var trgRetreat = (Controls.TrgLeft && !FacingLeft) || (Controls.TrgRight && FacingLeft);

			if (FrameDelay-- <= 0)
			{
				if (frames[currentFrame].ContainsKey("loop"))
					currentFrame += frames[currentFrame].Path<int>("/loop");

				currentFrame++;
				if (currentFrame >= totalFrames)
					DecideNextAnim();
				DecideCancelAnim();

				if (currentFrame >= frames.Count)
					currentFrame = 0;

				SetupImage();
				if (frames[currentFrame].ContainsKey("impulse"))
					Velocity += frames[currentFrame].Path<Vector2>("/impulse");
				else if (frames[currentFrame].ContainsKey("velocity"))
					Velocity = frames[currentFrame].Path<Vector2>("/velocity");
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

			if (Opponent != null)
			{
				//TODO Check for pushbox overlap
				//I don't think I can hack that. Getting them to mirror right was bad enough.
			}

			Position = new Vector2(Position.X + (FacingLeft ? -Velocity.X : Velocity.X), Position.Y);

			if (inputTimer > 0)
			{
				inputTimer--;
				if (inputTimer == 0)
					inputSequence = string.Empty;
			}
			if (!Locked)
			{
				if (Controls.TrgA && !inputSequence.EndsWith("A")) { inputSequence += "A"; inputTimer = 20; }
				if (Controls.TrgB && !inputSequence.EndsWith("B")) { inputSequence += "B"; inputTimer = 20; }
				if (Controls.TrgC && !inputSequence.EndsWith("C")) { inputSequence += "C"; inputTimer = 20; }
				if (Controls.TrgD && !inputSequence.EndsWith("D")) { inputSequence += "D"; inputTimer = 20; }
				if (Controls.TrgE && !inputSequence.EndsWith("E")) { inputSequence += "E"; inputTimer = 20; }
				if (Controls.TrgF && !inputSequence.EndsWith("F")) { inputSequence += "F"; inputTimer = 20; }
				if (trgAdvance && !inputSequence.EndsWith("f")) { inputSequence += "f"; inputTimer = 20; }
				if (trgRetreat && !inputSequence.EndsWith("b")) { inputSequence += "b"; inputTimer = 20; }
				if (Controls.TrgDown && !inputSequence.EndsWith("d")) { inputSequence += "d"; inputTimer = 20; }
				//TODO: find any listed moves found in the Inputs array and execute them.
				/* These should probably be listed (and/or sorted on load) by length of the input string.
				 * An example:
					{
						"name": "Weak punch",
						"sequence": "A",
						"stand": "WP-Stand",
						"standfar": "WP-Stand",
						"crouch": null,
						"air": null
					}
				 * Should inputSequence end with "A", assuming longer inputs have been exhausted already,
				 * we should determine our current state and ChangeAnim to whatever is specified.
				 */
			}
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
				posX += (int)CelOffset.X;
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
				var parms = Kafe.ClutEffect.Parameters;
				parms["PaletteMap"].SetValue(palette);
				parms["NumPalettes"].SetValue((float)palette.Height + 1);
				parms["NumColors"].SetValue((float)palette.Width);
				parms["TargetPalette"].SetValue((float)ColorSwap);
				parms["ColorMult"].SetValue(MultiplyColor);
				parms["ColorAdd"].SetValue(AddColor);
			}
			else
				batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);
		}

		public void Draw(SpriteBatch batch)
		{
			StartBatch(batch);
			batch.Draw(sheet, new Vector2(posX, posY), Image, Color.White, 0.0f, Vector2.Zero, 1.0f, FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
			batch.End();
			if (!EditMode)
			{
				batch.Begin();
				Text.DrawEx(batch, 0, inputSequence, FacingLeft ? Kafe.ScreenWidth - 4 : 4, 4, FacingLeft ? Alignment.Right : Alignment.Left);
				batch.End();
			}
		}

		private void DrawBorder(SpriteBatch batch, Rectangle rect, int thickness, Color color)
		{
			batch.Draw(editPixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
			batch.Draw(editPixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
			batch.Draw(editPixel, new Rectangle((rect.X + rect.Width - thickness), rect.Y, thickness, rect.Height), color);
			batch.Draw(editPixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
		}

		public void DrawEditStuff(SpriteBatch batch, bool stepMode)
		{
			if (ShowBoxes)
			{
				var src = new Rectangle(0, 0, 32, 32);
				for (var i = 0; i < boxes.Count; i++)
				{
					var box = boxes[i];
					var boxColor = BoxColors[(int)boxTypes[i] + (i == editBox ? 7 : 0)];
					var boxRect = new Rectangle((int)(Position.X + box.X), (int)(Position.Y + box.Y), box.Width, box.Height);
					batch.Draw(editGreebles, boxRect, src, boxColor);
					DrawBorder(batch, boxRect, 1, boxColor);
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
			var info = string.Format("anim {0} \"{1}\", color {2}\nframe {3} of {4}\n - image {5}, delay {6}\nfall to {7}\noffset {8}\n",
				(int)currentAnim, animation["name"], ColorSwap, currentFrame, totalFrames, Image.ToShort(), FrameDelay, fallTo, CelOffset);

			if (stepMode && Input.IsHeld(Keys.RightAlt))
			{
				if (!frames[currentFrame].ContainsKey("boxes"))
					info += "\n|c4|* Using previous boxset *|c0|";
				else
					info += "\n";
				for (var i = 0; i < boxes.Count; i++)
					info += string.Format("\n|c{0}|{1} {2}", (i == editBox ? 3 : 8), boxes[i], (new[]{"Vuln", "Atk", "Push", "PrVuln", "PrAtk", "Throw", "Grab"})[(int)boxTypes[i]]);
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
				var offset = cF.Path<double[]>("/img");
				if (control.TrgUp) offset[2] = (double)offset[2] + 1;
				else if (control.TrgDown) offset[2] = (double)offset[2] - 1;
				else if (control.TrgLeft) offset[1] = (double)offset[1] - 1;
				else if (control.TrgRight) offset[1] = (double)offset[1] + 1;
				CelOffset = new Vector2((int)(double)offset[1], (int)(double)offset[2]);
			}
			else
			{
				var offset = cF.Path<double[]>("/offset");
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
				((List<object>)cF["boxes"]).Add(new List<object>() { 0.0, 0.0, -8.0, 8.0, 8.0 });
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

			if (Input.WasJustReleased(Keys.End))
			{
				//boxTypes[editBox] = (BoxTypes)(((int)boxTypes[editBox] + 1) % 7);
				rect[0] = ((double)rect[0] + 1) % 7;
				SetupImage();
			}
			else if (Input.WasJustReleased(Keys.Up))
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
