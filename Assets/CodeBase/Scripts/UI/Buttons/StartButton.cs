using CodeBase.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.UI.Buttons
{
    public class StartButton : MonoBehaviour
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

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(StartGameplay);
        }

        private void StartGameplay() => _gameManager.ChangeState(GameState.Gameplay);
    }
}
