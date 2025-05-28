using CodeBase.Scripts.Detectors;
using UnityEngine;

namespace CodeBase.Scripts.Characters.Enemies.Animations
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private Detector detector;

        [Header("Triggers")]
        [SerializeField] private string walkTrigger = "IsWalking";

        private int _walkStateHash;

        private void Awake()
        {
            _walkStateHash = Animator.StringToHash(walkTrigger);
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            detector.OnTargetDetected += Run;
            detector.OnTargetLost += Walk;
        }

        private void Unsubscribe()
        {
            detector.OnTargetDetected -= Run;
            detector.OnTargetLost -= Walk;
        }

        private void Run() => animator.SetBool(_walkStateHash, false);

        private void Walk() => animator.SetBool(_walkStateHash, true);

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
