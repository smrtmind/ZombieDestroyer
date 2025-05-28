using CodeBase.Scripts.Projectiles;
using Unavinar.Pooling;
using UnityEngine;
using Zenject;

namespace CodeBase.Scripts.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected Transform shootPoint;
        [SerializeField] private Projectile projectile;
        [SerializeField] private PoolableParticle shootFx;

        [Header("Parameters")]
        [SerializeField, Range(1f, 100f)] protected float aimDistance = 10f;

        private ObjectPool _objectPool;

        [Inject]
        private void Construct(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        protected virtual void OnEnable()
        {
            Subscribe();
        }

        protected virtual void Subscribe() { }

        protected virtual void Unsubscribe() { }

        public virtual void Shoot(Vector3 targetPosition, float damage, bool isCritical)
        {
            ShootFx();
        }

        protected Projectile GetProjectile()
        {
            var projectile = _objectPool.Get(this.projectile);

            projectile.transform.position = shootPoint.position;
            projectile.transform.rotation = shootPoint.rotation;

            return projectile;
        }

        protected void ShootFx()
        {
            if (this.shootFx == null) return;

            var shootFx = _objectPool.Get(this.shootFx);
            shootFx.transform.position = shootPoint.position;
        }

        protected virtual void OnDisable()
        {
            Unsubscribe();
        }
    }
}
