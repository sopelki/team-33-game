using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;
using Logic.Monster;

namespace Logic.Unit
{
    public class UnitAStarMoveStrategy : IMovementStrategy
    {
        private readonly UnitModel unit;
        private readonly MonsterSystem monsterSystem;
        private readonly Field.Field field;
        private readonly Tilemap tilemap;
        private readonly HexAStarPathfinder pathfinder;

        private List<Vector2Int> currentPath;
        private int pathIndex;
        private Vector2Int currentTargetHex;

        public UnitAStarMoveStrategy(
            UnitModel unit,
            MonsterSystem monsterSystem,
            Field.Field field,
            Tilemap tilemap)
        {
            this.unit = unit;
            this.monsterSystem = monsterSystem;
            this.field = field;
            this.tilemap = tilemap;
            pathfinder = new HexAStarPathfinder(field);
        }

        public void Tick()
        {
            var target = monsterSystem.GetAllMonsters()
                .Where(m => !m.IsDead)
                .OrderBy(m =>
                    Vector3.Distance(
                        m.WorldPosition,
                        unit.WorldPosition))
                .FirstOrDefault();

            if (target == null)
                return;

            // ✅ если в радиусе атаки — не двигаемся
            if (Vector3.Distance(target.WorldPosition, unit.WorldPosition) <= unit.UnitData.attackRadius)
            {
                currentPath = null;
                return;
            }

            // ✅ если путь пустой или цель сменила клетку — перестраиваем
            if (currentPath == null || currentTargetHex != target.CurrentHex)
            {
                currentTargetHex = target.CurrentHex;

                currentPath = pathfinder.FindPath(unit.CurrentHex, currentTargetHex);
                pathIndex = 1;

                if (currentPath == null || currentPath.Count <= 1)
                {
                    currentPath = null;
                    return;
                }
            }

            // ✅ если дошли до конца пути
            if (pathIndex >= currentPath.Count)
            {
                currentPath = null;
                return;
            }

            var nextHex = currentPath[pathIndex];
            var hexObj = field.GetHex(nextHex);

            if (hexObj == null)
                return;

            Vector3 targetWorld = tilemap.GetCellCenterWorld(hexObj.offset);
            Vector3 currentPosition = unit.WorldPosition;
            // Vector3 dir = targetWorld - unit.WorldPosition;

            // float speed = unit.GetMoveSpeed();
            // float step = speed * Core.TickManager.Instance.tickInterval;
            //
            // if (dir.magnitude <= step)
            // {
            //     unit.WorldPosition = targetWorld;
            //     unit.SetHex(nextHex);
            //     pathIndex++;
            // }
            // else
            // {
            //     unit.WorldPosition += dir.normalized * step;
            // }
            
            float step = unit.GetMoveSpeed() * Core.TickManager.Instance.tickInterval;
            float distanceToNextNode = Vector3.Distance(currentPosition, targetWorld);
            if (distanceToNextNode <= step)
            {
                unit.SetPosition(targetWorld);
                unit.SetHex(nextHex);
                
                pathIndex++;
            }
            else
            {
                Vector3 direction = (targetWorld - currentPosition).normalized;
                unit.SetPosition(currentPosition + direction * step);
            }
        }
    }
}