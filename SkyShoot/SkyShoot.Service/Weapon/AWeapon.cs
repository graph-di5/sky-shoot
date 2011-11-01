﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;
using Microsoft.Xna.Framework;

namespace SkyShoot.Service.Weapon
{
    public abstract class AWeapon : AObtainableDamageModifier
    {
        protected AWeapon(Guid id) : base(id) { }

        public abstract AProjectile[] CreateBullets(AMob owner, Vector2 direction); 
    }
}