using CodeBase.Scripts.Managers;
using CodeBase.Scripts.UI.Menus.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.UI.Menus
{
    public class LobbyMenu : UiMenu
    {
        [SerializeField] private Button startButton;

        private GameManager _gameManager;

        [Inject]
        private void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void OnEnable()
        {
            startButton.onClick.AddListener(StartGameplay);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            startButton.onClick.RemoveListener(StartGameplay);
        }

        private void StartGameplay() => _gameManager.ChangeState(GameState.Gameplay);
    }
}
