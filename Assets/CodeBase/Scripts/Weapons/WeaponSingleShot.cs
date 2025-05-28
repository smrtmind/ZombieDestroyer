using CodeBase.Scripts.Projectiles;
using UnityEngine;

namespace CodeBase.Scripts.Weapons
{
    public abstract class WeaponSingleShot : Weapon
    {
        protected Projectile _projectile;

        public override void Shoot(Vector3 targetPosition, float damage, bool isCritical)
        {
            base.Shoot(targetPosition, damage, isCritical);

            _projectile = GetProjectile();

            Vector3 shootDirection = shootPoint.forward;
            _projectile.Set(gameObject, shootPoint.position + shootDirection * aimDistance, damage, isCritical);
        }
    }
}
