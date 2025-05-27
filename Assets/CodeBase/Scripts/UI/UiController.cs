using CodeBase.Scripts.Managers;
using CodeBase.Scripts.UI.Menus.Base;
using System.Linq;
using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private UiMenu[] menus;

        public UiMenu ActiveUiMenu { get; private set; }

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnGameStateChangeHandler;

            HideAllMenus();
        }

        private void OnDisable()
        {
            GameManager.OnAfterStateChanged -= OnGameStateChangeHandler;
        }

        private void OnGameStateChangeHandler(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Loading:
                    ActiveUiMenu = GetMenu(gameState);
                    ActiveUiMenu.ShowInstantly();
                    break;

                case GameState.Lobby:
                    SwitchElementSmoothlyTo(GetMenu(gameState));
                    break;

                case GameState.Gameplay:
                case GameState.Victory:
                case GameState.Defeat:
                    SwitchElementImmediatelyTo(GetMenu(gameState));
                    break;
            }
        }

        private UiMenu GetMenu(GameState state) => menus.FirstOrDefault(menu => menu.State == state);

        private void SwitchElementImmediatelyTo(UiMenu menu)
        {
            ActiveUiMenu.HideInstantly();
            ActiveUiMenu = menu;
            ActiveUiMenu.ShowInstantly();
        }

        private void SwitchElementSmoothlyTo(UiMenu menu)
        {
            ActiveUiMenu.Hide();
            ActiveUiMenu = menu;
            ActiveUiMenu.ShowInstantly();
        }

        private void HideAllMenus()
        {
            foreach (var menu in menus)
                menu.HideInstantly();
        }
    }
}
