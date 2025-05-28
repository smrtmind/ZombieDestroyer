using CodeBase.Scripts.Damageable;
using CodeBase.Scripts.Managers;
using CodeBase.Scripts.Service;
using CodeBase.Scripts.Weapons;
using Unavinar.Pooling;
using UnityEngine;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.Characters.Vehicles
{
    public class Vehicle : PoolableMonoBehaviour
    {
        [SerializeField] private PoolableParticle dieFxPrefab;
        [SerializeField] private VehicleMovement vehicleMovement;
        [SerializeField] private WeaponRotator weaponRotator;
        [SerializeField] private TouchController touchController;
        [field: SerializeField] public VehicleDamageableObject DamageableObject { get; private set; }

        private ObjectPool _objectPool;

        [Inject]
        private void Construct(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        private void OnEnable()
        {
            Subscribe();

            vehicleMovement.enabled = false;
            weaponRotator.enabled = false;
            touchController.enabled = false;
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
            DamageableObject.OnDied += OnDiedHandler;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
            DamageableObject.OnDied -= OnDiedHandler;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            if (state != GameState.Gameplay) return;

            vehicleMovement.enabled = true;
            weaponRotator.enabled = true;
            touchController.enabled = true;
        }

        private void OnDiedHandler()
        {
            vehicleMovement.enabled = false;
            weaponRotator.enabled = false;
            touchController.enabled = false;

            SpawnFx();
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
