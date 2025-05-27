using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Unavinar.Pooling
{
    public class ObjectPool : MonoBehaviour
    {
        private readonly Dictionary<Component, PoolTask> _activePoolTasks = new();

        [Inject] private DiContainer _diContainer;

        public T Get<T>(T prefab) where T : Component, IPoolable
        {
            if (!_activePoolTasks.TryGetValue(prefab, out var poolTask))
                AddTaskToPool(prefab, out poolTask);

            return poolTask.GetFreeObject(prefab);
        }

        private void AddTaskToPool<T>(T prefab, out PoolTask poolTask) where T : Component, IPoolable
        {
            var taskContainer = new GameObject($"{prefab.name}_pool")
            {
                transform = { parent = transform }
            };

            poolTask = _diContainer.Instantiate<PoolTask>(new object[] { taskContainer.transform });
            _activePoolTasks.Add(prefab, poolTask);
        }

        private void DisposeAllTasks()
        {
            foreach (var poolTask in _activePoolTasks.Values)
                poolTask.Dispose();
        }

        private void OnDisable()
        {
            DisposeAllTasks();
        }
    }
}
