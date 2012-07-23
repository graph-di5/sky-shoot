using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Utils;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Game.Input;
using SkyShoot.Game.Network;
using SkyShoot.Game.Screens;
using SkyShoot.Game.Utils;
using SkyShoot.Game.View;

namespace SkyShoot.Game.Game
{
	public sealed class GameController
	{
		#region singleton

		private static GameController _localInstance;

		public static GameController Instance
		{
			get { return _localInstance ?? (_localInstance = new GameController()); }
		}

		private GameController()
		{
			_weaponKeys = new Dictionary<Keys, WeaponType>
			              {
			              	{Keys.D1, WeaponType.Pistol},
			              	{Keys.D2, WeaponType.Shotgun},
			              	{Keys.D3, WeaponType.FlamePistol},
			              	{Keys.D4, WeaponType.RocketPistol},
			              	{Keys.D5, WeaponType.Heater},
			              	{Keys.D6, WeaponType.TurretMaker}
			              };
		}

		#endregion

		private GameModel _gameModel;

		public static Guid MyId { get; private set; }

		// todo temporary
		public static long StartTime { get; set; }

		public WeaponType CurrentWeapon { get; private set; }

		public bool IsGameStarted { get; private set; }

		// todo move to another place
		/// <summary>
		/// key -> weapon
		/// </summary>
		private IDictionary<Keys, WeaponType> _weaponKeys;

		private void Shoot(Vector2 direction)
		{
			ConnectionManager.Instance.Shoot(TypeConverter.Xna2XnaLite(direction));
		}

		private void Move(Vector2 direction)
		{
			ConnectionManager.Instance.Move(TypeConverter.Xna2XnaLite(direction));
		}

		public void GameStart(Contracts.Session.GameLevel arena, int gameId)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.GameplayScreen);

			var timeHelper = new TimeHelper(StartTime);

			var logger = new Logger(Logger.SolutionPath + "\\logs\\client_game_" + gameId + ".txt", timeHelper);

			_gameModel = new GameModel(GameFactory.CreateClientGameLevel(arena), timeHelper);

			_gameModel.Update(new GameTime());

			Trace.Listeners.Add(logger);
			Trace.WriteLine("Game initialized (model created, first synchroframe applied)");
			Trace.Listeners.Remove(Logger.ClientLogger);

			// GameModel initialized, set boolean flag  
			IsGameStarted = true;
		}

		public void GameOver()
		{
			ConnectionManager.Instance.Stop();
			_gameModel = null;
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.MainMenuScreen);
			IsGameStarted = false;
		}

		public void DrawWorld(SpriteBatch spriteBatch)
		{
			_gameModel.Draw(spriteBatch);
		}

		public void UpdateWorld(GameTime gameTime)
		{
			_gameModel.Update(gameTime);
		}

		#region ��������� �����

		private DateTime _dateTime;

		public void HandleInput(Controller controller)
		{
			DrawableGameObject player = _gameModel.GetGameObject(MyId);

			// current RunVector
			Vector2? currentRunVector = controller.RunVector;

			if (currentRunVector.HasValue)
			{
				Move(currentRunVector.Value);
				player.RunVectorM = currentRunVector.Value;
			}

			// ������������ ������ �� ����� � �������� ����(todo)
			if (controller is KeyboardAndMouse)
			{
				var keyboardAndMouse = controller as KeyboardAndMouse;

				bool weaponSwitched = false;

				foreach (var weapon in _weaponKeys)
				{
					if (keyboardAndMouse.IsNewKeyPressed(weapon.Key))
					{
						CurrentWeapon = weapon.Value;
						weaponSwitched = true;
					}
				}

				if (weaponSwitched)
				{
					ConnectionManager.Instance.ChangeWeapon(CurrentWeapon);
				}
				if (keyboardAndMouse.IsNewKeyPressed(Keys.D7)) ConnectionManager.Instance.ChangeWeapon(WeaponType.MobGenerator);
			}

			Vector2 mouseCoordinates = controller.SightPosition;

			player.ShootVectorM = mouseCoordinates - _gameModel.Camera2D.ConvertToLocal(player.CoordinatesM);
			if (player.ShootVector.Length() > 0)
				player.ShootVector.Normalize();

			if (controller.ShootButton == ButtonState.Pressed)
			{
				if ((DateTime.Now - _dateTime).Milliseconds > Constants.SHOOT_RATE)
				{
					_dateTime = DateTime.Now;
					Shoot(player.ShootVectorM);
				}
			}
		}

		#endregion

		public Guid? Login(string username, string password, out AccountManagerErrorCode errorCode)
		{
			// TODO check for null
			Guid? id = ConnectionManager.Instance.Login(username, password, out errorCode);
			if (id.HasValue)
			{
				MyId = id.Value;
			}
			return MyId;
		}

		public AccountManagerErrorCode Logout()
		{
			return ConnectionManager.Instance.Logout();
		}
	}
}