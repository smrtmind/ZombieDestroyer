using System;
using UnityEngine;

namespace Unavinar.Pooling
{
    public interface IPoolable
    {
        public GameObject GameObject { get; }

        public event Action<IPoolable> Destroyed;

        public void Release();
    }
}
