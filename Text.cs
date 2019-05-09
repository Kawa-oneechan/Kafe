using System.Collections.Generic;
using System.Linq;
using Kawa.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kafe
{
	public class Font
	{
		public Texture2D Sheet { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int LineHeight { get; private set; }
		public int[] Widths { get; private set; }
		public Font(Texture2D sheet, int width, int height, int lineHeight, int[] widths)
		{
			Sheet = sheet;
			Width = width;
			Height = height;
			LineHeight = lineHeight;
			Widths = widths;
		}
	}

	public static class Text
	{
		private static Font[] fonts;

		private static void LoadFonts()
		{
			var data = Mix.GetJson("fonts") as List<object>;
			fonts = new Font[data.Count];
			var i = 0;
			foreach (var f in data.Cast<JsonObj>())
			{
				var sheet = Mix.GetTexture(f["sheet"] as string);
				var width = (int)(double)f["width"];
				var height = (int)(double)f["height"];
				var lineHeight = (int)(double)f["line"];
				var widths = (f["widths"] as List<object>).Select(w => (int)(double)w).ToArray();
				fonts[i] = new Font(sheet, width, height, lineHeight, widths);
				i++;
			}
		}

		public static void Draw(SpriteBatch batch, int font, string text, int left, int top, Color? color = null, int spacing = 0)
		{
			if (fonts == null)
				LoadFonts();
			if (font >= fonts.Length)
				font = 0;
			var f = fonts[font];
			var pos = new Vector2(left, top);
			text = text.Replace("\r\n", "\n");
			foreach (var c in text)
			{
				if (c == '\n')
				{
					pos = new Vector2(left, pos.Y + f.LineHeight);
					continue;
				}
				var src = new Rectangle((c - 32) * f.Width, 0, f.Widths[c - 32], f.Height);
				batch.Draw(f.Sheet, pos, src, color.HasValue ? color.Value : Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
				pos.X += f.Widths[c - 32] + spacing;
			}
		}
	}
}
