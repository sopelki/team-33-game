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
        
        private float patrolWaitTimer;
        private bool isPatrolling;
        private Vector2Int spawnHex = new(-12, 9);

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
            var dt = Core.TickManager.Instance.tickInterval;
            repathTimer -= dt;
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

                isPatrolling = true;
                HandleIdlePatrol(dt);
                return;
            }
            
            if (isPatrolling)
            {
                isPatrolling = false;
                currentPath = null;
            }

            const float crowdPenalty = 2f;

            var target = monsters
                .OrderBy(m =>
                    Vector3.Distance(m.WorldPosition, unit.WorldPosition) +
                    m.TargetedByUnits * crowdPenalty)
                .First();
            
            if (Vector3.Distance(target.WorldPosition, unit.WorldPosition)
                <= unit.UnitData.attackRadius)
            {
                currentPath = null;
                return;
            }
            if (repathTimer <= 0f &&
                (currentTarget != target ||
                 Vector2Int.Distance(currentTargetHex, target.CurrentHex) > 1))
            {
                if (currentTarget != null && currentTarget != target)
                {
                    currentTarget.TargetedByUnits =
                        Mathf.Max(0, currentTarget.TargetedByUnits - 1);
                }
                if (currentTarget != target)
                {
                    currentTarget = target;
                    currentTarget.TargetedByUnits++;
                }

                BuildNewPath(target);
                repathTimer = repathDelay;
            }
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
                currentPath = null;
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
            
            if (Random.value < 0.5f)
                return center;

            return neighbours[Random.Range(0, neighbours.Count)].coordinates;
        }
        
        private void HandleIdlePatrol(float dt)
        {
            if (currentPath != null && pathIndex < currentPath.Count)
            {
                MoveAlongPath();
                return;
            }
            
            var distanceToRallyPoint = Vector2Int.Distance(unit.CurrentHex, spawnHex);
            if (distanceToRallyPoint > 10f)
            {
                currentPath = null;
                unit.CurrentDirection = Vector3.zero;
                return;
            }
            
            patrolWaitTimer -= dt;
            if (patrolWaitTimer > 0f)
                return;
            
            var radius = 2; 
            var randomX = Random.Range(-radius, radius + 1);
            var randomY = Random.Range(-radius, radius + 1);
            var potentialHex = spawnHex + new Vector2Int(randomX, randomY);

            var hexObj = field.GetHex(potentialHex);
            
            if (hexObj != null && field.IsWalkable(hexObj))
            {
                currentPath = pathfinder.FindPath(unit.CurrentHex, potentialHex);
                pathIndex = 1;

                if (currentPath == null || currentPath.Count <= 1)
                {
                    currentPath = null;
                }
                
                
                patrolWaitTimer = Random.Range(2f, 5f); 
            }
        }
    }
}