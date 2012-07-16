﻿using System;
using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Weapon
{
	public enum WeaponType
	{
		[EnumMember]
		Pistol,

		[EnumMember]
		Shotgun,

		[EnumMember]
		RocketPistol,

		[EnumMember]
		SpiderPistol,

		[EnumMember]
		Heater,

		[EnumMember]
		FlamePistol,

		[EnumMember]
		MobGenerator,

		[EnumMember]
		PoisonGun,

		[EnumMember]
		PoisonTick,

		[EnumMember]
		TurretMaker,

		[EnumMember]
		TurretGun
	}
}