using UnityEngine;

namespace CodeBase.Scripts.Characters.Enemies.AI
{
    public interface IEnemyState
    {
        public Rigidbody Rb { get; }

        void Enter();

        void Exit();

        void Tick();
    }
}
