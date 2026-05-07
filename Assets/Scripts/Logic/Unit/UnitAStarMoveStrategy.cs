// using System.Collections.Generic;
// using System.Linq;
// using Interfaces;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using Logic.Monster;
//
// namespace Logic.Unit
// {
//     public class UnitAStarMoveStrategy : IMovementStrategy
//     {
//         private readonly UnitModel unit;
//         private readonly MonsterSystem monsterSystem;
//         private readonly Field.Field field;
//         private readonly Tilemap tilemap;
//         private readonly HexAStarPathfinder pathfinder;
//
//         private List<Vector2Int> currentPath;
//         private int pathIndex;
//         private Vector2Int currentTargetHex;
//
//         public UnitAStarMoveStrategy(
//             UnitModel unit,
//             MonsterSystem monsterSystem,
//             Field.Field field,
//             Tilemap tilemap)
//         {
//             this.unit = unit;
//             this.monsterSystem = monsterSystem;
//             this.field = field;
//             this.tilemap = tilemap;
//             pathfinder = new HexAStarPathfinder(field);
//         }
//
//         public void Tick()
//         {
//             var target = monsterSystem.GetAllMonsters()
//                 .Where(m => !m.IsDead)
//                 .OrderBy(m =>
//                     Vector3.Distance(
//                         m.WorldPosition,
//                         unit.WorldPosition))
//                 .FirstOrDefault();
//
//             if (target == null)
//                 return;
//
//             // ✅ если в радиусе атаки — не двигаемся
//             if (Vector3.Distance(target.WorldPosition, unit.WorldPosition) <= unit.UnitData.attackRadius)
//             {
//                 currentPath = null;
//                 return;
//             }
//
//             // ✅ если путь пустой или цель сменила клетку — перестраиваем
//             if (currentPath == null || currentTargetHex != target.CurrentHex)
//             {
//                 currentTargetHex = target.CurrentHex;
//
//                 currentPath = pathfinder.FindPath(unit.CurrentHex, currentTargetHex);
//                 pathIndex = 1;
//
//                 if (currentPath == null || currentPath.Count <= 1)
//                 {
//                     currentPath = null;
//                     return;
//                 }
//             }
//
//             // ✅ если дошли до конца пути
//             if (pathIndex >= currentPath.Count)
//             {
//                 currentPath = null;
//                 return;
//             }
//
//             var nextHex = currentPath[pathIndex];
//             var hexObj = field.GetHex(nextHex);
//
//             if (hexObj == null)
//                 return;
//
//             Vector3 targetWorld = tilemap.GetCellCenterWorld(hexObj.offset);
//             Vector3 currentPosition = unit.WorldPosition;
//             // Vector3 dir = targetWorld - unit.WorldPosition;
//
//             // float speed = unit.GetMoveSpeed();
//             // float step = speed * Core.TickManager.Instance.tickInterval;
//             //
//             // if (dir.magnitude <= step)
//             // {
//             //     unit.WorldPosition = targetWorld;
//             //     unit.SetHex(nextHex);
//             //     pathIndex++;
//             // }
//             // else
//             // {
//             //     unit.WorldPosition += dir.normalized * step;
//             // }
//             
//             float step = unit.GetMoveSpeed() * Core.TickManager.Instance.tickInterval;
//             float distanceToNextNode = Vector3.Distance(currentPosition, targetWorld);
//             if (distanceToNextNode <= step)
//             {
//                 unit.SetPosition(targetWorld);
//                 unit.SetHex(nextHex);
//                 
//                 pathIndex++;
//             }
//             else
//             {
//                 Vector3 direction = (targetWorld - currentPosition).normalized;
//                 unit.SetPosition(currentPosition + direction * step);
//             }
//         }
//     }
// }

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
        private MonsterModel currentTarget;
        private Vector3 formationOffset;
        
        private float repathTimer;
        private const float repathDelay = 0.5f;

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
            formationOffset = new Vector3(
                Random.Range(-0.25f, 0.25f),
                Random.Range(-0.25f, 0.25f),
                0f);
        }

        public void Tick()
        {
            repathTimer -= Core.TickManager.Instance.tickInterval;
            var monsters = monsterSystem.GetAllMonsters()
                .Where(m => !m.IsDead)
                .ToList();

            if (monsters.Count == 0)
            {
                if (currentTarget != null)
                {
                    currentTarget.TargetedByUnits =
                        Mathf.Max(0, currentTarget.TargetedByUnits - 1);
                    currentTarget = null;
                }

                currentPath = null;
                return;
            }

            const float crowdPenalty = 2f;

            var target = monsters
                .OrderBy(m =>
                    Vector3.Distance(m.WorldPosition, unit.WorldPosition) +
                    m.TargetedByUnits * crowdPenalty)
                .First();

            // ✅ если в радиусе атаки — стоим
            if (Vector3.Distance(target.WorldPosition, unit.WorldPosition)
                <= unit.UnitData.attackRadius)
            {
                currentPath = null;
                return;
            }

            // ✅ если цель сменилась — строим новый путь
            if (repathTimer <= 0f &&
                (currentTarget != target ||
                 Vector2Int.Distance(currentTargetHex, target.CurrentHex) > 1))
            {
                // ✅ если цель меняется — уменьшаем счётчик у старой
                if (currentTarget != null && currentTarget != target)
                {
                    currentTarget.TargetedByUnits =
                        Mathf.Max(0, currentTarget.TargetedByUnits - 1);
                }

                // ✅ назначаем новую цель
                if (currentTarget != target)
                {
                    currentTarget = target;
                    currentTarget.TargetedByUnits++;
                }

                BuildNewPath(target);
                repathTimer = repathDelay;
            }

            // ✅ если путь закончился
            if (currentPath == null || pathIndex >= currentPath.Count)
            {
                currentPath = null;
                return;
            }

            MoveAlongPath();
        }

        private void BuildNewPath(MonsterModel target)
        {
            currentTargetHex = GetRandomizedGoal(target.CurrentHex);

            currentPath = pathfinder.FindPath(unit.CurrentHex, currentTargetHex);
            pathIndex = 1;

            if (currentPath == null || currentPath.Count <= 1)
            {
                currentPath = null;
            }
        }

        private void MoveAlongPath()
        {
            var nextHex = currentPath[pathIndex];
            var hexObj = field.GetHex(nextHex);

            if (hexObj == null)
                return;

            var targetWorld = tilemap.GetCellCenterWorld(hexObj.offset) + formationOffset;
            var currentPosition = unit.WorldPosition;

            var step = unit.GetMoveSpeed() * Core.TickManager.Instance.tickInterval;
            var distance = Vector3.Distance(currentPosition, targetWorld);

            if (distance <= step)
            {
                unit.SetPosition(targetWorld);
                unit.SetHex(nextHex);
                pathIndex++;
            }
            else
            {
                var direction = (targetWorld - currentPosition).normalized;
                unit.SetPosition(currentPosition + direction * step);
                unit.CurrentDirection = direction;
            }
        }

        /// <summary>
        /// Возвращает либо саму цель, либо случайную соседнюю клетку.
        /// Это даёт вариативность без дёргания.
        /// </summary>
        private Vector2Int GetRandomizedGoal(Vector2Int center)
        {
            var centerHex = field.GetHex(center);
            if (centerHex == null)
                return center;

            var neighbours = field.GetNeighbours(centerHex)
                                  .Where(h => field.IsWalkable(h))
                                  .ToList();

            if (neighbours.Count == 0)
                return center;

            // 50% шанс идти прямо в цель
            if (Random.value < 0.5f)
                return center;

            return neighbours[Random.Range(0, neighbours.Count)].coordinates;
        }
    }
}