using UnityEngine;
using Interfaces;
using Field;
using HexagonScripts;
using UnityEngine.Tilemaps;

namespace Logic.Monster
{
    public class HexMoveToTargetStrategy : IMovementStrategy
{
    private readonly MonsterModel monster;
    private readonly Field.Field field;
    private readonly Vector2Int targetHex;
    private readonly Tilemap tilemap;

    private Vector2Int? nextHex;
    private Vector3 targetWorld;

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
    }

    public void Tick()
    {
        if (monster.CurrentHex == targetHex)
            return;
        
        if (nextHex == null)
        {
            var currentHexObj = field.GetHex(monster.CurrentHex);
            if (currentHexObj == null)
                return;

            var neighbors = field.GetNeighbours(currentHexObj);

            Hexagon best = null;
            var bestDist = int.MaxValue;

            foreach (var n in neighbors)
            {
                if (!field.IsWalkable(n))
                    continue;

                var dist = HexagonMath.Distance(
                    n.coordinates,
                    targetHex
                );

                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = n;
                }
            }

            if (best == null)
                return;

            nextHex = best.coordinates;

            var hexObj = field.GetHex(best.coordinates);
            targetWorld = tilemap.GetCellCenterWorld(hexObj.offset);
        }
        
        var dir = (targetWorld - monster.WorldPosition);
        var distance = dir.magnitude;

        var step = Core.TickManager.Instance.tickInterval * monster.Data.moveSpeed;

        if (distance <= step)
        {
            monster.SetHex(nextHex.Value);
            monster.Move(dir.normalized * (distance / step));
            nextHex = null;
        }
        else
        {
            monster.Move(dir.normalized);
        }
    }
}
}