using CodeBase.Scripts.CameraLogic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CodeBase.Scripts.Service
{
    public class TouchController : MonoBehaviour
    {
        public Vector3 TargetPosition { get; private set; }

        private Camera _mainCamera;
        private EventSystem _eventSystem;

        public event Action OnTouched;
        public event Action OnReleased;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            _mainCamera = cameraController.MainCamera;
        }

        private void OnEnable()
        {
            _eventSystem = EventSystem.current;
        }

        private void Update()
        {
#if UNITY_EDITOR
            CheckMouseInput();
#else
            if (Input.touchCount > 0)
                CheckTouchInput();
#endif
        }

        private void CheckTouchInput()
        {
            var touch = Input.GetTouch(0);

            if (_eventSystem.IsPointerOverGameObject(touch.fingerId)) return;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouched?.Invoke();
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    TargetPosition = GetTargetPosition(touch.position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnReleased?.Invoke();
                    break;
            }
        }

        private void CheckMouseInput()
        {
            if (_eventSystem.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0))
            {
                OnTouched?.Invoke();
            }
            else if (Input.GetMouseButton(0))
            {
                TargetPosition = GetTargetPosition(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnReleased?.Invoke();
            }
        }

        private Vector3 GetTargetPosition(Vector3 inputVector)
        {
            var screenPosition = inputVector;
            screenPosition.z = Mathf.Abs(_mainCamera.transform.position.z - transform.position.z);

            var worldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
            worldPosition.y = transform.position.y;

            return worldPosition;
        }
    }
}
