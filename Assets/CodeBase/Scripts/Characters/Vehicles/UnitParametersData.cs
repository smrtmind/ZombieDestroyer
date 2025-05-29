using System;
using UnityEngine;

namespace CodeBase.Scripts.Characters.Vehicles
{
    [Serializable]
    public class UnitParametersData
    {
        [field: SerializeField, Min(0f)] public float Health { get; private set; }
        [field: SerializeField, Min(0f)] public float Damage { get; private set; }
    }
}
