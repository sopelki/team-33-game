using HexagonScripts;
using UnityEngine.Tilemaps;

namespace Field
{
    [System.Serializable]
    public struct TileMapping
    {
        public HexagonType type;
        public TileBase tileAsset;
    }
}