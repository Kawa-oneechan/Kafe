using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Kawa.Json;

namespace Kafe
{
	public enum MapKey
	{
		Up, Down, Left, Right, A, B, C, D, E, F
	}

	public class Input : GameComponent
	{
		#region Continuous
		public static bool Up { get { return Input.keys[(int)KeyMap[MapKey.Up]]; } }
		public static bool Down { get { return Input.keys[(int)KeyMap[MapKey.Down]]; } }
		public static bool Left { get { return Input.keys[(int)KeyMap[MapKey.Left]]; } }
		public static bool Right { get { return Input.keys[(int)KeyMap[MapKey.Right]]; } }
		public static bool A { get { return Input.keys[(int)KeyMap[MapKey.A]]; } }
		public static bool B { get { return Input.keys[(int)KeyMap[MapKey.B]]; } }
		public static bool C { get { return Input.keys[(int)KeyMap[MapKey.C]]; } }
		public static bool D { get { return Input.keys[(int)KeyMap[MapKey.D]]; } }
		public static bool E { get { return Input.keys[(int)KeyMap[MapKey.E]]; } }
		public static bool F { get { return Input.keys[(int)KeyMap[MapKey.F]]; } }
		public static bool Anything { get; private set; }
		#endregion

		#region Trigger
		public static bool TrgUp { get { return Input.WasJustPressed(KeyMap[MapKey.Up]); } }
		public static bool TrgDown { get { return Input.WasJustPressed(KeyMap[MapKey.Down]); } }
		public static bool TrgLeft { get { return Input.WasJustPressed(KeyMap[MapKey.Left]); } }
		public static bool TrgRight { get { return Input.WasJustPressed(KeyMap[MapKey.Right]); } }
		public static bool TrgA { get { return Input.WasJustPressed(KeyMap[MapKey.A]); } }
		public static bool TrgB { get { return Input.WasJustPressed(KeyMap[MapKey.B]); } }
		public static bool TrgC { get { return Input.WasJustPressed(KeyMap[MapKey.C]); } }
		public static bool TrgD { get { return Input.WasJustPressed(KeyMap[MapKey.D]); } }
		public static bool TrgE { get { return Input.WasJustPressed(KeyMap[MapKey.E]); } }
		public static bool TrgF { get { return Input.WasJustPressed(KeyMap[MapKey.F]); } }
		#endregion

		private static GamePadState padState;
		private static KeyboardState keyState;
		private static bool[] keys, oldKeys;

		public static Dictionary<MapKey, Keys> KeyMap;
		public static Dictionary<MapKey, Buttons> PadMap;

		public static bool GamepadAvailable
		{
			get
			{
				return GamePad.GetState(PlayerIndex.One).IsConnected;
			}
		}

		public Input(Game game) : base(game)
		{
			keys = new bool[256];
			oldKeys = new bool[256];
			keyState = Keyboard.GetState();

			KeyMap = new Dictionary<MapKey, Keys>();
			PadMap = new Dictionary<MapKey, Buttons>();

			if (!System.IO.File.Exists("keys.json"))
				System.IO.File.WriteAllText("keys.json", Mix.GetString("keys.json", false));
			var mapData = Json5.Parse(System.IO.File.ReadAllText("keys.json")) as JsonObj;
			foreach (var k in mapData["keyboard"] as JsonObj)
			{
				var mk = (MapKey)Enum.Parse(typeof(MapKey), k.Key, true);
				var mv = (Keys)Enum.Parse(typeof(Keys), k.Value as string, true);
				KeyMap[mk] = mv;
			}
			foreach (var k in mapData["gamepad"] as JsonObj)
			{
				var mk = (MapKey)Enum.Parse(typeof(MapKey), k.Key, true);
				var mv = (Buttons)Enum.Parse(typeof(Buttons), k.Value as string, true);
				PadMap[mk] = mv;
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			//InputExtensions.Update();
			Array.Copy(keys, oldKeys, 256);

			keyState = Keyboard.GetState();

			Anything = false;

			if (keyState.IsKeyDown(Keys.Down))
				gameTime.ToString();

			for (var i = 0; i < 256; i++)
			{
				if (keys[i] = (keyState[(Keys)i] == KeyState.Down))
					Anything = true;
			}

			//Remap gamepad to keyboard
			//TODO: use a configurable LUT
			padState = GamePad.GetState(PlayerIndex.One);
			foreach (var p in PadMap)
			{
				keys[(int)KeyMap[p.Key]] = keyState.IsKeyDown(KeyMap[p.Key]) || padState.IsButtonDown(p.Value);
			}
		}

		public static bool WasJustPressed(Keys key)
		{
			return keys[(int)key] && !oldKeys[(int)key];
		}

		public static bool WasJustReleased(Keys key)
		{
			return !keys[(int)key] && oldKeys[(int)key];
		}

		public static bool IsHeld(Keys key)
		{
			return keys[(int)key];
		}

		public static void Save()
		{
			var data = new JsonObj();
			var keyboard = new JsonObj();
			var gamepad = new JsonObj();
			foreach (var input in KeyMap)
				keyboard[input.Key.ToString()] = input.Value.ToString();
			foreach (var input in PadMap)
				gamepad[input.Key.ToString()] = input.Value.ToString();
			data["keyboard"] = keyboard;
			data["gamepad"] = gamepad;
			System.IO.File.WriteAllText("keys.json", data.Stringify());
		}
	}
}
