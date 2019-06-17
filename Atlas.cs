using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kafe
{
	class Atlas
	{
		private Rectangle[] rects;
		
		public Atlas(object json)
		{
			rects  = FromJson(json);
		}

		public static Rectangle[] FromJson(object json)
		{
			if (!(json is List<object>))
				throw new InvalidCastException();
			var list = json as List<object>;
			var ret = new Rectangle[list.Count];
			for (var i = 0; i < ret.Length; i++)
			{
				var item = list[i] as List<object>;
				ret[i] = new Rectangle((int)(double)item[0], (int)(double)item[1], (int)(double)item[2], (int)(double)item[3]);
			}
			return ret;
		}

		public void Draw(SpriteBatch batch, Texture2D texture, int index, Vector2 position, SpriteEffects flip = SpriteEffects.None)
		{
			if (index >= rects.Length)
				throw new IndexOutOfRangeException();
			batch.Draw(texture, position, rects[index], Color.White, 0f, Vector2.Zero, 1f, flip, 1f);
		}

		public void Draw(SpriteBatch batch, Texture2D texture, int index, int x, int y, SpriteEffects flip = SpriteEffects.None)
		{
			batch.Draw(texture, new Vector2(x, y), rects[index], Color.White, 0f, Vector2.Zero, 1f, flip, 1f);
		}

	}
}
