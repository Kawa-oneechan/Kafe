﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{
    public static class VectorExtensions
    {
        public static Vector2 ToInteger(this Vector2 vector)
        {
            return new Vector2((int)vector.X, (int)vector.Y);
        }
	}

	public static class RectangleExtensions
	{
		public static string ToShort(this Rectangle rect)
		{
			return string.Format("<{0},{1},{2},{3}>", rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static bool Intersects(this Rectangle a, Rectangle b, Vector2 originA, Vector2 originB)
		{
			var aX = a.X + originA.X;
			var aY = a.Y + originA.Y;
			var bX = b.X + originB.X;
			var bY = b.Y + originB.Y;
			return (bX < aX + a.Width && aX < bX + b.Width && bY < aY + a.Height && aY < bY + b.Height);
		}
	}

    public static class InputExtensions
    {
        private static KeyboardState oldKeyState, newKeyState;
		private static GamePadDPad oldDPadState, newDPadState;
		private static GamePadButtons oldButtonState, newButtonState;

		static InputExtensions()
        {
            oldKeyState = newKeyState = Keyboard.GetState();
			var padState = GamePad.GetState(PlayerIndex.One);
			oldDPadState = newDPadState = padState.DPad;
			oldButtonState = newButtonState = padState.Buttons;
		}

        public static void Update()
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();

			var padState = GamePad.GetState(PlayerIndex.One);
			oldDPadState = newDPadState;
			newDPadState = padState.DPad;
			oldButtonState = newButtonState;
			newButtonState = padState.Buttons;
		}

        public static bool IsDown(this Keys key)
        {
            return newKeyState.IsKeyDown(key);
        }

        public static bool IsUp(this Keys key)
        {
            return newKeyState.IsKeyUp(key);
        }

        public static bool WasJustPressed(this Keys key)
        {
            return newKeyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
        }

        public static bool WasJustReleased(this Keys key)
        {
            return newKeyState.IsKeyUp(key) && oldKeyState.IsKeyDown(key);
        }

		public static bool WasJustPressed(this Buttons key)
		{
			switch (key)
			{
				case Buttons.DPadUp: return newDPadState.Up == ButtonState.Pressed && oldDPadState.Up == ButtonState.Released;
				case Buttons.DPadDown: return newDPadState.Down == ButtonState.Pressed && oldDPadState.Down == ButtonState.Released;
				case Buttons.DPadLeft: return newDPadState.Left == ButtonState.Pressed && oldDPadState.Left == ButtonState.Released;
				case Buttons.DPadRight: return newDPadState.Right == ButtonState.Pressed && oldDPadState.Right == ButtonState.Released;
				case Buttons.A: return newButtonState.A == ButtonState.Pressed && oldButtonState.A == ButtonState.Released;
				case Buttons.B: return newButtonState.B == ButtonState.Pressed && oldButtonState.B == ButtonState.Released;
				case Buttons.X: return newButtonState.X == ButtonState.Pressed && oldButtonState.X == ButtonState.Released;
				case Buttons.Y: return newButtonState.Y == ButtonState.Pressed && oldButtonState.Y == ButtonState.Released;
			}
			return false;
		}
	}
}
