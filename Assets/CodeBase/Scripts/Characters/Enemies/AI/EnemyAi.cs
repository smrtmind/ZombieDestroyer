using System.Linq;
using UnityEngine;

namespace CodeBase.Scripts.Characters.Enemies.AI
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private EnemyState[] states;

        private IEnemyState _currentState;

        private void Update() => _currentState?.Tick();

        public void SwitchTo<T>() where T : EnemyState
        {
            var newState = GetState<T>();
            if (newState == null || newState == _currentState) 
                return;

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        private T GetState<T>() where T : EnemyState
            => states.FirstOrDefault(s => s is T) as T;
    }
}
