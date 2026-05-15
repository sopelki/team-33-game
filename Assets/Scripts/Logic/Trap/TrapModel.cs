using System.Collections.Generic;
using UnityEngine;
using Logic.Monster;
using Logic.Trap.Logic.Trap;

namespace Logic.Trap
{
    public class TrapModel
    {
        public TrapData Data { get; }
        public List<Vector2Int> Hexes { get; }

        public bool IsTriggered { get; private set; }

        // Для DamageZone
        public float TickTimer;

        // Для SlowZone — храним конкретные дебаффы
        public readonly Dictionary<MonsterModel, SlowDebuff> ActiveSlowDebuffs = new();

        public TrapModel(TrapData data, List<Vector2Int> hexes)
        {
            Data = data;
            Hexes = hexes;
        }

        public void Trigger()
        {
            IsTriggered = true;
        }
    }
}