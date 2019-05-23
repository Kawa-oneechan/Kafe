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
		private int anim, numPlayers;
		private int[] cursor;
		private Character[] selection;

		private Vector2[] positions = new[] { new Vector2(104, Kafe.Ground), new Vector2(484, Kafe.Ground) };
		private int[] namePositions = new[] { 40, 340 };

		public CharacterSelect(bool versus)
			: base(Kafe.Me)
		{
			stuff = Mix.GetTexture("menu.png");
			cursor = new[] { 1, 0 };
			selection = new[] { Kafe.Characters[1], Kafe.Characters[0] };
			numPlayers = versus ? 2 : 1;
			Kafe.CanExit = false;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (anim < 128)
				anim++;

			for (var i = 0; i < numPlayers; i++)
			{
				var control = Input.Controls[i];
				if (control.TrgLeft)
				{
					cursor[i]--;
					if (cursor[i] < 0)
						cursor[i] = Kafe.Characters.Length - 1;
					selection[i] = Kafe.Characters[cursor[i]];
				}
				else if (control.TrgRight)
				{
					cursor[i]++;
					if (cursor[i] >= Kafe.Characters.Length)
						cursor[i] = 0;
					selection[i] = Kafe.Characters[cursor[i]];
				}
				else if (control.TrgUp)
				{
					cursor[i] -= 5;
					if (cursor[i] < 0)
						cursor[i] = Kafe.Characters.Length - 1;
					selection[i] = Kafe.Characters[cursor[i]];
				}
				else if (control.TrgDown)
				{
					cursor[i] += 5;
					if (cursor[i] >= Kafe.Characters.Length)
						cursor[i] = 0;
					selection[i] = Kafe.Characters[cursor[i]];
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

			for (var i = 0; i < numPlayers; i++)
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
