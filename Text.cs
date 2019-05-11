using System;
using System.Collections.Generic;
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
			foreach (var entry in data)
			{
				var font = (JsonObj)entry;
				var sheet = Mix.GetTexture(font["sheet"] as string);
				var width = (int)(double)font["width"];
				var height = (int)(double)font["height"];
				var lineHeight = (int)(double)font["line"];
				var widths = (font["widths"] as List<object>);
				var newWidths = new int[widths.Count];
				for (var j = 0; j < newWidths.Length; j++)
					newWidths[j] = (int)(double)widths[j];
				fonts[i] = new Font(sheet, width, height, lineHeight, newWidths);
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
