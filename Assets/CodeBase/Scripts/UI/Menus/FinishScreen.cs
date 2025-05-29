using CodeBase.Scripts.Managers;
using CodeBase.Scripts.UI.Menus.Base;
using CodeBase.Scripts.Utils;
using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.UI.Menus
{
    public class FinishScreen : UiMenu
    {
        [SerializeField] private ObjectScaler objectScaler;
        [SerializeField] private TypeText typeText;

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            switch (state)
            {
                case GameState.Victory:
                case GameState.Defeat:
                    objectScaler.Scale();
                    typeText.StartAnimation();
                    break;
            }
        }
    }
}
