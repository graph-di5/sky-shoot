﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts;

namespace SkyShoot.Service.Weapon
{
	//!!@todo 2 delete
	class Claw:AWeapon
	{
		public Claw(Guid id) : base(id) { Owner = null; }

		public Claw(Guid id, Contracts.Mobs.AGameObject owner)
			: base(id) 
		{
			this.Owner = owner;
			_reloadSpeed = Constants.CLAW_ATTACK_RATE;
		}

		public override Contracts.Weapon.Projectiles.AProjectile[] CreateBullets(Contracts.Mobs.AGameObject owner, SkyShoot.XNA.Framework.Vector2 direction)
		{
			throw new NotImplementedException();
		}
	}
}
