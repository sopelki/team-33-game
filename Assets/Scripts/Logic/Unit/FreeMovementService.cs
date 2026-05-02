using HexagonScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Logic.Unit
{
    public class FreeMovementService
    {
        private readonly Field field;
        private readonly Tilemap tilemap;

        public FreeMovementService(Field field, Tilemap tilemap)
        {
            this.field = field;
            this.tilemap = tilemap;
        }

        public void Tick(UnitModel unit, float deltaTime)
        {
            var randomDir = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ).normalized;

            var newPos = unit.WorldPosition +
                         randomDir * unit.GetMoveSpeed() * deltaTime;

            Vector3Int cell = tilemap.WorldToCell(newPos);
            Vector2Int axial = HexagonMath.OffsetToAxial(cell.x, cell.y);

            var hex = field.GetHex(axial);

            Debug.Log($"Trying move to hex type: {hex?.type}");
            if (hex != null && field.IsWalkable(hex))
            {
                unit.Move(randomDir, deltaTime);
                unit.SetHex(axial);
            }
        }
    }
}