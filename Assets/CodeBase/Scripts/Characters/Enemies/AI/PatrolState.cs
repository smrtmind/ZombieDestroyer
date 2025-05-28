using UnityEngine;

namespace CodeBase.Scripts.Characters.Enemies.AI
{
    public class PatrolState : EnemyState
    {
        [SerializeField, Min(1f)] private float patrolRadius = 5f;
        [SerializeField, Min(0.1f)] private float pointReachThreshold = 0.2f;

        private Vector3 _targetPosition;

        public override void Enter() => SetNextTarget();

        public override void Exit() { }

        public override void Tick()
        {
            if (Rb == null) return;

            if (Vector3.Distance(transform.position, _targetPosition) < pointReachThreshold)
                SetNextTarget();

            Vector3 direction = (_targetPosition - Rb.position).normalized;
            Vector3 targetPosition = Rb.position + direction * moveSpeed * Time.deltaTime;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Rb.rotation = Quaternion.Slerp(Rb.rotation, targetRotation, Time.deltaTime * 10f);
            }

            Rb.MovePosition(targetPosition);
        }

        private void SetNextTarget()
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * patrolRadius;
            _targetPosition = transform.position + offset;
        }
    }
}
