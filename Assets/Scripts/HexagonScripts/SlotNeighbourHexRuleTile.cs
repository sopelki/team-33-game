using UnityEngine;
using UnityEngine.Tilemaps;

namespace HexagonScripts
{
    [CreateAssetMenu(fileName = "SlotNeighbourHexRuleTile",
        menuName = "2D/Tiles/Slot Neighbour Hex Rule Tile")]
    public class SlotNeighbourHexRuleTile : HexagonalRuleTile
    {
        [Tooltip("SlotTile")]
        public TileBase slotTile;

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            switch (neighbor)
            {
                // 1 клик в редакторе правил = "This" => проверяем на slotTile
                case TilingRuleOutput.Neighbor.This:
                    return other != null && other == slotTile;

                // 2 клика = "NotThis" => всё, что НЕ slotTile (включая пустоту)
                case TilingRuleOutput.Neighbor.NotThis:
                    return other != slotTile;
            }
            return base.RuleMatch(neighbor, other);
        }
    }
}