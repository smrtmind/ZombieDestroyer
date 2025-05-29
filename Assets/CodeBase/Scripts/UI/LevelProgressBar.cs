using CodeBase.Scripts.Managers;
using CodeBase.Scripts.Spawners;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.UI
{
    public class LevelProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider progressSlider;

        private float DistanceToWin => _groundFactory.GroundPlateSize * (_groundFactory.TotalGroundPlates - 1);

        private MatchManager _matchManager;
        private GroundFactory _groundFactory;
        private CancellationTokenSource _cts;

        public static event Action OnLevelCompleted;

        [Inject]
        private void Construct(MatchManager matchManager, GroundFactory groundFactory)
        {
            _matchManager = matchManager;
            _groundFactory = groundFactory;
        }

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
        }

        private void OnDisable()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;

            _cts?.Cancel();
            _cts?.Dispose();
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            if (state != GameState.Gameplay) return;

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            RefreshProgressAsync(_cts.Token).Forget();
        }

        private void Init()
        {
            progressSlider.maxValue = DistanceToWin;
            progressSlider.minValue = 0f;
            progressSlider.value = 0f;
        }

        private async UniTaskVoid RefreshProgressAsync(CancellationToken token)
        {
            var vehicleTransform = _matchManager.ActiveVehicle.transform;

            while (vehicleTransform.position.z < DistanceToWin && !token.IsCancellationRequested)
            {
                progressSlider.value = Mathf.Clamp(vehicleTransform.position.z, 0f, progressSlider.maxValue);
                await UniTask.NextFrame(token);
            }

            if (!token.IsCancellationRequested)
            {
                progressSlider.value = progressSlider.maxValue;
                OnLevelCompleted?.Invoke();
            }
        }
    }
}
