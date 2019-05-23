using System;
using Microsoft.Xna.Framework;

namespace Kafe
{
	class Arena : Background
	{
		private int timer = 0;
		public Character[] Characters { get; set; }

		public Arena(string file, Character left, Character right) : base(file)
		{
			left.Position = new Vector2(Kafe.LeftStart, Kafe.Ground);
			right.Position = new Vector2(Kafe.ScreenWidth - Kafe.RightStart, Kafe.Ground);
			right.FacingLeft = true;
			left.Opponent = right;
			right.Opponent = left;
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
		}

		public override void Update(GameTime gameTime)
		{
			if (Kafe.Paused)
				return;
			timer += (int)gameTime.ElapsedGameTime.Milliseconds;
			if (timer > Kafe.Speed)
			{
				timer = 0;
				foreach (var c in Characters)
					c.Update();
			}
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
		}
	}
}
