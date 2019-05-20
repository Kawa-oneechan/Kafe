using System;
using System.Collections.Generic;
using Kawa.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
	private JsonObj background;
	private Texture2D backgroundTexture;
	private BackgroundTypes backgroundType;
	private int backgroundFloor;
 */

namespace Kafe
{
	class Background : DrawableGameComponent
	{
		private JsonObj json;
		public Texture2D Sheet { get; private set; }
		public List<BackgroundLayer> Layers { get; private set; }
		public int LeftExtent, RightExtent;

		public Background(string file) : base(Kafe.Me)
		{
			json = Mix.GetJson(file) as JsonObj;
			Sheet = Mix.GetTexture("locales\\" + (json["base"] as string));
			Layers = new List<BackgroundLayer>();
			Kafe.Ground = json.ContainsKey("ground") ? (int)(double)json["ground"] : 240;
			Kafe.LeftStart = json.ContainsKey("start") ? (int)(double)((List<object>)json["start"])[0] : 300;
			Kafe.RightStart = json.ContainsKey("start") ? (int)(double)((List<object>)json["start"])[1] : 470;
			LeftExtent = 0;
			RightExtent = 512;
			if (json.ContainsKey("extent"))
			{
				var data = ((List<object>)json["extent"]);
				LeftExtent = (int)(double)data[0];
				RightExtent = (int)(double)data[1];
			}
			foreach (var layer in ((List<object>)json["layers"]))
				Layers.Add(new BackgroundLayer((JsonObj)layer, this));
		}

		public override void Update(GameTime gameTime)
		{
			if (Kafe.Paused)
				return;
			base.Update(gameTime);

			if (Kafe.Camera.X < LeftExtent) Kafe.Camera.X = LeftExtent;
			if (Kafe.Camera.X > RightExtent) Kafe.Camera.X = RightExtent;

			foreach (var layer in Layers)
				layer.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (var layer in Layers)
				layer.Draw(gameTime);
		}

		public virtual void DrawPlayers(GameTime gameTime)
		{
			throw new NotImplementedException("Don't call DrawPlayers on a raw Background!");
		}
	}

	class BackgroundLayer
	{
		public Vector2 Origin { get; private set; }
		public Rectangle Rect { get; private set; }
		public Vector2 Parallax { get; private set; }
		public Vector2 Movement { get; private set; }
		public int[] Frames { get; private set; }
		public int FrameRate { get; private set; }
		public bool Floor { get; private set; }
		public int[] FloorVals { get; private set; }
		public bool IsPlayerLayer { get; private set; }
		public Background Parent { get; private set; }
		public string BlendMode { get; private set; }

		private int frameTimeLeft, currentFrame;
		private Vector2 movementOrigin;

		public BackgroundLayer(JsonObj json, Background parent)
		{
			Parent = parent;

			if (json.ContainsKey("fighters"))
			{
				IsPlayerLayer = true;
				return;
			}

			if (!json.ContainsKey("rect"))
				throw new MissingFieldException("Background layer must have a rect.");

			var data = ((List<object>)json["rect"]);
			Rect = new Rectangle((int)(double)data[0], (int)(double)data[1], (int)(double)data[2], (int)(double)data[3]);

			Origin = Vector2.Zero;
			if (json.ContainsKey("origin"))
			{
				data = ((List<object>)json["origin"]);
				Origin = new Vector2((int)(double)data[0], (int)(double)data[1]);
			}

			Parallax = Vector2.One;
			if (json.ContainsKey("parallax"))
			{
				data = ((List<object>)json["parallax"]);
				Parallax = new Vector2((float)(double)data[0], (float)(double)data[1]);
			}

			Movement = Vector2.Zero;
			if (json.ContainsKey("movement"))
			{
				data = ((List<object>)json["movement"]);
				Movement = new Vector2((float)(double)data[0], (float)(double)data[1]);
			}

			if (json.ContainsKey("floor"))
			{
				Floor = (bool)json["floor"];
				if (json.ContainsKey("floorVals"))
				{
					data = ((List<object>)json["floorVals"]);
					FloorVals = new[] { (int)(double)data[0], (int)(double)data[1] };
				}
				else
					FloorVals = new[] { 128, 128 };
			}

			Frames = new int[1] { 0 };
			if (json.ContainsKey("frames"))
			{
				if (json["frames"] is List<object>)
				{
					var framesList = (List<object>)json["frames"];
					Frames = new int[framesList.Count];
					for (var j = 0; j < framesList.Count; j++)
						Frames[j] = (int)(double)framesList[j];
				}
				else
				{
					var i = (int)(double)json["frames"];
					Frames = new int[i];
					for (var j = 0; j < i; j++)
						Frames[j] = j;
				}
			}
			FrameRate = 100;
			if (json.ContainsKey("rate"))
				FrameRate = (int)(double)json["rate"];
			frameTimeLeft = FrameRate;

			BlendMode = "none";
			if (json.ContainsKey("blend"))
				BlendMode = json["blend"] as string;
		}

		public void Update(GameTime gameTime)
		{
			if (IsPlayerLayer)
				return;

			if (Frames.Length > 0)
			{
				frameTimeLeft -= gameTime.ElapsedGameTime.Milliseconds;
				if (frameTimeLeft <= 0)
				{
					frameTimeLeft = FrameRate;
					currentFrame++;
					if (currentFrame >= Frames.Length)
						currentFrame = 0;
				}
			}

			if (Movement.Length() != 0)
			{
				movementOrigin += Movement;
				//TODO: can we do better?
				while (movementOrigin.X < -Rect.Width)
					movementOrigin.X += Rect.Width;
				while (movementOrigin.Y < -Rect.Height)
					movementOrigin.Y -= Rect.Height;
				while (movementOrigin.X > Rect.Width)
					movementOrigin.X -= Rect.Width;
				while (movementOrigin.Y > Rect.Height)
					movementOrigin.Y -= Rect.Height;
			}
		}

		public void Draw(GameTime gameTime)
		{
			if (IsPlayerLayer)
			{
				Parent.DrawPlayers(gameTime);
				return;
			}

			var blendState = new BlendState()
			{
				ColorBlendFunction = BlendFunction.Add,
				ColorSourceBlend = Blend.SourceAlpha,
				ColorDestinationBlend = Blend.InverseSourceAlpha
			};
			switch (BlendMode)
			{
				case "add":
					blendState.ColorSourceBlend = Blend.One;
					blendState.ColorDestinationBlend = Blend.One;
					break;
				case "sub":
					//TODO: doesn't match what I expected from mockups.
					//"Add" matches "Screen" in Paintshop Pro, but "Sub"
					//does not match "Multiply".
					blendState.ColorBlendFunction = BlendFunction.ReverseSubtract;
					blendState.ColorSourceBlend = Blend.One;
					blendState.ColorDestinationBlend = Blend.One;
					break;
			}
			Kafe.SpriteBatch.Begin(SpriteSortMode.Immediate, blendState);
			var targetPos = (-Kafe.Camera + Origin) * Parallax;

			if (Floor)
			{
				var src = new Rectangle(Rect.Left, Rect.Top, Rect.Width, 1);
				var displacement = (Kafe.Camera.X - Parent.LeftExtent - FloorVals[0]) / FloorVals[1];
				for (var row = 0; row < Rect.Height; row++)
				{
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos, src, Color.White);
					src.Offset(0, 1);
					targetPos.X -= displacement;
					targetPos.Y += 1;
				}
			}
			else
			{
				var src = Rect;

				if (Movement.Length() != 0)
				{
					targetPos += movementOrigin;
					//TODO: can we do better?
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos + new Vector2(-Rect.Width, -Rect.Height), src, Color.White);
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos + new Vector2(-Rect.Width, 0), src, Color.White);
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos + new Vector2(-Rect.Width, Rect.Height), src, Color.White);
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos + new Vector2(0, Rect.Height), src, Color.White);
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos + new Vector2(Rect.Width, Rect.Height), src, Color.White);
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos + new Vector2(Rect.Width, 0), src, Color.White);
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos + new Vector2(Rect.Width, -Rect.Height), src, Color.White);
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos + new Vector2(0, -Rect.Height), src, Color.White);
				}
	
				if (Frames.Length > 0)
					src.Offset(Rect.Width * Frames[currentFrame], 0);
				Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos, src, Color.White);
			}
			Kafe.SpriteBatch.End();
		}
	}
}
