using UnityEngine;
using Interfaces;

namespace Logic.Monster
{
    public class MonsterModel : IDamageable
    {
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int CurrentHex { get; private set; }
        public Vector3 CurrentVelocity { get; private set; }
        public bool IsDead => currentHealth <= 0;
        public MonsterData Data { get; }

        private int currentHealth;
        public int TargetedByUnits;

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

            Data = data;
            this.movementStrategy = movementStrategy;
            this.attackStrategy = attackStrategy;

            currentHealth = data.maxHealth;
        }

        public void Tick()
        {
            if (IsDead)
                return;

            attackStrategy?.Tick();

            if (attackStrategy?.IsAttacking == true)
                return;

            movementStrategy?.Tick();
        }

        // public void Move(Vector3 direction)
        // {
        //     var step = Core.TickManager.Instance.tickInterval;
        //     WorldPosition += direction * (Data.moveSpeed * step);
        //     CurrentVelocity = direction * Data.moveSpeed;
        // }
        
        public void SetPosition(Vector3 newPosition)
        {
            this.WorldPosition = newPosition;
        }

        public void SetHex(Vector2Int hex)
        {
            CurrentHex = hex;
        }

        public void TakeDamage(int damage)
        {
            Debug.Log($"Monster got damage: {damage}");
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