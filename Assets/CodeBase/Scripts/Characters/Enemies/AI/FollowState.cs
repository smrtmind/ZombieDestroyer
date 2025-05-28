using CodeBase.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace CodeBase.Scripts.Characters.Enemies.AI
{
    public class FollowState : EnemyState
    {
        private Transform _vehicleTransform;

        [Inject]
        private void Construct(MatchManager matchManager)
        {
            _vehicleTransform = matchManager.ActiveVehicle.DamageableObject.transform;
        }

        public override void Enter() { }

        public override void Exit() { }

        public override void Tick()
        {
            if (Rb == null) return;

            Vector3 direction = (_vehicleTransform.position - Rb.position).normalized;
            Vector3 targetPosition = Rb.position + direction * moveSpeed * Time.deltaTime;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Rb.rotation = Quaternion.Slerp(Rb.rotation, targetRotation, Time.deltaTime * 10f);
            }

            Rb.MovePosition(targetPosition);
        }
    }
}
