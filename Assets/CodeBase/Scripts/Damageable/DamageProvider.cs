using UnityEngine;

namespace CodeBase.Scripts.Damageable
{
    public class DamageProvider : MonoBehaviour
    {
        [field: SerializeField, Min(0f)] public float DefaultDamage { get; private set; }
        [field: SerializeField, Min(0f)] public float Damage { get; private set; }

        private void OnEnable()
        {
            RestoreDamage();
        }

        private void SetDamage(float damage)
        {
            var clampedDamage = Mathf.Clamp(damage, 0f, DefaultDamage);

            if (Mathf.Approximately(Damage, clampedDamage))
                return;

            Damage = clampedDamage;
        }

        private void RestoreDamage() => SetDamage(DefaultDamage);
    }
}
