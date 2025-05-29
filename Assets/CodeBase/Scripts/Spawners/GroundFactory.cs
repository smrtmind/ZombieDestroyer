using CodeBase.Scripts.Environment;
using CodeBase.Scripts.Managers;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unavinar.Pooling;
using UnityEngine;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.Spawners
{
    public class GroundFactory : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Ground groundPrefab;

        [field: Header("Parameters")]
        [field: SerializeField, Min(2)] public int TotalGroundPlates { get; private set; } = 10;
        [SerializeField, Min(1)] private int maxGroundPlatesAlive = 2;
        [SerializeField] private float distanceAheadOfPlayer = 30f;

        public float GroundPlateSize => groundPrefab.Size.z;

        private readonly List<Ground> _activeGroundPlates = new();
        private ObjectPool _objectPool;
        private MatchManager _matchManager;
        private Ground _lastSpawnedGroundPlate;
        private CancellationTokenSource _cts;

        private int _platesSpawnedCount;

        public static event Action<Ground> OnNewGroundPlateSpawned;

        [Inject]
        private void Construct(ObjectPool objectPool, MatchManager matchManager)
        {
            _objectPool = objectPool;
            _matchManager = matchManager;
        }

        private void OnEnable()
        {
            Subscribe();

            _platesSpawnedCount = 0;
            SpawnFirstGroundPlate();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            switch (state)
            {
                case GameState.Gameplay:
                    StartSpawn();
                    break;

                case GameState.Victory:
                case GameState.Defeat:
                    StopSpawn();
                    break;
            }
        }

        private void StartSpawn()
        {
            StopSpawn();

            _cts = new CancellationTokenSource();
            SpawnLoopAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid SpawnLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _platesSpawnedCount < TotalGroundPlates)
            {
                if (ShouldSpawnNewSection())
                {
                    RemoveOldestGroundPlate();
                    SpawnNewGroundPlate();
                }

                await UniTask.Yield(token);
            }
        }

        private void StopSpawn()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private bool ShouldSpawnNewSection()
        {
            var distanceToLastSection = _matchManager.ActiveVehicle.transform.position.z - _lastSpawnedGroundPlate.transform.position.z;
            return distanceToLastSection > distanceAheadOfPlayer;
        }

        private void SpawnNewGroundPlate()
        {
            var newPosisitonByZ = _lastSpawnedGroundPlate.transform.position.z + _lastSpawnedGroundPlate.Size.z;

            _lastSpawnedGroundPlate = GetNewGroundPlate();
            _lastSpawnedGroundPlate.transform.position = new Vector3(0f, 0f, newPosisitonByZ);
            _platesSpawnedCount++;

            OnNewGroundPlateSpawned?.Invoke(_lastSpawnedGroundPlate);
        }

        private void SpawnFirstGroundPlate()
        {
            _lastSpawnedGroundPlate = GetNewGroundPlate();
            _lastSpawnedGroundPlate.transform.position = Vector3.zero;
        }

        private Ground GetNewGroundPlate()
        {
            var section = _objectPool.Get(groundPrefab);
            _activeGroundPlates.Add(section);

            return section;
        }

        private void RemoveOldestGroundPlate()
        {
            if (_activeGroundPlates.Count < maxGroundPlatesAlive) return;

            var oldestGroundPlate = _activeGroundPlates[0];
            oldestGroundPlate.Release();
            _activeGroundPlates.RemoveAt(0);
        }

        private void ReleaseAllSections()
        {
            foreach (var section in _activeGroundPlates)
                section?.Release();

            _activeGroundPlates.Clear();
        }

        private void OnDisable()
        {
            Unsubscribe();
            StopSpawn();
            ReleaseAllSections();
        }
    }
}
