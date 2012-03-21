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
			//LifeDistance = otherProjectile.LifeDistance;
			Owner = otherProjectile.Owner;
		}

		public AProjectile(AProjectile other)
		{
			Copy(other);
		}

		public AGameObject Owner { get; set; }

		//[Obsolete("Use health")]
		//public float LifeDistance { get; set; }

		public override void Do(AGameObject obj)
		{
			if(Owner.Id == obj.Id) // �� ������� ��������� ����
				return;
			obj.HealthAmount -= Damage;
			// ������� ����
			IsActive = false;
		}

		public override Vector2 ComputeMovement(long updateDelay, Session.GameLevel gameLevel)
		{
			//!! rewrite
			var newCoord = base.ComputeMovement(updateDelay, gameLevel);
			var x = MathHelper.Clamp(newCoord.X, 0, gameLevel.levelHeight);
			var y = MathHelper.Clamp(newCoord.Y, 0, gameLevel.levelWidth);
			const float epsilon = 0.01f;
			// ������ ����, ������� ����� �� �����
			IsActive = (Math.Abs(newCoord.X - x) < epsilon) 
				&& (Math.Abs(newCoord.Y - y) < epsilon);
			return newCoord;
		}
	}
}
