using UnityEngine;

namespace CodeBase.Scripts.Characters.Enemies.AI
{
    public abstract class EnemyState : MonoBehaviour, IEnemyState
    {
        [field: Header("Base Settings")]
        [field: SerializeField] public Rigidbody Rb { get; private set; }
        [SerializeField] protected float moveSpeed = 3f;

        public abstract void Enter();

        public abstract void Exit();

        public abstract void Tick();
    }
}
