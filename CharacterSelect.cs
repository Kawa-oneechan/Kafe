using System;
using System.Collections.Generic;
using Kawa.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{
	class CharacterSelect : DrawableGameComponent
	{
		private Texture2D stuff;
		private int numPlayers;
		private int[] cursor;
		private Character[] selection;
		private bool[] locked;
		private long lockTimer = -1;
		private bool showTwo;

		private Vector2[] positions = new[] { new Vector2(100, Kafe.Ground), new Vector2(Kafe.ScreenWidth - 100, Kafe.Ground) };
		private int[] namePositions = new[] { 40, 340 };

		public CharacterSelect(bool versus) : base(Kafe.Me)
		{
			stuff = Mix.GetTexture("menu.png");
			cursor = new[] { 1, 0 };
			locked = new[] { false, false };
			selection = new[] { Kafe.Characters[1], Kafe.Characters[0] };
			numPlayers = versus ? 2 : 1;
			showTwo = versus;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			for (var i = 0; i < numPlayers; i++)
			{
				if (locked[i])
					continue;
				var control = Input.Controls[i];
				var oldCursor = cursor[i];
				if (control.TrgLeft)
				{
					cursor[i]--;
					if (cursor[i] < 0)
						cursor[i] = Kafe.Characters.Length - 1;
				}
				else if (control.TrgRight)
				{
					cursor[i]++;
					if (cursor[i] >= Kafe.Characters.Length)
						cursor[i] = 0;
				}
				else if (control.TrgUp)
				{
					cursor[i] -= 5;
					if (cursor[i] < 0)
						cursor[i] = Kafe.Characters.Length - 1;
				}
				else if (control.TrgDown)
				{
					cursor[i] += 5;
					if (cursor[i] >= Kafe.Characters.Length)
						cursor[i] = 0;
				}
				else if (control.TrgA || control.TrgB || control.TrgC || control.TrgD || control.TrgE || control.TrgF)
				{
					locked[i] = true;
					if (control.TrgA) selection[i].ColorSwap = 0;
					else if (control.TrgB) selection[i].ColorSwap = 1;
					else if (control.TrgC) selection[i].ColorSwap = 2;
					else if (control.TrgD) selection[i].ColorSwap = 3;
					else if (control.TrgE) selection[i].ColorSwap = 4;
					else if (control.TrgF) selection[i].ColorSwap = 5;
					selection[i].SwitchTo(StandardAnims.Select);

					//For single-player games, select another character at random for now.
					if (numPlayers == 1)
					{
						var j = i ^ 1;
						var rand = new Random();
						while (cursor[j] == cursor[i])
							cursor[j] = rand.Next(Kafe.Characters.Length);
						selection[j] = Kafe.Characters[cursor[j]];
						selection[j].ColorSwap = 0;
						selection[j].SwitchTo(StandardAnims.Select);
						//selection[j].Computer = true;
						locked[j] = true;
						showTwo = true;
					}
				}
				if (numPlayers > 1 && cursor[i] == cursor[i ^ 1])
					cursor[i] = oldCursor;
				else
					selection[i] = Kafe.Characters[cursor[i]];
			}
			if (Input.WasJustReleased(Keys.Escape))
			{
				Kafe.DoTransition(false, () =>
				{
					Kafe.Me.Components.Remove(this);
					LoadingScreen.Start(() =>
					{
						var title = new TitleScreen(true) { Enabled = false };
						Kafe.Me.Components.Add(title);
						Kafe.DoTransition(true, () => { title.Enabled = true; });
					});
				});
			}
			if (lockTimer == -1)
			{
				if (locked[0] && locked[1])
					lockTimer = 1000;
			}
			else
			{
				lockTimer -= gameTime.ElapsedGameTime.Milliseconds;
				if (lockTimer <= 0)
				{
					this.Enabled = false;
					Kafe.DoTransition(false, () =>
					{
						Kafe.Me.Components.Remove(this);
						for (var i = 0; i < Kafe.Me.Components.Count; i++)
						{
							if (Kafe.Me.Components[i] is TitleBackground)
							{
								Kafe.Me.Components.RemoveAt(i);
								break;
							}
						}
						LoadingScreen.Start(() =>
						{
							//TODO: base this on the other character.
							var arena = new Arena("locales\\mci_corridor.bg.json", selection[0], selection[1]) { Enabled = false };
							Kafe.Me.Components.Add(arena);
							Kafe.DoTransition(true, () => { arena.Enabled = true; });							
						});
					});
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			var batch = Kafe.SpriteBatch;
			batch.Begin();

			var x = 176;
			var y = 56;
			var dst = new Rectangle(x, y, 25, 25);
			var src = new Rectangle(16, 50, 25, 25);
			for (var i = 0; i < Kafe.Characters.Length; i++)
			{
				batch.Draw(stuff, dst, src, Color.White);
				dst.Inflate(-1, -1);
				Kafe.Characters[i].DrawIcon(batch, dst, false);
				dst.Inflate(1, 1);
				dst.Offset(24, 0);
				if (i % 5 == 4)
					dst.Offset(-(5 * 24), 24);
			}
			dst = new Rectangle(x - 6, y - 6, 36, 36);
			src = new Rectangle(42, 50, 36, 36);
			for (var i = 0; i < Kafe.Characters.Length; i++)
			{
				if (i == cursor[0])
					batch.Draw(stuff, dst, src, Color.Lime);
				if (numPlayers > 1 && i == cursor[1])
					batch.Draw(stuff, dst, src, Color.Red);
				dst.Offset(24, 0);
				if (i % 5 == 4)
					dst.Offset(-(5 * 24), 24);
			}
			batch.End();

			for (var i = 0; i < (showTwo ? 2 : 1); i++)
			{
				selection[i].Position = positions[i];
				selection[i].FacingLeft = (i == 1);
				selection[i].SelectMode = true;
				selection[i].Update();
				batch.Begin();
				selection[i].PreDraw();
				//selection[i].DrawShadow(batch);
				Text.Draw(batch, 1, selection[i].Name, namePositions[i], 80);
				batch.End();
				selection[i].Draw(batch);
			}

		}
	}
}
