﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Game.Game;
using SkyShoot.Game.Input;
using SkyShoot.Game.Network;
using SkyShoot.Game.View;

namespace SkyShoot.Game.Screens
{
	internal class GameplayScreen : GameScreen
	{
		// оружие -> описание оружия
		private readonly IDictionary<WeaponType, string> _weapons;

		private LabelControl _levelLabel;
		private LabelControl _expLabel;
		private LabelControl _fragLabel;
		private LabelControl _creepsLabel;

		private int _counter;

		public GameplayScreen()
		{
			_weapons = new Dictionary<WeaponType, string>
			           {
			           	{WeaponType.Pistol, "Pistol"},
			           	{WeaponType.Shotgun, "Shotgun"},
			           	{WeaponType.FlamePistol, "Flame"},
			           	{WeaponType.RocketPistol, "Rocket"},
			           	{WeaponType.Heater, "Heater"},
			           	{WeaponType.TurretMaker, "Turret"}
			           };

			CreateControls();
			InitializeControls();
		}

		public override void LoadContent()
		{
			// load landscapes
			Textures.SandLandscape = ContentManager.Load<Texture2D>("Textures/Landscapes/SandLandscape");
			Textures.GrassLandscape = ContentManager.Load<Texture2D>("Textures/Landscapes/GrassLandscape");
			Textures.SnowLandscape = ContentManager.Load<Texture2D>("Textures/Landscapes/SnowLandscape");
			Textures.DesertLandscape = ContentManager.Load<Texture2D>("Textures/Landscapes/DesertLandscape");
			Textures.VolcanicLandscape = ContentManager.Load<Texture2D>("Textures/Landscapes/VolcanicLandscape");

			// load weapon
			Textures.Gun = ContentManager.Load<Texture2D>("Textures/Weapon/Gun");
			Textures.Laser = ContentManager.Load<Texture2D>("Textures/Weapon/Laser");
			Textures.FlameProjectile = ContentManager.Load<Texture2D>("Textures/Weapon/FlameProjectile");
			Textures.ShotgunProjectile = ContentManager.Load<Texture2D>("Textures/Weapon/ShotProjectile");
			Textures.RocketProjectile = ContentManager.Load<Texture2D>("Textures/Weapon/RocketProjectile");
			Textures.TurretProjectile = ContentManager.Load<Texture2D>("Textures/Weapon/TurretProjectile");
			Textures.PoisonProjectile = ContentManager.Load<Texture2D>("Textures/Weapon/PoisonProjectile");
			Textures.SpiderProjectile = ContentManager.Load<Texture2D>("Textures/Weapon/SpiderProjectile");
			Textures.Explosion = ContentManager.Load<Texture2D>("Textures/Weapon/Explosion");
			Textures.DoubleDamage = ContentManager.Load<Texture2D>("Textures/BonusesIcons/DoubleDamage");
			Textures.Fire = ContentManager.Load<Texture2D>("Textures/BonusesIcons/Fire");
			Textures.Frozen = ContentManager.Load<Texture2D>("Textures/BonusesIcons/Frozen");
			Textures.MedChest = ContentManager.Load<Texture2D>("Textures/BonusesIcons/MedChest");
			Textures.Mirror = ContentManager.Load<Texture2D>("Textures/BonusesIcons/Mirror");
			Textures.Protection = ContentManager.Load<Texture2D>("Textures/BonusesIcons/Protection");
			Textures.Speed = ContentManager.Load<Texture2D>("Textures/BonusesIcons/Speed");
			Textures.Poisoner = ContentManager.Load<Texture2D>("Textures/Mobs/Spider1");
			//Textures.Poisoning = ContentManager.Load<Texture2D>("Textures/Mobs/Poisoning");
			Textures.Hydra = ContentManager.Load<Texture2D>("Textures/Mobs/Spider2");
			Textures.ParentMob = ContentManager.Load<Texture2D>("Textures/Mobs/Spider3");
			Textures.Caterpillar = ContentManager.Load<Texture2D>("Textures/Mobs/CaterpillerSegmentHead");
			//load minions

			Textures.Turret = ContentManager.Load<Texture2D>("Textures/Mobs/Turret");

			// load stones
			for (int i = 1; i <= Textures.STONES_AMOUNT; i++)
				Textures.Stones[i - 1] = ContentManager.Load<Texture2D>("Textures/Landscapes/Stone" + i);
			Textures.OneStone = ContentManager.Load<Texture2D>("Textures/Landscapes/Stone" + 1);

			// load bricks
			Textures.Brick = ContentManager.Load<Texture2D>("Textures/Landscapes/Brick");

			// load player
			Textures.PlayerTexture = ContentManager.Load<Texture2D>("Textures/Mobs/Man");

			// load mobs
			for (int i = 1; i <= Textures.MOBS_AMOUNT; i++)
				Textures.MobTextures[i - 1] = ContentManager.Load<Texture2D>("Textures/Mobs/Spider" + i);

			// load dead player
			Textures.DeadPlayerTexture =
				ContentManager.Load<Texture2D>("Textures/Mobs/man_animation(new man)/death_animation/death_animation_06");

			// load dead spider
			Textures.DeadSpiderTexture =
				ContentManager.Load<Texture2D>(
					"Textures/Mobs/mob_animation(v.2)/paukan_death_animation/paukan_death_animation_03");

			// load mob animation
			for (int i = 1; i <= Textures.SPIDER_ANIMATION_FRAME_COUNT; i++)
				Textures.SpiderAnimation.AddFrame(
					ContentManager.Load<Texture2D>("Textures/Mobs/spider_animation(uncomplete)/spider_" + i.ToString("D2")));

			// load player animation
			for (int i = 1; i <= Textures.PLAYER_ANIMATION_FRAME_COUNT; i++)
				Textures.PlayerAnimation.AddFrame(
					ContentManager.Load<Texture2D>("Textures/Mobs/man_animation(new man)/run/run_" + i.ToString("D2")));

			ScreenManager.Instance.Game.ResetElapsedTime();
		}

		public override void HandleInput(Controller controller)
		{
			GameController.Instance.HandleInput(controller);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			Debug.Assert(GameController.Instance.IsGameStarted);

			GameController.Instance.UpdateWorld(gameTime);

			if (_counter % 60 == 0)
			{
				var stat = ConnectionManager.Instance.GetStats();
				if (stat != null)
				{
					_levelLabel.Text = "Level " + stat.Value.Level.ToString(CultureInfo.InvariantCulture);
					_expLabel.Text = "Exp " + stat.Value.Experience.ToString(CultureInfo.InvariantCulture);
					_fragLabel.Text = "Frag " + stat.Value.Frag.ToString(CultureInfo.InvariantCulture);
					_creepsLabel.Text = "Creeps " + stat.Value.Creeps.ToString(CultureInfo.InvariantCulture);
				}
			}
			_counter++;
		}

		public override void Draw(GameTime gameTime)
		{
			Debug.Assert(GameController.Instance.IsGameStarted);

			GraphicsDevice graphicsDevice = ScreenManager.Instance.GraphicsDevice;
			graphicsDevice.Clear(Color.SkyBlue);

			GameController.Instance.DrawWorld(SpriteBatch);

			#region список оружия

			WeaponType currentWeapon = GameController.Instance.CurrentWeapon;

			const int positionX = 720;
			int positionY = 0;

			SpriteFont = ContentManager.Load<SpriteFont>("Times New Roman");

			SpriteBatch.Begin();

			int currentIndex = 1;
			foreach (var weapon in _weapons)
			{
				DrawString(currentIndex++ + "-" + weapon.Value, positionX, positionY,
						   weapon.Key == currentWeapon ? Color.Red : Color.Black);
				positionY += 20;
			}

			SpriteBatch.End();

			#endregion
		}

		public override void Destroy()
		{
			base.Destroy();
			GameController.Instance.Destroy();
		}

		private void CreateControls()
		{
			#region статистика

			_levelLabel = new LabelControl
							{
								Text = "Level",
								Bounds = new UniRectangle(new UniVector(-60, -40), new UniVector(0, 0)),
							};

			_expLabel = new LabelControl
							{
								Text = "Exp",
								Bounds = new UniRectangle(new UniVector(-60, -20), new UniVector(0, 0)),
							};

			_fragLabel = new LabelControl
							{
								Text = "Frag",
								Bounds = new UniRectangle(new UniVector(-60, 0), new UniVector(0, 0)),
							};

			_creepsLabel = new LabelControl
							{
								Text = "Creeps",
								Bounds = new UniRectangle(new UniVector(-60, 20), new UniVector(0, 0)),
							};

			#endregion
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_levelLabel);
			Desktop.Children.Add(_expLabel);
			Desktop.Children.Add(_fragLabel);
			Desktop.Children.Add(_creepsLabel);
		}
	}
}
