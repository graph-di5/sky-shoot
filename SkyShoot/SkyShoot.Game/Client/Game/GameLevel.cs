using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Contracts.Session;

using SkyShoot.Game.Client.View;

using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Game
{
    public class GameLevel : IDrawable
    {

        public const int StonesNumber = 50;

        private static Texture2D _texture;

        public static int Width
        {
            get { return _texture.Width; }
        }

        public static int Height
        {
            get { return _texture.Height; }
        }

        public GameLevel(TileSet tileSet)
        {
            //todo using const arrays
            switch (tileSet)
            {
                case TileSet.Grass:
                    _texture = Textures.GrassLandscape; break;
                case TileSet.Desert:
                    _texture = Textures.DesertLandscape; break;
                case TileSet.Sand:
                    _texture = Textures.SandLandscape; break;
                case TileSet.Snow:
                    _texture = Textures.SnowLandscape; break;
                case TileSet.Volcanic:
                    _texture = Textures.VolcanicLandscape; break;
            }

            var random = new Random();

            for (int i = 0; i < StonesNumber; i++) {
                int stone = random.Next(3);
                var randomPosition = new Vector2(random.Next(Width - Textures.Stones[stone].Width),
                    random.Next(Height - Textures.Stones[stone].Height));
                Textures.Merge(_texture, Textures.Stones[stone], randomPosition);
            }

        }

        public void AddTexture(Texture2D texture, Vector2 position)
        {
            Textures.Merge(_texture, texture, position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
        }

    }
}