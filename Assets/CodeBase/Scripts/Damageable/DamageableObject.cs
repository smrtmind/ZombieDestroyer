using System;
using UnityEngine;

namespace CodeBase.Scripts.Damageable
{
    public abstract class DamageableObject : MonoBehaviour, IDamageable
    {
        [field: SerializeField, Min(0f)] public float MaxHealth { get; protected set; }
        [field: SerializeField, Min(0f)] public float Health { get; protected set; }

        public Transform GetTransform => transform;
        public bool IsDead => Health <= 0f;

        public event Action<float> OnHealthChanged;
        public event Action OnHealthRestored;
        public event Action OnDamaged;
        public event Action OnDied;
        public static event Action<DamageResult> OnAnyDamaged;

        private void OnEnable()
        {
            RestoreHealth();
        }

        public virtual void DoDamage(DamageResult result)
        {
            if (result.IsNegative)
            {
                OnDamaged?.Invoke();
                OnAnyDamaged?.Invoke(result);
            }

            if (IsDead) return;

            SetHealth(Health + result.DealtDamage);
        }

        public void SetHealth(float health)
        {
            var clampedHealth = Mathf.Clamp(health, 0f, MaxHealth);

            if (Mathf.Approximately(Health, clampedHealth))
                return;

            Health = clampedHealth;
            OnHealthChanged?.Invoke(Health);

            if (IsDead) OnDied?.Invoke();
        }

        public void RestoreHealth()
        {
            SetHealth(MaxHealth);
            OnHealthRestored?.Invoke();
        }

        public void Kill() => SetHealth(0f);
    }
}
