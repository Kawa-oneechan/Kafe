using System;
using System.Collections.Generic;
using Kawa.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kafe
{
	public enum MapKey
	{
		Up, Down, Left, Right, A, B, C, D, E, F
	}

	public class Input : GameComponent
	{
		private static KeyboardState keyState;
		private static bool[] keys, oldKeys;
		public static bool Anything { get; private set; }

		public static ControlSet[] Controls;

		public Input(Game game) : base(game)
		{
			keys = new bool[256];
			oldKeys = new bool[256];
			keyState = Keyboard.GetState();

			Controls = new ControlSet[2]; 

			if (!System.IO.File.Exists("keys.json"))
				System.IO.File.WriteAllText("keys.json", Mix.GetString("keys.json", false));
			var mapData = Json5.Parse(System.IO.File.ReadAllText("keys.json")) as List<object>;
			for (var i = 0; i < Controls.Length; i++)
			{
				Controls[i] = new ControlSet((JsonObj)mapData[i]);
				Controls[i].Index = i;
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			Array.Copy(keys, oldKeys, 256);

			keyState = Keyboard.GetState();

			Anything = false;
			for (var i = 0; i < 256; i++)
			{
				if (keys[i] = (keyState[(Keys)i] == KeyState.Down))
					Anything = true;
			}

			foreach (var control in Controls)
				control.Update();
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

		public static void Flush()
		{
			for (var i = 0; i < 256; i++)
			{
				keys[i] = oldKeys[i] = false;
			}
		}

		public static Keys LastKeyPress()
		{
			if (!Anything)
				return Keys.None;
			for (var i = 0; i < 256; i++)
				if (keys[i])
					return (Keys)i;
			return Keys.None;
		}

		public static Buttons LastGamepadPress(int playerIndex = 0)
		{
			var state = GamePad.GetState((PlayerIndex)playerIndex);
			if (!state.IsConnected)
				return (Buttons)0;
			var enumVals = Enum.GetValues(typeof(Buttons));
			for (var i = 0; i < enumVals.Length; i++)
				if (state.IsButtonDown((Buttons)enumVals.GetValue(i)))
					return (Buttons)enumVals.GetValue(i);
			return (Buttons)0;
		}

		public static void Save()
		{
			var list = new List<JsonObj>();
			foreach (var control in Controls)
			{
				var data = new JsonObj();
				var keyboard = new JsonObj();
				var gamepad = new JsonObj();
				foreach (var input in control.KeyMap)
					keyboard[input.Key.ToString()] = input.Value.ToString();
				foreach (var input in control.PadMap)
					gamepad[input.Key.ToString()] = input.Value.ToString();
				data["keyboard"] = keyboard;
				data["gamepad"] = gamepad;
			}
			System.IO.File.WriteAllText("keys.json", list.Stringify());
		}
	}

	public class ControlSet
	{
		public Dictionary<MapKey, Keys> KeyMap;
		public Dictionary<MapKey, Buttons> PadMap;
		public int Index
		{
			get { return index; }
			set { index = value % 2; }
		}
		public PlayerIndex PlayerIndex
		{
			get { return (PlayerIndex)Index; }
			set { Index = (int)value; }
		}
		public bool GamepadAvailable
		{
			get { return GamePad.GetState(PlayerIndex).IsConnected; }
		}

		#region Continuous
		public bool Up { get { return Input.IsHeld(KeyMap[MapKey.Up]) || padState.IsButtonDown(PadMap[MapKey.Up]); } }
		public bool Down { get { return Input.IsHeld(KeyMap[MapKey.Down]) || padState.IsButtonDown(PadMap[MapKey.Down]); } }
		public bool Left { get { return Input.IsHeld(KeyMap[MapKey.Left]) || padState.IsButtonDown(PadMap[MapKey.Left]); } }
		public bool Right { get { return Input.IsHeld(KeyMap[MapKey.Right]) || padState.IsButtonDown(PadMap[MapKey.Right]); } }
		public bool A { get { return Input.IsHeld(KeyMap[MapKey.A]) || padState.IsButtonDown(PadMap[MapKey.A]); } }
		public bool B { get { return Input.IsHeld(KeyMap[MapKey.B]) || padState.IsButtonDown(PadMap[MapKey.B]); } }
		public bool C { get { return Input.IsHeld(KeyMap[MapKey.C]) || padState.IsButtonDown(PadMap[MapKey.C]); } }
		public bool D { get { return Input.IsHeld(KeyMap[MapKey.D]) || padState.IsButtonDown(PadMap[MapKey.D]); } }
		public bool E { get { return Input.IsHeld(KeyMap[MapKey.E]) || padState.IsButtonDown(PadMap[MapKey.E]); } }
		public bool F { get { return Input.IsHeld(KeyMap[MapKey.F]) || padState.IsButtonDown(PadMap[MapKey.F]); } }
		public bool Anything { get; private set; }
		#endregion

		#region Trigger
		public bool TrgUp { get { return Input.WasJustPressed(KeyMap[MapKey.Up]) || PadWasReleased(PadMap[MapKey.Up]); } }
		public bool TrgDown { get { return Input.WasJustPressed(KeyMap[MapKey.Down]) || PadWasReleased(PadMap[MapKey.Down]); } }
		public bool TrgLeft { get { return Input.WasJustPressed(KeyMap[MapKey.Left]) || PadWasReleased(PadMap[MapKey.Left]); } }
		public bool TrgRight { get { return Input.WasJustPressed(KeyMap[MapKey.Right]) || PadWasReleased(PadMap[MapKey.Right]); } }
		public bool TrgA { get { return Input.WasJustPressed(KeyMap[MapKey.A]) || PadWasReleased(PadMap[MapKey.A]); } }
		public bool TrgB { get { return Input.WasJustPressed(KeyMap[MapKey.B]) || PadWasReleased(PadMap[MapKey.B]); } }
		public bool TrgC { get { return Input.WasJustPressed(KeyMap[MapKey.C]) || PadWasReleased(PadMap[MapKey.C]); } }
		public bool TrgD { get { return Input.WasJustPressed(KeyMap[MapKey.D]) || PadWasReleased(PadMap[MapKey.D]); } }
		public bool TrgE { get { return Input.WasJustPressed(KeyMap[MapKey.E]) || PadWasReleased(PadMap[MapKey.E]); } }
		public bool TrgF { get { return Input.WasJustPressed(KeyMap[MapKey.F]) || PadWasReleased(PadMap[MapKey.F]); } }
		#endregion

		private int index;
		private GamePadState padState, oldState;

		public ControlSet(JsonObj mapData)
		{
			KeyMap = new Dictionary<MapKey, Keys>();
			PadMap = new Dictionary<MapKey, Buttons>();
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

		public void Update()
		{
			if (GamepadAvailable)
			{
				oldState = padState;
				padState = GamePad.GetState(PlayerIndex);
			}
			Anything = Up || Down || Left || Right || A || B || C || D || E || F;
		}

		private bool PadWasReleased(Buttons button)
		{
			return oldState.IsButtonDown(button) && padState.IsButtonUp(button);
		}
	}
}
