using CodeBase.Scripts.Damageable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Scripts.Detectors
{
    public class Detector : MonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private DamageableObject damagableObject;

        [Header("Parameters")]
        [SerializeField, Min(1)] private int cacheSize = 5;
        [SerializeField, Min(0.1f)] private float detectionRadius = 3f;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Color gizmoColor;
        [SerializeField] private LayerMask targetLayer;

        public IDamageable ClosestTarget { get; private set; }

        private Collider[] _cache;
        private readonly HashSet<Collider> _detectedObjects = new();

        public event Action OnTargetDetected;
        public event Action OnTargetLost;
        #endregion

        private void Awake()
        {
            _cache = new Collider[cacheSize];
        }

        private void OnEnable()
        {
            damagableObject.OnDied += OutAttackRange;
        }

        private void OnDisable()
        {
            damagableObject.OnDied -= OutAttackRange;

            ClosestTarget = null;
            _detectedObjects.Clear();
        }

        private void FixedUpdate()
        {
            if (damagableObject != null && damagableObject.IsDead) return;

            if (InAttackRange())
                OnAttackRange();
            else
                OutAttackRange();
        }

        private bool InAttackRange()
        {
            _detectedObjects.Clear();
            int count = Physics.OverlapSphereNonAlloc(GetDetectionCenter(), detectionRadius, _cache, targetLayer);

            for (int i = 0; i < count; i++)
            {
                if (_cache[i] != null && _cache[i].transform != transform)
                    _detectedObjects.Add(_cache[i]);
            }

            return _detectedObjects.Count > 0;
        }

        private void OnAttackRange()
        {
            if (HasValidTarget()) return;

            ClosestTarget = GetClosestTarget();
            OnTargetDetected?.Invoke();
        }

        private void OutAttackRange()
        {
            ClosestTarget = null;
            OnTargetLost?.Invoke();
        }

        private IDamageable GetClosestTarget()
        {
            return _detectedObjects
                .OrderBy(c => (c.transform.position - transform.position).sqrMagnitude)
                .FirstOrDefault()
                ?.GetComponent<IDamageable>();
        }

        private bool HasValidTarget() => ClosestTarget != null && !ClosestTarget.IsDead;

        private Vector3 GetDetectionCenter() => transform.position + offset;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(GetDetectionCenter(), detectionRadius);
        }
#endif
    }
}
