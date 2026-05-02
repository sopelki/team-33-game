using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Unit
{
    public class UnitSystem
    {
        private readonly List<UnitModel> units = new();
        private readonly FreeMovementService movementService;

        public event Action<UnitModel> OnUnitCreated;
        public event Action<UnitModel> OnUnitMoved;
        public event Action<UnitModel> OnUnitDied;

        public UnitSystem(FreeMovementService movementService)
        {
            this.movementService = movementService;
        }

        public void SpawnUnit(Vector3 worldPos, Vector2Int hexPos, UnitData stats)
        {
            var unit = new UnitModel(worldPos, hexPos, stats);
            units.Add(unit);

            OnUnitCreated?.Invoke(unit);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Tick(float deltaTime)
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

                movementService.Tick(unit, deltaTime);
            }
        }
    }
}