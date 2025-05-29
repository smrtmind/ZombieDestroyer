using DG.Tweening;
using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    public class ObjectPuncher : MonoBehaviour
    {
        [SerializeField] private Vector3 additionalScale;
        [SerializeField, Min(0f)] private float scaleDuration = 0.2f;

        private Tween _punchTween;
        private Vector3 _defaultScale;

        private bool _isScaling;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            transform.localScale = _defaultScale;
            _isScaling = false;
        }

        public void Punch()
        {
            if (_isScaling) return;

            _isScaling = true;
            transform.localScale = _defaultScale;

            _punchTween?.Kill();
            _punchTween = transform.DOPunchScale(additionalScale, scaleDuration, 1)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .SetUpdate(true)
                .OnComplete(() => _isScaling = false);
        }

        private void OnDisable()
        {
            _punchTween?.Kill();
        }
    }
}
