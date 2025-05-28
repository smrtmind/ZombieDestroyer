using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase.Scripts.UI.Buttons
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button restartButton;

        private void OnEnable()
        {
            restartButton.onClick.AddListener(StartGameplay);
        }

        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(StartGameplay);
        }

        private void StartGameplay() => SceneManager.LoadSceneAsync(0);
    }
}
