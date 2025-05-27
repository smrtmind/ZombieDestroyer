using Cinemachine;
using System.Linq;
using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.CameraLogic
{
    public class CameraController : MonoBehaviour
    {
        [field: Header("Components")]
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [SerializeField] private CinemachineBrain cameraBrain;

        [Space]
        [SerializeField] private VirtualCameraData[] virtualCameraData;

        private VirtualCameraData _currentVirtualCameraData;

        public void SetFollowTarget(Transform target)
        {
            if (_currentVirtualCameraData == null || _currentVirtualCameraData.Camera.Follow == target)
                return;

            _currentVirtualCameraData.Camera.Follow = target;
        }

        public void SwitchCamera(VcamType cameraType, float blendDuration = 0.75f)
        {
            var newCameraData = GetCameraData(cameraType);
            if (newCameraData == null || newCameraData == _currentVirtualCameraData)
                return;

            _currentVirtualCameraData = newCameraData;

            SetCameraBlendDuration(blendDuration);
            SetActiveCamera(_currentVirtualCameraData);
        }

        private VirtualCameraData GetCameraData(VcamType cameraType)
            => virtualCameraData.FirstOrDefault(data => data.Type == cameraType);

        private void SetCameraBlendDuration(float blendDuration)
        {
            if (cameraBrain == null || cameraBrain.m_DefaultBlend.m_Time == blendDuration)
                return;

            cameraBrain.m_DefaultBlend.m_Time = blendDuration;
        }

        private void SetActiveCamera(VirtualCameraData activeData)
        {
            foreach (var cameraData in virtualCameraData)
                cameraData.Camera.Priority = (cameraData == activeData) ? 1 : 0;
        }
    }
}
