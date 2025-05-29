using CodeBase.Scripts.Service;
using UnityEngine;

namespace CodeBase.Scripts.Detectors
{
    public class RangeAttackControllerAuto : RangeAttackController
    {
        [SerializeField] private TouchController touchController;

        [Header("Parameters")]
        [SerializeField, Min(0.05f)] private float shootDelay = 0.33f;

        private float _baseShootDelay;
        private float _currentShootDelay;
        private bool _canShoot;

        protected override void OnEnable()
        {
            base.OnEnable();

            _baseShootDelay = 0f;
            _currentShootDelay = shootDelay;
        }

        protected override void Subscribe()
        {
            touchController.OnTouched += StartShoot;
            touchController.OnReleased += StopShoot;
        }

        protected override void Unsubscribe()
        {
            touchController.OnTouched -= StartShoot;
            touchController.OnReleased -= StopShoot;
        }

        private void Update()
        {
            if (!_canShoot) return;

            if (_baseShootDelay > 0f)
            {
                _baseShootDelay -= Time.deltaTime;
            }
            else
            {
                weapon.Shoot(touchController.TargetPosition, damageProvider.Damage);
                _baseShootDelay = _currentShootDelay;
            }
        }

        private void StartShoot()
        {
            if (_canShoot) return;

            _canShoot = true;
        }

        public void StopShoot()
        {
            if (!_canShoot) return;

            _canShoot = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopShoot();
        }
    }
}
