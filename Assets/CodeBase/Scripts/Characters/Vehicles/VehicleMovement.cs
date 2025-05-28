using UnityEngine;

namespace CodeBase.Scripts.Characters.Vehicles
{
    public class VehicleMovement : MonoBehaviour
    {
        #region Variables
        [Header("Forward Movement")]
        [SerializeField] private Rigidbody rb;
        [SerializeField, Min(1f)] private float acceleration = 15f;
        [SerializeField, Min(1f)] private float maxSpeed = 20f;

        [Header("Turn Settings")]
        [SerializeField, Min(1f)] private float delayBetweenTurns = 2f;
        [SerializeField, Min(1f)] private float xRange = 3f;
        [SerializeField, Min(0.1f)] private float turnDuration = 0.6f;
        [SerializeField, Min(2f)] private float maxAllowedXShift = 4f;

        [Header("Rotation Settings")]
        [SerializeField] private float maxTurnAngle = 25f;
        [SerializeField, Min(0.1f)] private float rotationReturnSpeed = 5f;

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
            MoveForward();
            UpdateTurn();
            ApplyTurnRotation();
        }

        private void Init()
        {
            _nextTurnTime = Time.time + delayBetweenTurns;
            _targetX = transform.position.x;
        }

        private void MoveForward()
        {
            rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);

            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVelocity.magnitude > maxSpeed)
            {
                Vector3 clampedVelocity = horizontalVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
            }
        }

        private void UpdateTurn()
        {
            if (Time.time >= _nextTurnTime && !_isTurning)
                StartTurn();

            if (_isTurning)
            {
                float t = (Time.time - _startTurnTime) / turnDuration;

                if (t >= 1f)
                {
                    t = 1f;
                    _isTurning = false;
                    _nextTurnTime = Time.time + delayBetweenTurns;
                }

                float newX = Mathf.Lerp(_startX, _targetX, t);
                newX = Mathf.Clamp(newX, -xRange, xRange);

                Vector3 newPos = new Vector3(newX, rb.position.y, rb.position.z);
                rb.MovePosition(newPos);
            }
        }

        private void StartTurn()
        {
            _startTurnTime = Time.time;
            _startX = rb.position.x;

            float minTargetX = Mathf.Max(-maxAllowedXShift, _startX - xRange);
            float maxTargetX = Mathf.Min(maxAllowedXShift, _startX + xRange);

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
                float t = (Time.time - _startTurnTime) / turnDuration;
                t = Mathf.Clamp01(t);

                float easing = Mathf.Sin(t * Mathf.PI);

                float turnDirection = Mathf.Sign(_targetX - _startX);
                targetYAngle = turnDirection * easing * maxTurnAngle;
            }

            float currentY = transform.eulerAngles.y;
            float newY = Mathf.LerpAngle(currentY, targetYAngle, rotationReturnSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0f, newY, 0f);
        }
    }
}
