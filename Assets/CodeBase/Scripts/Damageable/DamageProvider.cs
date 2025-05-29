using CodeBase.Scripts.Characters.Vehicles;
using UnityEngine;

namespace CodeBase.Scripts.Damageable
{
    public class DamageProvider : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private UnitParametersDataStorage dataStorage;

        private UnitParametersData Data => dataStorage.UnitData;

        public float DefaultDamage => Data.Damage;
        public float Damage { get; private set; }

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
