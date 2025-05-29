using UnityEngine;

namespace CodeBase.Scripts.Characters.Vehicles
{
    public class VehicleMovement : MonoBehaviour
    {
        #region Variables
        [Header("Data")]
        [SerializeField] private VehicleMovementDataStorage dataStorage;

        [Header("Components")]
        [SerializeField] private Rigidbody rb;

        private VehicleMovementData Data => dataStorage.MovementData;

        private bool _canMove;
        private float _nextTurnTime;
        private float _startTurnTime;
        private float _startX;
        private float _targetX;
        private bool _isTurning;
        #endregion

        private void Start()
        {
            Init();
        }

        private void FixedUpdate()
        {
            if (!_canMove) return;

            MoveForward();
            UpdateTurn();
            ApplyTurnRotation();
        }

        private void Init()
        {
            _nextTurnTime = Time.time + Data.DelayBetweenTurns;
            _targetX = transform.position.x;
        }

        private void MoveForward()
        {
            rb.AddForce(transform.forward * Data.Acceleration, ForceMode.Acceleration);

            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVelocity.magnitude > Data.MaxSpeed)
            {
                Vector3 clampedVelocity = horizontalVelocity.normalized * Data.MaxSpeed;
                rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
            }
        }

        private void UpdateTurn()
        {
            if (Time.time >= _nextTurnTime && !_isTurning)
                StartTurn();

            if (_isTurning)
            {
                float t = (Time.time - _startTurnTime) / Data.TurnDuration;
                if (t >= 1f)
                {
                    t = 1f;
                    _isTurning = false;
                    _nextTurnTime = Time.time + Data.DelayBetweenTurns;
                }

                float newX = Mathf.Lerp(_startX, _targetX, t);
                newX = Mathf.Clamp(newX, -Data.RangeX, Data.RangeX);

                Vector3 newPos = new Vector3(newX, rb.position.y, rb.position.z);
                rb.MovePosition(newPos);
            }
        }

        private void StartTurn()
        {
            _startTurnTime = Time.time;
            _startX = rb.position.x;

            float minTargetX = Mathf.Max(-Data.MaxAllowedXShift, _startX - Data.RangeX);
            float maxTargetX = Mathf.Min(Data.MaxAllowedXShift, _startX + Data.RangeX);

            do
            {
                _targetX = Random.Range(minTargetX, maxTargetX);
            }
            while (Mathf.Abs(_targetX - _startX) < 0.2f);// To prevent too small turns

            _isTurning = true;
        }

        private void ApplyTurnRotation()
        {
            float targetYAngle = 0f;

            if (_isTurning)
            {
                float t = (Time.time - _startTurnTime) / Data.TurnDuration;
                t = Mathf.Clamp01(t);

                float easing = Mathf.Sin(t * Mathf.PI);

                float turnDirection = Mathf.Sign(_targetX - _startX);
                targetYAngle = turnDirection * easing * Data.MaxTurnAngle;
            }

            float currentY = transform.eulerAngles.y;
            float newY = Mathf.LerpAngle(currentY, targetYAngle, Data.RotationReturnSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0f, newY, 0f);
        }

        public void StartMovement()
        {
            if (_canMove) return;

            _canMove = true;
        }

        public void StopMovement()
        {
            if (!_canMove) return;

            _canMove = false;
            rb.velocity = Vector3.zero;
        }
    }
}
