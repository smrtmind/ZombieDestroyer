using CodeBase.Scripts.CameraLogic;
using CodeBase.Scripts.Damageable;
using CodeBase.Scripts.Managers;
using CodeBase.Scripts.Utils;
using Unavinar.Pooling;
using UnityEngine;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.Projectiles
{
    public abstract class Projectile : PoolableMonoBehaviour
    {
        #region Variables
        [SerializeField] private TriggerObserver triggerObserver;
        [SerializeField] protected PoolableParticle fxOnContact;

        [Space]
        [SerializeField, Min(0f)] protected float speed = 3f;
        [SerializeField, Min(0.01f)] private float targetReachTolerance = 0.01f;

        protected CameraController _cameraController;
        protected ObjectPool _objectPool;
        protected GameObject _damagerObject;
        protected Vector3 _targetPosition;

        protected bool _isCritical;
        protected float _damage;
        #endregion

        [Inject]
        private void Construct(CameraController cameraController, ObjectPool objectPool)
        {
            _cameraController = cameraController;
            _objectPool = objectPool;
        }

        protected virtual void OnEnable()
        {
            Subscribe();
        }

        protected virtual void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
            triggerObserver.OnEnter += OnTriggerEnterHandler;
        }

        protected virtual void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
            triggerObserver.OnEnter -= OnTriggerEnterHandler;
        }

        protected virtual void Update()
        {
            if (Vector3.Distance(transform.position, _targetPosition) < targetReachTolerance)
                OnReachedTargetPosition();
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            switch (state)
            {
                case GameState.Victory:
                case GameState.Defeat:
                    Release();
                    break;
            }
        }

        public virtual void Set(GameObject damagerObject, Vector3 targetPosition, float damage, bool isCritical)
        {
            _damagerObject = damagerObject;
            _targetPosition = targetPosition;
            _damage = damage;
            _isCritical = isCritical;
        }

        protected void RotateTowardsTarget()
        {
            Vector3 direction = _targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        protected abstract void DoEffect(IDamageable damagableObject);

        protected abstract void PerformBehaviour();

        protected abstract void OnReachedTargetPosition();

        protected void SpawnFx()
        {
            if (fxOnContact == null) return;

            var contactFx = _objectPool.Get(fxOnContact);
            contactFx.transform.position = transform.position;
        }

        private void OnTriggerEnterHandler(Collider triggerCollider)
        {
            var damagable = triggerCollider.GetComponent<IDamageable>();
            if (damagable != null)
                DoEffect(damagable);
        }

        protected virtual void OnDisable()
        {
            Unsubscribe();
        }
    }
}
