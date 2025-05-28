using System;
using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        [SerializeField] private LayerMask _target;

        public event Action<Collider> OnEnter;
        public event Action<Collider> OnExit;

        private void OnTriggerEnter(Collider other)
        {
            if ((_target & (1 << other.gameObject.layer)) != 0)
                OnEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((_target & (1 << other.gameObject.layer)) != 0)
                OnExit?.Invoke(other);
        }
    }
}
