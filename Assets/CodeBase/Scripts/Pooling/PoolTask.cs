using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Unavinar.Pooling
{
    public class PoolTask
    {
        private readonly Queue<IPoolable> _freeObjects;
        private readonly List<IPoolable> _objectsInUse;
        private readonly Transform _container;
        private readonly DiContainer _diContainer;

        [Inject]
        public PoolTask(Transform container, DiContainer diContainer)
        {
            _container = container;
            _diContainer = diContainer;
            _freeObjects = new Queue<IPoolable>();
            _objectsInUse = new List<IPoolable>();
        }

        public T GetFreeObject<T>(T prefab) where T : Component, IPoolable
        {
            T poolable;
            if (_freeObjects.Count > 0)
            {
                poolable = (T)_freeObjects.Dequeue();
            }
            else
            {
                poolable = _diContainer.InstantiatePrefabForComponent<T>(prefab, _container);
            }

            poolable.Destroyed += ReturnToPool;
            poolable.GameObject.SetActive(true);

            _objectsInUse.Add(poolable);

            return poolable;
        }

        private void ReturnToPool(IPoolable poolable)
        {
            if (_objectsInUse.Remove(poolable))
            {
                poolable.Destroyed -= ReturnToPool;

                poolable.GameObject.SetActive(false);
                poolable.GameObject.transform.SetParent(_container);

                _freeObjects.Enqueue(poolable);
            }
        }

        private void ReturnAllObjectsToPool()
        {
            for (int i = _objectsInUse.Count - 1; i >= 0; i--)
                _objectsInUse[i].Release();

            _objectsInUse.Clear();
        }

        private void DestroyAllObjects()
        {
            while (_freeObjects.Count > 0)
            {
                var poolable = _freeObjects.Dequeue();

                if (poolable.GameObject.activeSelf)
                    poolable.GameObject.SetActive(false);

                Object.Destroy(poolable.GameObject);
            }

            _freeObjects.Clear();
        }

        public void Dispose()
        {
            ReturnAllObjectsToPool();
            DestroyAllObjects();
        }
    }
}
