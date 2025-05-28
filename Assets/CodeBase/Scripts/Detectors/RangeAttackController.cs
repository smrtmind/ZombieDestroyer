using CodeBase.Scripts.Damageable;
using CodeBase.Scripts.Weapons;
using UnityEngine;

namespace CodeBase.Scripts.Detectors
{
    public abstract class RangeAttackController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected Weapon weapon;
        [SerializeField] protected DamageProvider damageProvider;

        protected virtual void OnEnable()
        {
            Subscribe();
        }

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();


        protected virtual void OnDisable()
        {
            Unsubscribe();
        }
    }
}
