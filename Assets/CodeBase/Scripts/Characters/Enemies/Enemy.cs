using CodeBase.Scripts.Characters.Enemies.AI;
using CodeBase.Scripts.Damageable;
using CodeBase.Scripts.Detectors;
using CodeBase.Scripts.Managers;
using Unavinar.Pooling;
using UnityEngine;
using Zenject;

namespace CodeBase.Scripts.Characters.Enemies
{
    public abstract class Enemy : PoolableMonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PoolableParticle dieFxPrefab;
        [SerializeField] private Detector detector;
        [SerializeField] private EnemyAI enemyAi;
        [SerializeField] protected DamageableObject damageableObject;
        [SerializeField] protected DamageProvider damageProvider;

        private ObjectPool _objectPool;
        protected DamageableObject _vehicleDamageable;

        [Inject]
        private void Construct(ObjectPool objectPool, MatchManager matchManager)
        {
            _objectPool = objectPool;
            _vehicleDamageable = matchManager.ActiveVehicle.DamageableObject;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        protected virtual void Subscribe()
        {
            detector.OnTargetDetected += OnTargetDetectedHandler;
            detector.OnTargetLost += OnTargetLostHandler;
            damageableObject.OnDied += OnDiedHandler;
        }

        protected virtual void Unsubscribe()
        {
            detector.OnTargetDetected -= OnTargetDetectedHandler;
            detector.OnTargetLost -= OnTargetLostHandler;
            damageableObject.OnDied -= OnDiedHandler;
        }

        private void OnTargetDetectedHandler() => enemyAi.SwitchTo<FollowState>();

        private void OnTargetLostHandler() => enemyAi.SwitchTo<PatrolState>();

        protected void OnDiedHandler()
        {
            SpawnFx();
            Release();
        }

        private void SpawnFx()
        {
            if (dieFxPrefab == null) return;

            var dieFx = _objectPool.Get(dieFxPrefab);
            dieFx.transform.position = transform.position;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
