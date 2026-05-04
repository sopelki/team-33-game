using System;
using System.Collections.Generic;
using Logic.Monster;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Logic.Unit
{
    public class UnitSystem
    {
        private readonly List<UnitModel> units = new();
        private readonly MonsterSystem monsterSystem;
        private readonly Field.Field field;
        private readonly Tilemap tilemap;

        public event Action<UnitModel> OnUnitCreated;
        // public event Action<UnitModel> OnUnitMoved;
        public event Action<UnitModel> OnUnitDied;

        public UnitSystem(
            MonsterSystem monsterSystem,
            Field.Field field,
            Tilemap tilemap)
        {
            this.monsterSystem = monsterSystem;
            this.field = field;
            this.tilemap = tilemap;
        }

        public void SpawnUnit(Vector3 worldPos, Vector2Int hexPos, UnitData stats)
        {
            var unit = new UnitModel(worldPos, hexPos, stats);
            
            var attack = new UnitAttackStrategy(unit, monsterSystem);
            var movement = new UnitAStarMoveStrategy(
                unit,
                monsterSystem,
                field,
                tilemap
            );

            unit.SetStrategies(movement, attack);
            units.Add(unit);

            OnUnitCreated?.Invoke(unit);
        }

        // TODO: Переделать в foreach
        public void Tick()
        {
            for (var i = units.Count - 1; i >= 0; i--)
            {
                var unit = units[i];

                if (unit.IsDead)
                {
                    units.RemoveAt(i);
                    OnUnitDied?.Invoke(unit);
                    continue;
                }

                unit.Tick();
            }
        }
        
        public IReadOnlyList<UnitModel> GetAllUnits()
        {
            return units;
        }
    }
}