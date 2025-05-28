using DG.Tweening;
using UnityEngine;

namespace CodeBase.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class BounceScaler : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        [Space]
        [SerializeField] private bool useUnscaledTime = true;
        [SerializeField] private bool isBouncing = true;
        [SerializeField] private float bounceTime = 0.5f;
        [SerializeField] private float bounceKoef = 0.05f;

        private Tween _bounceTween;

        private void OnEnable()
        {
            if (isBouncing)
                StartBounce();
        }

        private void StartBounce()
        {
            float targetScale = 1 + bounceKoef;

            _bounceTween?.Kill();
            _bounceTween = _rectTransform
                .DOScale(Vector3.one * targetScale, bounceTime / 2f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(useUnscaledTime);
        }

        private void StopBounce()
        {
            _bounceTween?.Kill();
            _rectTransform.localScale = Vector3.one;
        }

        private void OnDisable()
        {
            StopBounce();
        }
    }
}
