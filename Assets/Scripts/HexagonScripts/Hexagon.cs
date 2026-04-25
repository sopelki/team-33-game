using System;
using UnityEngine;

namespace HexagonScripts
{
    [Serializable]
    public class Hexagon
    {
        public Vector2Int coordinates;
        public Vector3Int offset;
        public HexagonType type;
        // public TextureType texture;
        
        // public bool isWalkable;
        // public bool isSlot;

        public Hexagon(int x, int y, Vector3Int offset, HexagonType type, TextureType texture = TextureType.Grass)
        {
            coordinates = new Vector2Int(x, y);
            this.offset = offset;
            this.type = type;
            // this.texture = texture;

            // isWalkable = type == HexagonType.Path;
            // isSlot = type == HexagonType.Slot;
        }
    
    }
}