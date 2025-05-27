using System;
using UnityEngine;

namespace Unavinar.Pooling
{
    public class PoolableMonoBehaviour : MonoBehaviour, IPoolable
    {
        public GameObject GameObject => gameObject;

        public event Action<IPoolable> Destroyed;

        public void Release()
        {
            if (this != null && gameObject != null)
                Destroyed?.Invoke(this);
        }
    }
}
