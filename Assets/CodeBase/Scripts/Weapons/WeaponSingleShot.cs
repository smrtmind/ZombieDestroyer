using CodeBase.Scripts.Projectiles;
using UnityEngine;

namespace CodeBase.Scripts.Weapons
{
    public abstract class WeaponSingleShot : Weapon
    {
        protected Projectile _projectile;

        public override void Shoot(Vector3 targetPosition, float damage)
        {
            base.Shoot(targetPosition, damage);

            Vector3 shootDirection = shootPoint.forward;

            _projectile = GetProjectile();
            _projectile.Set(gameObject, shootPoint.position + shootDirection * aimDistance, damage);
            _projectile.transform.rotation = Quaternion.LookRotation(shootDirection);
        }
    }
}
