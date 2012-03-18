using System;

using System.Runtime.Serialization;

using SkyShoot.XNA.Framework;

using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts.Weapon.Projectiles
{
	[DataContract]
	public class AProjectile : AGameObject
	{
		protected AProjectile(AGameObject owner, Guid id, Vector2 direction)
		{
			Owner = owner;
			Id = id;
			RunVector = direction;
			Coordinates = owner.Coordinates;
		}

		public AProjectile()
		{
			Owner = null;
		}

		public override void Copy(AGameObject other)
		{
			if (!(other is AProjectile))
			{
				throw new Exception("Type mistmath");
			}
			var otherProjectile = other as AProjectile;
			base.Copy(other);
			LifeDistance = otherProjectile.LifeDistance;
			Owner = otherProjectile.Owner;
		}

		public AProjectile(AProjectile other)
		{
			Copy(other);
		}

		public AGameObject Owner { get; set; }

		public float LifeDistance { get; set; }

		public Vector2 OldCoordinates;

		
	}
}
