using UnityEngine;

namespace CodeBase.Scripts.Damageable
{
    public struct DamageResult
    {
        public float DealtDamage { get; private set; }
        public GameObject DamagerObject { get; private set; }
        public IDamageable Victim { get; private set; }

        public bool IsNegative => DealtDamage < 0f;

        public DamageResult(float dealtDamage, GameObject damagerObject, IDamageable victim)
        {
            DealtDamage = dealtDamage;
            DamagerObject = damagerObject;
            Victim = victim;
        }
    }
}
