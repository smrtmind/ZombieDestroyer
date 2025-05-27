using Unavinar.Pooling;
using UnityEngine;

namespace CodeBase.Scripts.Environment
{
    public class Ground : PoolableMonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;

        public Vector3 Size => boxCollider.size;
    }
}
