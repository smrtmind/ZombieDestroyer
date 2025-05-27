using CodeBase.Scripts.Characters.Enemies;
using CodeBase.Scripts.Environment;
using CodeBase.Scripts.Managers;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Unavinar.Pooling;
using UnityEngine;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;
using Random = UnityEngine.Random;

namespace CodeBase.Scripts.Spawners
{
    public class EnemyFactory : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Enemy enemyPrefab;

        [Header("Parameters")]
        [SerializeField, Min(1)] private float enemiesToSpawnPerGroundPlate = 20f;
        [SerializeField] private float enemiesBehindVehiclePositionZ = 10f;

        private readonly HashSet<Enemy> _activeEnemies = new();
        private ObjectPool _objectPool;
        private MatchManager _matchManager;
        private CancellationTokenSource _cts;

        [Inject]
        private void Construct(ObjectPool objectPool, MatchManager matchManager)
        {
            _objectPool = objectPool;
            _matchManager = matchManager;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
            GroundFactory.OnNewGroundPlateSpawned += OnNewGroundPlateSpawnedHandler;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
            GroundFactory.OnNewGroundPlateSpawned -= OnNewGroundPlateSpawnedHandler;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            switch (state)
            {
                case GameState.Victory:
                case GameState.Defeat:
                    ReleaseAllEnemies();
                    StopSpawn();
                    break;
            }
        }

        private void OnNewGroundPlateSpawnedHandler(Ground ground)
        {
            ReleaseEnemiesBehind();
            StopSpawn();

            _cts = new CancellationTokenSource();
            SpawnEnemiesOnGroundAsync(ground, _cts.Token).Forget();
        }

        private async UniTaskVoid SpawnEnemiesOnGroundAsync(Ground ground, CancellationToken token)
        {
            for (int i = 0; i < enemiesToSpawnPerGroundPlate; i++)
            {
                token.ThrowIfCancellationRequested();
                SpawnNewEnemy(GetSpawnPosition(ground));

                await UniTask.Yield(token);
            }
        }

        private void SpawnNewEnemy(Vector3 spawnPosition)
        {
            var enemy = _objectPool.Get(enemyPrefab);
            enemy.transform.position = spawnPosition;
            _activeEnemies.Add(enemy);
        }

        private void ReleaseEnemiesBehind()
        {
            var vehiclePositionZ = _matchManager.ActiveVehicle.transform.position.z;
            var enemiesToRelease = new HashSet<Enemy>();

            foreach (var enemy in _activeEnemies)
            {
                if (enemy.transform.position.z < vehiclePositionZ - enemiesBehindVehiclePositionZ)
                    enemiesToRelease.Add(enemy);
            }

            foreach (var enemy in enemiesToRelease)
            {
                enemy.Release();
                _activeEnemies.Remove(enemy);
            }
        }

        private void StopSpawn()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private Vector3 GetSpawnPosition(Ground ground)
        {
            Vector3 center = ground.transform.position;
            Vector3 size = ground.Size;

            return center + new Vector3(
                Random.Range(-size.x / 2f, size.x / 2f),
                0f,
                Random.Range(-size.z / 2f, size.z / 2f)
                );
        }

        private void ReleaseAllEnemies()
        {
            foreach (var enemy in _activeEnemies)
                enemy?.Release();

            _activeEnemies.Clear();
        }

        private void OnDisable()
        {
            Unsubscribe();
            StopSpawn();
            ReleaseAllEnemies();
        }
    }
}
