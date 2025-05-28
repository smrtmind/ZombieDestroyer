using CodeBase.Scripts.Damageable;
using CodeBase.Scripts.Utils;
using UnityEngine;

namespace CodeBase.Scripts.Characters.Enemies
{
    public class KamikazeEnemy : Enemy
    {
        [SerializeField] private TriggerObserver triggerObserver;

        protected override void Subscribe()
        {
            base.Subscribe();

            triggerObserver.OnEnter += OnEnterTriggerHandler;
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();

            triggerObserver.OnEnter -= OnEnterTriggerHandler;
        }

        private void OnEnterTriggerHandler(Collider collider)
        {
            _vehicleDamageable.DoDamage(new DamageResult(-damageProvider.Damage, gameObject, _vehicleDamageable));

            OnDiedHandler();
            damageableObject.Kill();
        }
    }
}
