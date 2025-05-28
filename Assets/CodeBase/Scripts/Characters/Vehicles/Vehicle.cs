using CodeBase.Scripts.Managers;
using Unavinar.Pooling;
using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.Characters.Vehicles
{
    public class Vehicle : PoolableMonoBehaviour
    {
        [SerializeField] private VehicleMovement vehicleMovement;

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;

            vehicleMovement.enabled = false;
        }

        private void OnDisable()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            if (state != GameState.Gameplay) return;

            vehicleMovement.enabled = true;
        }
    }
}
