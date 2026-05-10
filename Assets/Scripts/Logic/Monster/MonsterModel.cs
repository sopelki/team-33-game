using UnityEngine;
using Interfaces;

namespace Logic.Monster
{
    public class MonsterModel : IDamageable
    {
        public int TargetedByUnits;

        public Vector3 WorldPosition { get; private set; }
        public Vector2Int CurrentHex { get; private set; }
        public Vector3 CurrentVelocity { get; private set; }

        public bool IsDead => currentHealth <= 0;

        public readonly MonsterData Data;

        public int MaxHealth { get; }
        public int Damage { get; }
        public float MoveSpeed { get; }
        public float AttackRadius { get; }
        public float AttackCooldown { get; }
        public int GoldReward { get; }

        private int currentHealth;

        private IMovementStrategy movementStrategy;
        private IAttackStrategy attackStrategy;

        public MonsterModel(
            Vector3 startWorldPos,
            Vector2Int startHex,
            MonsterData data,
            float healthMultiplier,
            float damageMultiplier,
            float speedMultiplier)
        {
            Data = data;

            WorldPosition = startWorldPos;
            CurrentHex = startHex;
            Data = data;

            MaxHealth = Mathf.RoundToInt(data.maxHealth * healthMultiplier);
            Damage = Mathf.RoundToInt(data.damage * damageMultiplier);
            MoveSpeed = data.moveSpeed * speedMultiplier;

            AttackRadius = data.attackRadius;
            AttackCooldown = data.attackCooldown;
            GoldReward = data.goldReward;

            currentHealth = MaxHealth;
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

        public void Move(Vector3 direction)
        {
            var step = Core.TickManager.Instance.tickInterval;

            WorldPosition += direction * (MoveSpeed * step);
            CurrentVelocity = direction * MoveSpeed;
        }

        public void SetHex(Vector2Int hex) => CurrentHex = hex;

        public void TakeDamage(int damage) => currentHealth -= damage;

        public void SetStrategies(IMovementStrategy movement, IAttackStrategy attack)
        {
            movementStrategy = movement;
            attackStrategy = attack;
        }
    }
}