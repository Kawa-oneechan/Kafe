using System;
using System.Collections.Generic;
using Kawa.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
			Sheet = Mix.GetTexture("locales\\" + json.Path<string>("/base"));
			Layers = new List<BackgroundLayer>();
			Kafe.Ground = json.ContainsKey("ground") ? json.Path<int>("/ground") : 240;
			Kafe.LeftStart = Kafe.RightStart = 100;
			if (json.ContainsKey("start"))
			{
				Kafe.LeftStart = json.Path<int>("/start/0");
				Kafe.RightStart = json.Path<int>("/start/1");
			}
			LeftExtent = 0;
			RightExtent = 512;
			if (json.ContainsKey("extent"))
			{
				LeftExtent = json.Path<int>("/extent/0");
				RightExtent = json.Path<int>("/extent/1");
			}
			foreach (var layer in ((List<object>)json["layers"]))
				Layers.Add(new BackgroundLayer((JsonObj)layer, this));
		}

		public override void Update(GameTime gameTime)
		{
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

			Rect = RectangleExtensions.FromJson(json["rect"]);

			Origin = json.ContainsKey("origin") ? VectorExtensions.FromJson(json["origin"]) : Vector2.Zero;

			Parallax = json.ContainsKey("parallax") ? VectorExtensions.FromJson(json["parallax"]) : Vector2.One;

			Movement = json.ContainsKey("movement") ? VectorExtensions.FromJson(json["movement"]) : Vector2.Zero;

			if (json.ContainsKey("floor"))
			{
				Floor = (bool)json["floor"];
				FloorVals = json.ContainsKey("floorVals") ? json.Path<int[]>("/floorVals") : new[] { 128, 128 };
			}

			Frames = new int[1] { 0 };
			if (json.ContainsKey("frames"))
			{
				if (json["frames"] is List<object>)
					Frames = json.Path<int[]>("/frames");
				else
				{
					var i = json.Path<int>("/frames");
					Frames = new int[i];
					for (var j = 0; j < i; j++)
						Frames[j] = j;
				}
			}
			FrameRate = json.ContainsKey("rate") ? json.Path<int>("/rate") : 100;
			frameTimeLeft = FrameRate;

			BlendMode = json.ContainsKey("blend") ? json.Path<string>("/blend") : "none";
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
			var targetPos = (-Kafe.Camera + Origin) * Parallax;

			var batch = Kafe.SpriteBatch;

			if (Floor)
			{
				batch.Begin(SpriteSortMode.Immediate, blendState);
				var src = new Rectangle(Rect.Left, Rect.Top, Rect.Width, 1);
				var displacement = (Kafe.Camera.X - Parent.LeftExtent - FloorVals[0]) / FloorVals[1];
				for (var row = 0; row < Rect.Height; row++)
				{
					Kafe.SpriteBatch.Draw(Parent.Sheet, targetPos, src, Color.White);
					src.Offset(0, 1);
					targetPos.X -= displacement;
					targetPos.Y += 1;
				}
				batch.End();
			}
			else
			{
				var src = Rect;

				if (Movement.Length() != 0)
				{
					targetPos += movementOrigin;
					batch.Begin(SpriteSortMode.Immediate, blendState, SamplerState.LinearWrap);
					batch.Draw(Parent.Sheet, Vector2.Zero, new Rectangle((int)targetPos.X, (int)targetPos.Y, src.Width * 3, src.Height * 3), Color.White);
					batch.End();
				}
				else
				{
					if (Frames.Length > 0)
						src.Offset(Rect.Width * Frames[currentFrame], 0);
					batch.Begin(SpriteSortMode.Immediate, blendState);
					batch.Draw(Parent.Sheet, targetPos, src, Color.White);
					batch.End();
				}
			}
		}
	}
}
