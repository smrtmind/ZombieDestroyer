using CodeBase.Scripts.CameraLogic;
using CodeBase.Scripts.Managers;
using CodeBase.Scripts.Spawners;
using Unavinar.Pooling;
using UnityEngine;

namespace CodeBase.Scripts.Installers
{
    public class GeneralInstaller : BaseInstaller
    {
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private GroundFactory GroundFactory;
        [SerializeField] private CameraController cameraController;

        protected override void BindInstances()
        {
            BindInstanceAsSingle(objectPool);
            BindInstanceAsSingle(gameManager);
            BindInstanceAsSingle(matchManager);
            BindInstanceAsSingle(GroundFactory);
            BindInstanceAsSingle(cameraController);
        }
    }
}
