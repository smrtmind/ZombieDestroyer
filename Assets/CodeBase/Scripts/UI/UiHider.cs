using DG.Tweening;
using UnityEngine;

namespace CodeBase.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UiHider : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Parameters")]
        [SerializeField] private bool isHidden;
        [SerializeField] private bool useUnscaledTime = true;

        [Space]
        [SerializeField, Min(0f)] private float showDelay = 0.5f;
        [SerializeField, Min(0f)] private float fadeDuration = 0.3f;
        [SerializeField, Range(0f, 1f)] private float showAlpha = 1f;
        [SerializeField, Range(0f, 1f)] private float hideAlpha = 0f;

        private Tween _fadeTween;

        private void Awake()
        {
            SetHiddenInstantly(isHidden);
        }

        public void Show() => SetHidden(false);

        public void ShowInstantly() => SetHiddenInstantly(false);

        public void Hide() => SetHidden(true);

        public void HideInstantly() => SetHiddenInstantly(true);

        private void SetHidden(bool isHidden)
        {
            this.isHidden = isHidden;

            float targetAlpha = isHidden ? hideAlpha : showAlpha;

            _fadeTween?.Kill();
            _fadeTween = DOVirtual.DelayedCall(showDelay, () =>
            {
                canvasGroup.DOFade(targetAlpha, fadeDuration)
                    .SetEase(Ease.Linear)
                    .SetUpdate(useUnscaledTime)
                    .OnComplete(() =>
                    {
                        canvasGroup.interactable = !isHidden;
                        canvasGroup.blocksRaycasts = !isHidden;
                    });
            });
        }

        private void SetHiddenInstantly(bool isHidden)
        {
            this.isHidden = isHidden;

            canvasGroup.alpha = isHidden ? hideAlpha : showAlpha;
            canvasGroup.interactable = !isHidden;
            canvasGroup.blocksRaycasts = !isHidden;
        }

        protected virtual void OnDisable()
        {
            _fadeTween?.Kill();
        }
    }
}
