using CodeBase.Scripts.CameraLogic;
using CodeBase.Scripts.Characters.Vehicles;
using Unavinar.Pooling;
using UnityEngine;
using Zenject;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.Managers
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private Vehicle vehiclePrefab;

        public Vehicle ActiveVehicle { get; private set; }

        private ObjectPool _objectPool;
        private GameManager _gameManager;
        private CameraController _cameraController;

        [Inject]
        private void Construct(ObjectPool objectPool, GameManager gameManager, CameraController cameraController)
        {
            _objectPool = objectPool;
            _gameManager = gameManager;
            _cameraController = cameraController;
        }

        private void OnEnable()
        {
            SpawnVehicle();
            Subscribe();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
            ActiveVehicle.DamageableObject.OnDied += EndMatch;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
            ActiveVehicle.DamageableObject.OnDied -= EndMatch;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            if (state != GameState.Gameplay) return;

            _cameraController.SwitchCamera(VcamType.Gameplay);
            _cameraController.SetFollowTarget(ActiveVehicle.transform);
        }

        private void EndMatch() => _gameManager.ChangeState(GameState.Defeat);

        private void SpawnVehicle()
        {
            ActiveVehicle = _objectPool.Get(vehiclePrefab);
            ActiveVehicle.transform.position = Vector3.zero;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
