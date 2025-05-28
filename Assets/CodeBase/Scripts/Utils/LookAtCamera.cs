using CodeBase.Scripts.Utils;
using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace Game.Scripts.Runtime
{
    [DefaultExecutionOrder(99)]
    public class LookAtCamera : MonoBehaviour
    {
        public enum UpdateType { FixedUpdate, LateUpdate }

        [SerializeField] private UpdateType updateType = UpdateType.LateUpdate;
        [SerializeField] private Axis axisConstraints;
        [SerializeField] private bool byCameraRotation;

        private Camera _camera;
        private Vector3 _originalRotationAngles;

        private void Start()
        {
            _camera = Camera.main;
            _originalRotationAngles = transform.eulerAngles;
        }

        public void FixedUpdate()
        {
            if (updateType.Equals(UpdateType.FixedUpdate))
                UpdateRotation();
        }

        public void LateUpdate()
        {
            if (updateType.Equals(UpdateType.LateUpdate))
                UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (_camera == null) return;

            if (byCameraRotation)
            {
                transform.rotation = Quaternion.LookRotation(_camera.transform.rotation * -Vector3.forward, _camera.transform.rotation * Vector3.up);
            }
            else
            {
                Vector3 direction = _camera.orthographic ? -_camera.transform.forward : _camera.transform.position - transform.position;

                if (!direction.Approximately(Vector3.zero))
                {
                    var newRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Euler(ApplyAxisConstraint(_originalRotationAngles, newRotation.eulerAngles));
                }
            }
        }

        private Vector3 ApplyAxisConstraint(Vector3 original, Vector3 input)
        {
            if (axisConstraints == 0) return input;

            if (axisConstraints.HasFlag(Axis.X)) input.x = original.x;
            if (axisConstraints.HasFlag(Axis.Y)) input.y = original.y;
            if (axisConstraints.HasFlag(Axis.Z)) input.z = original.z;

            return input;
        }
    }
}
