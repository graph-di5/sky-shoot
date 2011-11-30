﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SkyShoot.Game.Screens
{
    class MessageBox:ScreenManager.GameScreen
    {
        //private string message;
		private static string text1 = "Username is too short!\nPress Enter to continue";
		private static string text2 = "Password is too short!\nPress Enter to continue";
		private static string text3 = "Registration failed";
        private Texture2D texture;
		private ContentManager _content;
        public MessageBox()
        {
            //message = text;
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
            IsPopup = true;
        }
        public override void LoadContent()
        {
            base.LoadContent();
			if (_content == null)
				_content = new ContentManager(ScreenManager.Game.Services, "Content");
			texture = _content.Load<Texture2D>("Textures/screens/screen_02");
            Color[] colors = new Color[1];
            colors[0] = Color.LightSeaGreen;
            //texture.SetData<Color>(colors);
        }
        public override void HandleInput(ScreenManager.InputState input)
        {
            if (input.IsMenuSelect()||input.IsMenuCancel()) ExitScreen(); 
        }

		public static String message
		{
			get
			{
				switch (Settings.Default.Message)
				{
					case 0: return text1;
					case 1: return text2;
					case 3: return text3;
					default: return text1;
				}
			}
		}

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;


            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(texture,
                 new Rectangle(0, 0, viewport.Width, viewport.Height),
                 Color.Black * TransitionAlpha * 0.66f);

            spriteBatch.Draw(texture, backgroundRectangle, color);

            spriteBatch.DrawString(font, message, textPosition, color);

            spriteBatch.End();
        }
    }
}
