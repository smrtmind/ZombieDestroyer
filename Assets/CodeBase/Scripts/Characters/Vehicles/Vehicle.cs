using CodeBase.Scripts.Damageable;
using CodeBase.Scripts.Detectors;
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
        [Header("Components")]
        [SerializeField] private PoolableParticle dieFxPrefab;
        [SerializeField] private GameObject fxDieFlameContainer;
        [SerializeField] private GameObject fxSmokeContainer;

        [Space]
        [SerializeField] private VehicleMovement vehicleMovement;
        [SerializeField] private WeaponRotator weaponRotator;
        [SerializeField] private TouchController touchController;
        [SerializeField] private RangeAttackControllerAuto rangeAttackControllerAuto;
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
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            switch (state)
            {
                case GameState.Gameplay:
                    vehicleMovement.StartMovement();
                    break;

                case GameState.Victory:
                    StopLiveSimulation();
                    break;

                case GameState.Defeat:
                    StopLiveSimulation();
                    SpawnFx();

                    fxSmokeContainer.SetActive(false);
                    fxDieFlameContainer.SetActive(true);
                    break;
            }
        }

        private void StopLiveSimulation()
        {
            vehicleMovement.StopMovement();
            rangeAttackControllerAuto.StopShoot();

            weaponRotator.enabled = false;
            touchController.enabled = false;
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
