﻿using System;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;


namespace SkyShoot.Game.Screens
{
    class LoginScreen:ScreenManager.GameScreen
    {
        private GuiManager _gui;
        private LabelControl _loginLabel;
        private InputControl _loginBox;
        private LabelControl _passwordLabel;
        private InputControl _passwordBox;
        private ButtonControl _backButton;
        private ButtonControl _loginButton;
        private Screen _mainScreen;

        public override void LoadContent()
        {
            base.LoadContent();
            _gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;

            _mainScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f) 
            );

            // Login Input
            _loginBox = new InputControl
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
                Text = Settings.Default.login 
            };
            _mainScreen.Desktop.Children.Add(_loginBox);

            // Password Input
            _passwordBox = new InputControl
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
                Text = Settings.Default.password
            };
            _mainScreen.Desktop.Children.Add(_passwordBox);

            // Login Label
            _loginLabel = new LabelControl("Username")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, -70), 100, 30)
            };
            _mainScreen.Desktop.Children.Add(_loginLabel);
            
            // Password Label
            _passwordLabel = new LabelControl("Password")
            {
                Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, 0), 100, 30)
            };
            _mainScreen.Desktop.Children.Add(_passwordLabel);

            // Login Button
            _loginButton = new ButtonControl
            {
                Text = "Login",
                Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
            };
            _loginButton.Pressed += LoginButtonPressed;
            _mainScreen.Desktop.Children.Add(_loginButton);

            // Password Button
            _backButton = new ButtonControl
            {
                Text = "Back",
                Bounds = new UniRectangle(new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32)
            };
            _backButton.Pressed += BackButtonPressed;
            _mainScreen.Desktop.Children.Add(_backButton);

        }

        private void BackButtonPressed(object sender, EventArgs args)
        {
            ExitScreen();
        }

        private void LoginButtonPressed(object sender, EventArgs args)
        {

            if(_loginBox.Text.Length < 3) ScreenManager.AddScreen(new MessageBox("Username is too short!\nPress Enter to continue"));
            else if (_passwordBox.Text.Length < 3) ScreenManager.AddScreen(new MessageBox("Password is too short!\nPress Enter to continue"));
            else
            {
                Settings.Default.login = _loginBox.Text;
                Settings.Default.password = _passwordBox.Text;
                Settings.Default.Save();
                // todo login
               // foreach (GameScreen screen in ScreenManager.GetScreens()) screen.ExitScreen();
                ScreenManager.AddScreen(new MultiplayerScreen());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

    }
}
