using UnityEngine;
using Interfaces;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Logic.Unit;

namespace Logic.Monster
{
    public class HexMoveToTargetStrategy : IMovementStrategy
    {
        private readonly MonsterModel monster;
        private readonly Field.Field field;
        private readonly Vector2Int targetHex;
        private readonly Tilemap tilemap;
        private readonly HexAStarPathfinder pathfinder;

        private List<Vector2Int> currentPath;
        private int pathIndex;

        private float repathTimer;
        private const float repathDelay = 0.7f;

        private readonly Vector3 formationOffset;

        public HexMoveToTargetStrategy(
            MonsterModel monster,
            Field.Field field,
            Vector2Int targetHex,
            Tilemap tilemap)
        {
            this.monster = monster;
            this.field = field;
            this.targetHex = targetHex;
            this.tilemap = tilemap;

            pathfinder = new HexAStarPathfinder(field);

            formationOffset = new Vector3(
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.2f, 0.2f),
                0f);
        }

        public void Tick()
        {
            if (monster.CurrentHex == targetHex)
                return;

            repathTimer -= Core.TickManager.Instance.tickInterval;

            if (currentPath == null || pathIndex >= currentPath.Count || repathTimer <= 0f)
            {
                BuildPath();
                repathTimer = repathDelay;
            }

            if (currentPath == null || pathIndex >= currentPath.Count)
                return;

            MoveAlongPath();
        }

        private void BuildPath()
        {
            // ✅ цель — случайная клетка рядом с замком
            Vector2Int goal = GetRandomizedGoal(targetHex);

            currentPath = pathfinder.FindPath(monster.CurrentHex, goal);
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

            Vector3 targetWorld =
                tilemap.GetCellCenterWorld(hexObj.offset) + formationOffset;

            Vector3 dir = targetWorld - monster.WorldPosition;
            float distance = dir.magnitude;

            float step = monster.Data.moveSpeed *
                         Core.TickManager.Instance.tickInterval;

            if (distance <= step)
            {
                monster.SetHex(nextHex);
                monster.SetPosition(targetWorld);
                pathIndex++;
            }
            else
            {
                monster.SetPosition(
                    monster.WorldPosition + dir.normalized * step);
            }
        }

        private Vector2Int GetRandomizedGoal(Vector2Int center)
        {
            var centerHex = field.GetHex(center);
            if (centerHex == null)
                return center;

            var neighbours = field.GetNeighbours(centerHex);

            var walkable = new List<Vector2Int>();

            foreach (var n in neighbours)
            {
                if (field.IsWalkable(n))
                    walkable.Add(n.coordinates);
            }

            if (walkable.Count == 0)
                return center;

            return walkable[Random.Range(0, walkable.Count)];
        }
    }
}