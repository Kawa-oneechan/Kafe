using System;
using Microsoft.Xna.Framework;

namespace Kafe
{
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
			Kafe.Camera.Y = 16;
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
