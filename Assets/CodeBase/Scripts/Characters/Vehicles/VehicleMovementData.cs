using System;
using UnityEngine;

namespace CodeBase.Scripts.Characters.Vehicles
{
    [Serializable]
    public class VehicleMovementData
    {
        [field: Header("Forward Movement")]
        [field: SerializeField, Min(1f)] public float Acceleration { get; private set; } = 15f;
        [field: SerializeField, Min(1f)] public float MaxSpeed { get; private set; } = 20f;

        [field: Header("Turn Settings")]
        [field: SerializeField, Min(1f)] public float DelayBetweenTurns { get; private set; } = 2f;
        [field: SerializeField, Min(1f)] public float RangeX { get; private set; } = 3f;
        [field: SerializeField, Min(0.1f)] public float TurnDuration { get; private set; } = 0.6f;
        [field: SerializeField, Min(2f)] public float MaxAllowedXShift { get; private set; } = 4f;

        [field: Header("Rotation Settings")]
        [field: SerializeField] public float MaxTurnAngle { get; private set; } = 25f;
        [field: SerializeField, Min(0.1f)] public float RotationReturnSpeed { get; private set; } = 5f;
    }
}
