using CodeBase.Scripts.Service;
using UnityEngine;

namespace CodeBase.Scripts.Weapons
{
    public class WeaponRotator : MonoBehaviour
    {
        [SerializeField] private TouchController touchController;
        [SerializeField] private Transform pivot;

        [Space]
        [SerializeField, Range(0f, 90f)] private float maxRotationAngle = 45f;

        private Vector3 _defaultForward;

        private void OnEnable()
        {
            _defaultForward = pivot.forward;
            pivot.localRotation = Quaternion.identity;
        }

        private void Update()
        {
            RotateTowardsTarget(touchController.TargetPosition);
        }

        private void RotateTowardsTarget(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - pivot.position;
            direction.y = 0;

            if (direction == Vector3.zero)
                return;

            direction.Normalize();

            float angle = -Vector3.SignedAngle(_defaultForward, direction, Vector3.up);
            angle = Mathf.Clamp(angle, -maxRotationAngle, maxRotationAngle);

            Quaternion clampedRotation = Quaternion.AngleAxis(angle, Vector3.up);
            pivot.localRotation = clampedRotation;
        }
    }
}
