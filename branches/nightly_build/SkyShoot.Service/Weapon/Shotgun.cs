﻿using System;

using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.Service.Weapon
{
	public class Shotgun : AWeapon
	{
		private Random _rand;

		private void Init()
		{
			_rand = new Random();
			Type = AObtainableDamageModifiers.Shotgun;
            _reloadSpeed = SkyShoot.Contracts.Constants.SHOTGUN_ATTACK_RATE;
		}

		public Shotgun(Guid id) : base(id)
		{
			Init();
		}

		public Shotgun(Guid id, AMob owner) : base(id, owner)
		{
			Init();
		}

		public override AProjectile[] CreateBullets(AMob owner, Vector2 direction)
		{
			ShotgunBullet[] bullets = new ShotgunBullet[8];

			for (int i = 0; i < 8; i++)
			{
				bullets[i] = new ShotgunBullet(owner, Guid.NewGuid(),
				                               Vector2.Transform(direction,
				                                                 Matrix.CreateRotationZ(
				                                                 	(float) (-Math.PI/6f + _rand.NextDouble()*Math.PI/3f))));
			}

			return bullets;
		}
	}
}