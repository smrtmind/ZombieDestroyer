using DG.Tweening;
using System;
using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    public class ObjectScaler : MonoBehaviour
    {
        [SerializeField] private bool scaleOnEnable = false;

        [Space]
        [SerializeField] private Ease ease = Ease.OutBack;
        [SerializeField] private Vector3 startScale;
        [SerializeField] private Vector3 endScale;
        [SerializeField, Min(0f)] private float scaleDuration = 0.2f;

        private Tween _scaleTween;

        private void OnEnable()
        {
            if (!scaleOnEnable) return;

            Scale();
        }

        public void Scale(Action onComplete = null)
        {
            transform.localScale = startScale;

            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(endScale, scaleDuration)
                .SetEase(ease)
                .SetUpdate(true)
                .SetLink(gameObject)
                .OnComplete(() => onComplete?.Invoke());
        }

        private void OnDisable()
        {
            _scaleTween?.Kill();
        }
    }
}
