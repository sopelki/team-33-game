using UnityEngine;
using Interfaces;

namespace Logic.Monster
{
    public class MonsterModel : IDamageable
    {
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int CurrentHex { get; private set; }
        public bool IsDead => currentHealth <= 0;
        public MonsterData Data => data;

        private int currentHealth;
        private readonly MonsterData data;

        private IMovementStrategy movementStrategy;
        private IAttackStrategy attackStrategy;

        public MonsterModel(
            Vector3 startWorldPos,
            Vector2Int startHex,
            MonsterData data,
            IMovementStrategy movementStrategy,
            IAttackStrategy attackStrategy)
        {
            WorldPosition = startWorldPos;
            CurrentHex = startHex;

            this.data = data;
            this.movementStrategy = movementStrategy;
            this.attackStrategy = attackStrategy;

            currentHealth = data.maxHealth;
        }

        public void Tick()
        {
            if (IsDead)
                return;

            attackStrategy?.Tick();

            if (attackStrategy != null && attackStrategy.IsAttacking)
                return;

            movementStrategy?.Tick();
        }

        public void Move(Vector3 direction)
        {
            var step = Core.TickManager.Instance.tickInterval;
            WorldPosition += direction * (data.moveSpeed * step);
        }

        public void SetHex(Vector2Int hex)
        {
            CurrentHex = hex;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }
        
        public void SetStrategies(
            IMovementStrategy movement,
            IAttackStrategy attack)
        {
            movementStrategy = movement;
            attackStrategy = attack;
        }
    }
}