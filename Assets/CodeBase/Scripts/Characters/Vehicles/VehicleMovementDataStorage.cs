using UnityEngine;

namespace CodeBase.Scripts.Characters.Vehicles
{
    [CreateAssetMenu(fileName = nameof(VehicleMovementDataStorage), menuName = "ScriptableObjects/" + nameof(VehicleMovementDataStorage))]
    public class VehicleMovementDataStorage : ScriptableObject
    {
        [SerializeField] private VehicleMovementData vehicleMovementData;

        public VehicleMovementData MovementData => vehicleMovementData;
    }
}
