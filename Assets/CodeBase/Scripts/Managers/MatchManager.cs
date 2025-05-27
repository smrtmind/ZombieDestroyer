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
        private CameraController _cameraController;

        [Inject]
        private void Construct(ObjectPool objectPool, CameraController cameraController)
        {
            _objectPool = objectPool;
            _cameraController = cameraController;
        }

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChangedHandler;
        }

        private void OnDisable()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChangedHandler;
        }

        private void OnAfterStateChangedHandler(GameState state)
        {
            switch (state)
            {
                case GameState.Loading:
                    SpawnVehicle();
                    break;

                case GameState.Gameplay:
                    _cameraController.SwitchCamera(VcamType.Gameplay);
                    _cameraController.SetFollowTarget(ActiveVehicle.transform);
                    break;
            }
        }

        private void SpawnVehicle()
        {
            ActiveVehicle = _objectPool.Get(vehiclePrefab);
            ActiveVehicle.transform.position = Vector3.zero;
        }
    }
}
