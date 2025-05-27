using Cinemachine;
using System;
using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.CameraLogic
{
    [Serializable]
    public class VirtualCameraData
    {
        [field: SerializeField] public VcamType Type { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }
    }
}
