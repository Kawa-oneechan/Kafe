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

	public enum Alignment { Left, Right, Center }

	public static class Text
	{
		private static Font[] fonts;
		private static Color[] colors = new[]
		{
			Color.White, Color.Blue, Color.Lime, Color.Cyan, Color.Red, Color.Magenta, Color.Yellow, Color.Gray,
			Color.DarkGray, Color.Navy, Color.Green, Color.Teal, Color.Maroon, Color.Purple, Color.Brown, Color.DarkGray
		};

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
			for (var i = 0; i < text.Length; i++)
			{
				var c = text[i];
				if (c == '\n')
				{
					pos = new Vector2(left, pos.Y + f.LineHeight);
					continue;
				}
				if (c == '|' && i < text.Length - 3 && text[i + 3] == '|')
				{
					switch (text[i + 1])
					{
						case 'c':
							color = colors[int.Parse(text.Substring(i + 2, 1), System.Globalization.NumberStyles.HexNumber)];
							i += 3;
							continue;
					}
				}
				var src = new Rectangle((c - 32) * f.Width, 0, f.Widths[c - 32], f.Height);
				batch.Draw(f.Sheet, pos, src, color.HasValue ? color.Value : Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
				pos.X += f.Widths[c - 32] + spacing;
			}
		}

		public static int Measure(int font, string text, int spacing = 0)
		{
			if (fonts == null)
				LoadFonts();
			if (font >= fonts.Length)
				font = 0;
			var f = fonts[font];
			var maxWidth = 0;
			var thisWidth = 0;
			text = text.Replace("\r\n", "\n");
			for (var i = 0; i < text.Length; i++)
			{
				var c = text[i];
				if (c == '\n')
				{
					thisWidth = 0;
					continue;
				}
				if (c == '|' && i < text.Length - 3 && text[i + 3] == '|')
				{
					i += 3;
					continue;
				}
				thisWidth += f.Widths[c - 32] + spacing;
				if (thisWidth > maxWidth)
					maxWidth = thisWidth;
			}
			return maxWidth;
		}

		public static void DrawEx(SpriteBatch batch, int font, string text, int left, int top, Alignment alignment = Alignment.Left, int maxWidth = 0, Color? color = null, int spacing = 0)
		{
			if (maxWidth > 0)
			{
				//TODO: word-wrap to the best of our abilities
			}
			var width = Measure(font, text, spacing);
			if (alignment == Alignment.Right)
				left -= width;
			else if (alignment == Alignment.Center)
				left -= width / 2;
			Draw(batch, font, text, left, top, color, spacing);
		}
	}
}
