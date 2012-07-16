﻿using System.Collections.Generic;
using System.Linq;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.ServProgram.Mobs
{
    class ShootingMob: Mob
	{
		protected readonly int _shootingDelay = 10000;
        protected long _lastShoot;

        public ShootingMob(float health, AWeapon weapon, int shootingDelay)
            : base(health)
		{
			Weapon = weapon;
			Weapon.Owner = this;
			_shootingDelay = shootingDelay;
			Radius = 10;
			Speed = 0.03f;
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new List<AGameEvent>(base.Think(gameObjects, newGameObjects, time));
			ShootVector = RunVector;
			ShootVector.Normalize();
			if (time - _lastShoot > _shootingDelay && Weapon != null && Weapon.IsReload(time))
			{
				_lastShoot = time;
			    var bullets = Weapon.CreateBullets(ShootVector);
			    res.AddRange(bullets.Select(bullet => new NewObjectEvent(bullet, time)));
			    newGameObjects.AddRange(bullets);
			}
			return res;
		}
	}
}
