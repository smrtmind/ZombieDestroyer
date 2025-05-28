using System;
using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnExit;

        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }
    }
}
