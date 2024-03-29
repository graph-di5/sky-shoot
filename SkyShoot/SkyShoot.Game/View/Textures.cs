using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.View
{
	public static class Textures
	{
		public const int STONES_AMOUNT = 4;
		public const int MOBS_AMOUNT = 2;

		public const int PLAYER_ANIMATION_FRAME_COUNT = 2;
		public const int SPIDER_ANIMATION_FRAME_COUNT = 9;

		/// <summary>
		/// current graphic device
		/// </summary>
		public static GraphicsDevice GraphicsDevice;

		// Landscape textures
		public static Texture2D SandLandscape;
		public static Texture2D GrassLandscape;
		public static Texture2D SnowLandscape;
		public static Texture2D DesertLandscape;
		public static Texture2D VolcanicLandscape;

		// cursor textures
		public static Texture2D Arrow;
		public static Texture2D Plus;
		public static Texture2D Cross;
		public static Texture2D Target;

		// weapon textures
		public static Texture2D Gun;
		public static Texture2D Laser;
		public static Texture2D FlameProjectile;
		public static Texture2D ShotgunProjectile;
		public static Texture2D RocketProjectile;
		public static Texture2D SpiderProjectile;
		public static Texture2D PoisonProjectile;
		public static Texture2D TurretProjectile;
		public static Texture2D Explosion;

		//minions textures

		public static Texture2D Turret;

		// bonuses textures
		public static Texture2D DoubleDamage;
		public static Texture2D Fire;
		public static Texture2D Frozen;
		public static Texture2D MedChest;
		public static Texture2D Mirror;
		public static Texture2D Protection;
		public static Texture2D Speed;

		// stone textures
		public static Texture2D[] Stones = new Texture2D[STONES_AMOUNT];
		public static Texture2D OneStone;

		// brick textures
		public static Texture2D Brick;

		// player animation textures
		public static Animation2D PlayerAnimation = new Animation2D();

		// mob animation textures
		public static Animation2D SpiderAnimation = new Animation2D();

		// Dead player texture
		public static Texture2D DeadPlayerTexture;

		// Dead spider texture
		public static Texture2D DeadSpiderTexture;

		// mob textures
		public static Texture2D PlayerTexture;
		public static Texture2D[] MobTextures = new Texture2D[MOBS_AMOUNT];
		public static Texture2D Poisoner;
		//public static Texture2D Poisoning;
		public static Texture2D Hydra;
		public static Texture2D ParentMob;
		public static Texture2D Caterpillar;

		public static Texture2D HealthRect(int width, int heigth, Color c)
		{
			return Create(width, heigth < 1 ? 1 : heigth, c);
		}

		public static Texture2D HeaterProjectile
		{
			get { return Create(2, 8, Color.Black); }
		}

		public static Texture2D LaserProjectile
		{
			get { return Create(2, 5, Color.Red); }
		}

		/// <summary>
		/// create a colored rectangle
		/// </summary>
		public static Texture2D Create(int width, int height, Color color)
		{
			// create the rectangle texture without colors
			var texture = new Texture2D(GraphicsDevice, width, height);

			// create a color array for the pixels
			var colors = new Color[width * height];
			var newColor = new Color(color.ToVector3());
			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = newColor;
			}

			// set the color data for the texture
			texture.SetData(colors);

			return texture;
		}

		/// <summary>
		/// copy a texture
		/// </summary>
		public static Texture2D Clone(Texture2D texture)
		{
			// get pixels from texture
			var textureData = new Color[texture.Width * texture.Height];
			texture.GetData(textureData);

			// create clone texture
			var clone = new Texture2D(GraphicsDevice, texture.Width, texture.Height);
			clone.SetData(textureData);

			return clone;
		}

		/// <summary>
		/// add small texture into big texture at Vector2D position
		/// </summary>
		public static void Merge(Texture2D big, Texture2D small, Vector2 position)
		{
			// get pixels from big texture
			var bigData = new Color[big.Width * big.Height];
			big.GetData(bigData);

			// get pixels from small texture
			var smallData = new Color[small.Width * small.Height];
			small.GetData(smallData);

			// replace transparent pixels
			for (int i = 0; i < small.Height; i++)
				for (int j = 0; j < small.Width; j++)
					if (smallData[i * small.Width + j] == Color.Transparent)
						smallData[i * small.Width + j] = bigData[((int)position.Y + i) * big.Width + ((int)position.X + j)];

			// set the new data
			big.SetData(
				0,
				new Rectangle((int)position.X, (int)position.Y, small.Width, small.Height),
				smallData,
				0,
				small.Width * small.Height);
		}

		public static Texture2D ActiveCursor
		{
			get
			{
				switch (Settings.Default.Cursor)
				{
					case 1:
						return Arrow;
					case 2:
						return Plus;
					case 3:
						return Cross;
					case 4:
						return Target;
					default:
						return Arrow;
				}
			}
		}

		public static Vector2 GetCursorPosition(float x, float y)
		{
			switch (Settings.Default.Cursor)
			{
				case 1:
					return new Vector2(x - 11f, y - 2.5f);
				case 2:
					return new Vector2(x - 23f, y - 23f);
				case 3:
					return new Vector2(x - 23f, y - 23f);
				case 4:
					return new Vector2(x - 24f, y - 23f);
				default:
					return new Vector2(x - 11f, y - 2.5f);
			}
		}
	}
}

