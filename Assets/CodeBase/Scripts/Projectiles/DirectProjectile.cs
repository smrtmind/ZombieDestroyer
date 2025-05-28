using CodeBase.Scripts.Damageable;
using UnityEngine;

namespace CodeBase.Scripts.Projectiles
{
    public class DirectProjectile : Projectile
    {
        private Vector3 _direction;

        protected override void Update()
        {
            base.Update();

            PerformBehaviour();
        }

        protected override void PerformBehaviour()
        {
            var step = speed * Time.deltaTime;
            transform.position += _direction * step;
        }

        public override void Set(GameObject damagerObject, Vector3 targetPosition, float damage, bool isCritical)
        {
            base.Set(damagerObject, targetPosition, damage, isCritical);

            _direction = (_targetPosition - transform.position).normalized;
            RotateTowardsTarget();
        }

        protected override void DoEffect(IDamageable damagableObject)
        {
            damagableObject.DoDamage(new DamageResult(-_damage, _damagerObject, damagableObject));
            SpawnFx();
            Release();
        }

        protected override void OnReachedTargetPosition()
        {
            return;
        }
    }
}
