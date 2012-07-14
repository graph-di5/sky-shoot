﻿using System;
using SkyShoot.Contracts;
using SkyShoot.Service.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	class BonusFactory
	{
		private Random _random = new Random();

		public AGameBonus CreateBonus(Vector2 coordinates)
		{ 
			int r = _random.Next(Constants.BONUS_TYPE_COUNT);
			if (r == 0)
			{
				return new Shield(coordinates);
			}
			if (r == 1)
			{
				return new DoubleDamage(coordinates);
			}
			if (r == 2)
			{
				return new Remedy(coordinates);
			}
			else
			{
				return new Speedup(coordinates);
			}
		}
	}
}