﻿using System;

using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Contracts;

namespace SkyShoot.Service.Weapon.Bullets
{
    public class ShotgunBullet : AProjectile
    {
       
        private const EnumBulletType TYPE = EnumBulletType.Bullet;

        public ShotgunBullet(AMob owner, Guid id, Vector2 direction)
            : base(owner, id, direction) 
        {
            Speed = Constants.SHOTGUN_BULLET_SPEED;
            Damage = Constants.SHOTGUN_BULLET_DAMAGE;
			LifeDistance = Constants.SHOTGUN_BULLET_LIFE_DISTANCE;
            Type = TYPE;
        }
    }
}