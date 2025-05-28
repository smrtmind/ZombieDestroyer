using DG.Tweening;
using TMPro;
using Unavinar.Pooling;
using UnityEngine;

namespace CodeBase.Scripts.UI
{
    public class PopupElement : PoolableMonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private TMP_Text _text;

        [Header("Parameters")]
        [SerializeField] private Color fadeColor;
        [SerializeField, Min(0.1f)] private float scaleDuration = 0.25f;
        [SerializeField, Min(0.1f)] private float moveDuration = 0.5f;
        [SerializeField] private float offsetPositionY = 0.5f;

        [Space]
        [SerializeField] private Vector3 normalScale;

        private Sequence _sequence;
        #endregion

        public void Setup(string text, Color color)
        {
            _text.text = text;
            _text.color = color;
        }

        public void Run()
        {
            transform.localScale = Vector3.zero;

            _sequence?.Kill();
            _sequence = DOTween.Sequence().SetLink(gameObject);

            _sequence.Append(transform.DOScale(normalScale, scaleDuration).SetEase(Ease.OutBack))
                .AppendCallback(() =>
                {
                    transform.DOMoveY(transform.position.y + offsetPositionY, moveDuration)
                        .SetEase(Ease.Linear)
                        .SetLink(gameObject);
                    _text.DOColor(fadeColor, moveDuration)
                        .SetEase(Ease.Linear)
                        .SetLink(gameObject);
                })
                .AppendInterval(moveDuration)
                .OnComplete(Release);
        }

        private void OnDisable()
        {
            _sequence?.Kill();

            DOTween.Kill(transform);
            DOTween.Kill(_text);
        }
    }
}
