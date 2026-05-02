using UnityEngine;
using UnityEngine.Tilemaps;
using HexagonScripts; // Обязательно, чтобы видеть HexagonMath

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Misc
{
    // Повесьте этот скрипт прямо на объект с Tilemap
    public class HexLabeler : MonoBehaviour
    {
        private Tilemap tilemap;

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (tilemap == null)
                tilemap = GetComponent<Tilemap>();

            if (tilemap == null) return;

            var style = new GUIStyle
            {
                normal =
                {
                    textColor = Color.white
                },
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };

            var bounds = tilemap.cellBounds;

            foreach (var pos in bounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos))
                    continue;
                
                var axial = HexagonMath.OffsetToAxial(pos.x, pos.y);

                var worldPos = tilemap.GetCellCenterWorld(pos);

                worldPos.z -= 0.1f;

                Handles.Label(worldPos, $"{axial.x}, {axial.y}", style);
            }
#endif
        }
    }
}