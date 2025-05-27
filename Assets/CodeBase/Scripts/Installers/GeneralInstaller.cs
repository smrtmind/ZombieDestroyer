using CodeBase.Scripts.CameraLogic;
using CodeBase.Scripts.Managers;
using UnityEngine;

namespace CodeBase.Scripts.Installers
{
    public class GeneralInstaller : BaseInstaller
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private GameManager gameManager;

        protected override void BindInstances()
        {
            BindInstanceAsSingle(cameraController);
            BindInstanceAsSingle(gameManager);
        }
    }
}
