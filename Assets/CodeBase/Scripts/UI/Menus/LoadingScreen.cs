using CodeBase.Scripts.Managers;
using CodeBase.Scripts.UI.Menus.Base;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.UI.Menus
{
    public class LoadingScreen : UiMenu
    {
        [SerializeField] private Image filler;
        [SerializeField, Min(1f)] private float loadingDuration = 2f;

        private GameManager _gameManager;
        private Tween _loadingTween;

        [Inject]
        private void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;

            _loadingTween?.Kill();
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            if (state != GameState.Loading) return;

            ImitateLoading();
        }

        private void ImitateLoading()
        {
            _loadingTween?.Kill();
            _loadingTween = filler.DOFillAmount(1f, loadingDuration)
                .From(0f)
                .SetUpdate(true)
                .OnComplete(() => _gameManager.ChangeState(GameState.Lobby));
        }
    }
}
