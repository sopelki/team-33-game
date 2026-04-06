using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    [System.Serializable]
    public struct TileMapping
    {
        public HexagonType type;
        public TileBase tileAsset;
    }
}