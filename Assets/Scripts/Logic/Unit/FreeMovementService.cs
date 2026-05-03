using Core;
using HexagonScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Logic.Unit
{
    // TODO: Переделать во что-то адекватное
    public class FreeMovementService
    {
        private readonly Field.Field field;
        private readonly Tilemap tilemap;

        // private Vector3 currentDirection;
        // private float directionTimer;

        public FreeMovementService(Field.Field field, Tilemap tilemap)
        {
            this.field = field;
            this.tilemap = tilemap;
        }

        public void Tick(UnitModel unit)
        {
            var step = TickManager.Instance.tickInterval;
            unit.DirectionTimer -= step;

            if (unit.DirectionTimer <= 0f)
            {
                unit.CurrentDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    0f
                ).normalized;

                unit.DirectionTimer = 2f;
            }

            var newPos = unit.WorldPosition +
                         unit.CurrentDirection * unit.GetMoveSpeed() * step;

            var cell = tilemap.WorldToCell(newPos);
            var axial = HexagonMath.OffsetToAxial(cell.x, cell.y);

            var hex = field.GetHex(axial);

            if (hex != null && field.IsWalkable(hex))
            {
                unit.Move(unit.CurrentDirection, step);
                unit.SetHex(axial);
            }
            else
            {
                unit.DirectionTimer = 0f; // сменить направление
            }
        }
    }
}