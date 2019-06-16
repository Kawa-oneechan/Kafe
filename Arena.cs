using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kafe
{
	class Arena : Background
	{
		private int timer = 0;
		private Texture2D bars;
		private float[] redHealth;
		private int state = 0;

		public Arena(string file, Character left, Character right) : base(file)
		{
			Console.WriteLine("Instantiating an Arena (regular two-player battle) with {0} against {1}...", left.Name, right.Name);
			bars = Mix.GetTexture("health_bars");
			left.Position = new Vector2(Kafe.LeftStart, Kafe.Ground);
			right.Position = new Vector2(Kafe.ScreenWidth - Kafe.RightStart, Kafe.Ground);
			right.FacingLeft = right.IsRight = true;
			left.Opponent = right;
			right.Opponent = left;
			left.Health = right.Health = 1.0f;
			left.SelectMode = false;
			right.SelectMode = false;
			left.EditMode = false;
			right.EditMode = false;
			left.Controls = Input.Controls[0];
			right.Controls = Input.Controls[1];
			left.SwitchTo(StandardAnims.Idle); //TODO: switch to Intro when both test characters
			right.SwitchTo(StandardAnims.Idle); //have proper Intro animations.
			left.SetupImage();
			right.SetupImage();
			Characters = new[] { left, right };
			Kafe.Camera.Y = 0;
			redHealth = new float[Characters.Length];
		}

		public override void Update(GameTime gameTime)
		{
			timer += (int)gameTime.ElapsedGameTime.Milliseconds;

			if (timer > Kafe.Speed)
			{
				timer = 0;
				for (var i = 0; i < Characters.Length; i++)
				{
					Characters[i].Update();

					if (Characters[i].Health > redHealth[i])
						redHealth[i] = Math.Min(Characters[i].Health, redHealth[i] + 0.01f);
					else if (Characters[i].Health < redHealth[i])
						redHealth[i] = Math.Max(Characters[i].Health, redHealth[i] - 0.01f);
				}
			}
/*			if (Input.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.H))
				Characters[1].Health -= Characters[1].Health / 3;
			if (Input.WasJustPressed(Microsoft.Xna.Framework.Input.Keys.J))
			{
				Characters[0].Health = Characters[1].Health = 1.0f;
				redHealth[0] = redHealth[1] = 0.0f;
			}
*/
			base.Update(gameTime);
		}

		public override void DrawPlayers(GameTime gameTime)
		{
			var batch = Kafe.SpriteBatch;
			batch.Begin();
			foreach (var c in Characters)
			{
				c.PreDraw();
				c.DrawShadow(batch);
			}
			batch.End();
			foreach (var c in Characters)
				c.Draw(batch);

			batch.Begin();
			foreach (var c in Characters)
			{
				c.DrawEditStuff(batch, false);
			}
			batch.End();
		}

		public override void Draw(GameTime gameTime)
		{
			var batch = Kafe.SpriteBatch;
			base.Draw(gameTime);
			var barPos = new Vector2((Kafe.ScreenWidth / 2) - 216, 8);
			var hpBar = new Rectangle(16, 89, 8, 6);

			batch.Begin();
			batch.Draw(bars, barPos, new Rectangle(0, 40, 432, 40), Color.White);
			batch.Draw(bars, barPos, new Rectangle(0, 0, 432, 40), Color.White);

			for (var i = 0; i < 2; i++)
			{
				var green = Characters[i].Health;
				var red = redHealth[i];
				var greenLen = (int)(green * 183);
				var redLen = (int)(red * 183);
				if (greenLen > redLen)
					greenLen = redLen;
				if (i == 0)
				{
					batch.Draw(bars, new Rectangle((int)barPos.X + 192 - redLen, (int)barPos.Y + 17, redLen, 6), hpBar, Color.Red);
					batch.Draw(bars, new Rectangle((int)barPos.X + 192 - greenLen, (int)barPos.Y + 17, greenLen, 6), hpBar, Color.White);
				}
				else
				{
					batch.Draw(bars, new Rectangle((int)barPos.X + 240, (int)barPos.Y + 17, redLen, 6), hpBar, Color.Red);
					batch.Draw(bars, new Rectangle((int)barPos.X + 240, (int)barPos.Y + 17, greenLen, 6), hpBar, Color.White);
				}
			}

			batch.End();
		}
	}
}
