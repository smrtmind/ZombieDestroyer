using CodeBase.Scripts.Service;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Scripts.Weapons
{
    public class WeaponRotator : MonoBehaviour
    {
        [SerializeField] private TouchController touchController;
        [SerializeField] private Transform pivot;

        [Space]
        [SerializeField, Range(0f, 90f)] private float maxRotationAngle = 45f;
        [SerializeField, Min(0.1f)] private float returnDuration = 0.5f;

        private Tween _returnTween;

        private float _startTouchX;
        private float _startAngle;
        private bool _isDragging;

        private void OnEnable()
        {
            Subscribe();

            pivot.localRotation = Quaternion.identity;
        }

        private void Subscribe()
        {
            touchController.OnTouched += OnTouchedHandler;
            touchController.OnReleased += OnReleasedHandler;
        }

        private void Unsubscribe()
        {
            touchController.OnTouched -= OnTouchedHandler;
            touchController.OnReleased -= OnReleasedHandler;
        }

        private void OnTouchedHandler()
        {
            _returnTween?.Kill();

            _isDragging = true;

#if UNITY_EDITOR
            _startTouchX = Input.mousePosition.x;
#else
            _startTouchX = Input.GetTouch(0).position.x;
#endif

            float raw = pivot.localEulerAngles.y;
            _startAngle = raw > 180f ? raw - 360f : raw;
        }

        private void OnReleasedHandler()
        {
            _isDragging = false;

            ReturnToCenter();
        }

        private void Update()
        {
            if (!_isDragging) return;

            float currentX;
#if UNITY_EDITOR
            currentX = Input.mousePosition.x;
#else
            currentX = Input.GetTouch(0).position.x;
#endif
            RotateTowards(currentX);
        }

        private void RotateTowards(float currentX)
        {
            float deltaX = currentX - _startTouchX;
            float normalized = deltaX * 2f / Screen.width;

            float angle = Mathf.Clamp(
                _startAngle + normalized * maxRotationAngle,
                -maxRotationAngle,
                +maxRotationAngle);

            pivot.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        private void ReturnToCenter()
        {
            _returnTween?.Kill();
            _returnTween = pivot
                .DOLocalRotate(Vector3.zero, returnDuration)
                .SetEase(Ease.OutCubic)
                .OnKill(() => _returnTween = null)
                .OnComplete(() => _returnTween = null);
        }

        private void OnDisable()
        {
            Unsubscribe();

            _returnTween?.Kill();
        }
    }
}
