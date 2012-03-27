﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class DoubleDamage : AGameBonus
	{
		//public DoubleDamage(Guid id, DateTime startTime)
		//    : base(id, 1, 2, 30000, startTime,AObtainableDamageModifiers.DoubleDamage)  {  }
		public DoubleDamage(Vector2 coordinates) : base(coordinates)
		{
			this.Type = AGameObject.EnumObjectType.DoubleDamage;
			this.damageFactor = 2f;
			this._milliseconds = Constants.DOUBLE_DAMAGE_MILLISECONDS;
		}
	}
}