using System;
using System.Collections.Generic;
using Kawa.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{
	class TitleBackground : DrawableGameComponent
	{
		private Texture2D sheet, backdrop;
		private int lineAnim, backdropAnim;

		private static int gridStart = 49;

		public TitleBackground() : base(Kafe.Me)
		{
			sheet = Mix.GetTexture("menu.png");
			backdrop = Mix.GetTexture("menu_back.png");
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			backdropAnim++;
			if (backdropAnim >= backdrop.Width * 16)
				backdropAnim = 0;
			lineAnim++;
			if (lineAnim == 64)
				lineAnim = 0;
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			var batch = Kafe.SpriteBatch;
			batch.Begin();
			var dst = new Rectangle(-(backdropAnim / 16), 0, backdrop.Width, Kafe.ScreenHeight);
			batch.Draw(backdrop, dst, Color.White);
			dst.Offset(backdrop.Width, 0);
			batch.Draw(backdrop, dst, Color.White);
			var src = new Rectangle(0, 0, 384, 49);
			dst = new Rectangle(0, Kafe.ScreenHeight - gridStart, Kafe.ScreenWidth, gridStart);
			batch.Draw(sheet, dst, src, Color.White);
			src = new Rectangle(1, 50, 14, 14);
			dst = new Rectangle(0, Kafe.ScreenHeight - gridStart + (lineAnim / 4), Kafe.ScreenWidth, 14);
			for (var i = 0; i < 4; i++)
			{
				batch.Draw(sheet, dst, src, Color.White);
				dst.Offset(0, 16);
			}
			batch.End();
		}
	}

	class TitleScreen : DrawableGameComponent
	{
		private Texture2D title, logo;
		private int anim, selection;
		private bool skipLogo;
		private string[] captions = new[] { "SOLO GAME", "VERSUS GAME", "QUIT" };

		public TitleScreen(bool skipLogo) : base(Kafe.Me)
		{
			title = Mix.GetTexture("title_logo.png");
			logo = Mix.GetTexture("firrhna_logo.png");
			selection = 0;
			this.skipLogo = skipLogo;
			if (skipLogo)
				anim = 64;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (gameTime.TotalGameTime.Seconds > 2 || skipLogo)
				if (anim < 128)
					anim++;

			if (anim < 64)
				return;

			if (Input.Controls[0].TrgUp)
			{
				if (selection == 0)
					selection = captions.Length;
				selection--;
			}
			else if (Input.Controls[0].TrgDown)
			{
				selection++;
				if (selection == captions.Length)
					selection = 0;
			}
			else if (Input.WasJustReleased(Keys.Escape))
			{
				Input.Flush();
				if (selection < 2)
					selection = 2;
				else
					Kafe.AskToQuit();
			}
			else if (Input.WasJustReleased(Keys.Enter) || Input.Controls[0].TrgA)
			{
				Input.Flush();
				switch (selection)
				{
					case 0: //Story Mode
						Kafe.DoTransition(false, () =>
						{
							Kafe.Me.Components.Remove(this);
							LoadingScreen.Start(() =>
							{
								var charSelect = new CharacterSelect(false) { Enabled = false };
								Kafe.Me.Components.Add(charSelect);
								Kafe.DoTransition(true, () => { charSelect.Enabled = true; });
							});
						});
						break;
					case 1: //Versus Mode
						Kafe.DoTransition(false, () =>
						{
							Kafe.Me.Components.Remove(this);
							LoadingScreen.Start(() =>
							{
								var charSelect = new CharacterSelect(true) { Enabled = false };
								Kafe.Me.Components.Add(charSelect);
								Kafe.DoTransition(true, () => { charSelect.Enabled = true; });
							});
						});
						break;
					case 2: //Quit
						Kafe.AskToQuit();
						break;
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			var batch = Kafe.SpriteBatch;
			if (gameTime.TotalGameTime.Seconds < 2)
			{
				batch.Begin();
				batch.Draw(logo, Vector2.Zero, Color.White);
				batch.End();
				return;
			}

			base.Draw(gameTime);
			batch.Begin();
			var dst = new Vector2(Kafe.ScreenWidth / 2, Kafe.ScreenHeight / 2);
			var center = new Vector2(title.Width / 2, title.Height / 2);

			batch.Draw(title, dst, null, Color.White, 0.0f, center, (anim < 32) ? anim / 32f : 1f, SpriteEffects.None, 0);

			if (anim >= 64)
			{
				const int left = Kafe.ScreenWidth / 2;
				var top = 160;
				for (var i = 0; i < captions.Length; i++)
				{
					Text.DrawEx(batch, 1, captions[i], left, top, Alignment.Center, 0, (i == selection) ? Color.White : Color.Gray);
					top += 20;
				}
			}

			batch.End();
		}
	}
}
