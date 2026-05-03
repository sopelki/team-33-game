using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logic.Unit
{
    public class UnitModel
    {
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int CurrentHex { get; private set; }
        public Vector3 CurrentDirection { get; set; }
        public float DirectionTimer { get; set; }

        public UnitData UnitData { get; }
        private readonly List<Buff> buffs = new();

        private int CurrentHealth { get; set; }
        public bool IsDead => CurrentHealth <= 0;

        public UnitModel(Vector3 startPos, Vector2Int startHex, UnitData unitData)
        {
            WorldPosition = startPos;
            CurrentHex = startHex;
            UnitData = unitData;
            CurrentHealth = unitData.maxHealth;
        }

        public float GetMoveSpeed() =>
            buffs.Aggregate(UnitData.moveSpeed, (current, buff) => buff.ModifyMoveSpeed(current));

        public int GetAttack() => buffs.Aggregate(UnitData.attack, (current, buff) => buff.ModifyAttack(current));

        public void AddBuff(Buff buff) => buffs.Add(buff);

        public void TakeDamage(int damage) => CurrentHealth -= damage;

        public void Move(Vector3 direction, float step) => WorldPosition += direction * GetMoveSpeed() * step;

        public void SetHex(Vector2Int hex) => CurrentHex = hex;
    }
}