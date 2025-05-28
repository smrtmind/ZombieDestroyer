using UnityEngine;

namespace CodeBase.Scripts.Damageable
{
    public interface IDamageable
    {
        public float MaxHealth { get; }
        public float Health { get; }
        public bool IsDead { get; }

        public Transform GetTransform { get; }

        public void DoDamage(DamageResult result);

        public void SetHealth(float health);

        public void RestoreHealth();
    }
}
