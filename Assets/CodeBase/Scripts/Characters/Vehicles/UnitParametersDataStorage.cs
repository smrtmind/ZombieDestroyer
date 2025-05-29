using UnityEngine;

namespace CodeBase.Scripts.Characters.Vehicles
{
    [CreateAssetMenu(fileName = nameof(UnitParametersDataStorage), menuName = "ScriptableObjects/" + nameof(UnitParametersDataStorage))]
    public class UnitParametersDataStorage : ScriptableObject
    {
        [SerializeField] private UnitParametersData unitData;

        public UnitParametersData UnitData => unitData;
    }
}
