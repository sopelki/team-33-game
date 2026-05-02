using System.Collections.Generic;
using UnityEngine;

namespace Logic.Unit
{
    public class UnitModel
    {
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int CurrentHex { get; private set; }
        public Vector3 CurrentDirection { get; set; }
        public float DirectionTimer { get; set; }

        private readonly UnitData baseStats;
        public UnitData Stats => baseStats;
        private readonly List<Buff> buffs = new();

        private int CurrentHealth { get; set; }
        public bool IsDead => CurrentHealth <= 0;

        public UnitModel(Vector3 startPos, Vector2Int startHex, UnitData stats)
        {
            WorldPosition = startPos;
            CurrentHex = startHex;
            baseStats = stats;
            CurrentHealth = stats.maxHealth;
        }
        
        public float GetMoveSpeed()
        {
            var value = baseStats.moveSpeed;

            foreach (var buff in buffs)
                value = buff.ModifyMoveSpeed(value);

            return value;
        }

        public int GetAttack()
        {
            var value = baseStats.attack;
            foreach (var buff in buffs)
                value = buff.ModifyAttack(value);

            return value;
        }

        public void AddBuff(Buff buff)
        {
            buffs.Add(buff);
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }

        public void Move(Vector3 direction, float deltaTime)
        {
            WorldPosition += direction * GetMoveSpeed() * deltaTime;
        }

        public void SetHex(Vector2Int hex)
        {
            CurrentHex = hex;
        }
    }
}