using CodeBase.Scripts.Managers;
using CodeBase.Scripts.Weapons;
using Unavinar.Pooling;
using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.Characters.Vehicles
{
    public class Vehicle : PoolableMonoBehaviour
    {
        [SerializeField] private VehicleMovement vehicleMovement;
        [SerializeField] private WeaponRotator WeaponRotator;

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;

            vehicleMovement.enabled = false;
            WeaponRotator.enabled = false;
        }

        private void OnDisable()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            if (state != GameState.Gameplay) return;

            vehicleMovement.enabled = true;
            WeaponRotator.enabled = true;
        }
    }
}
