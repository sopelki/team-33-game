// using System;
// using HexagonScripts;
// using UnityEngine;
// using UnityEngine.Serialization;
//
//
// [Serializable]
// public class Hexagon
// {
//     public Vector2Int coordinates;
//     public Vector3Int offset;
//     public HexagonType type;
//     // public bool isWalkable;
//     // public bool isSlot;
//     public TextureType texture;
//
//     public Hexagon(int x, int y, Vector3Int offset, HexagonType type, TextureType texture = TextureType.Grass)
//     {
//         coordinates = new Vector2Int(x, y);
//         this.offset = offset;
//         this.type = type;
//         this.texture = texture;
//
//         // isWalkable = type == HexagonType.Path;
//         // isSlot = type == HexagonType.Slot;
//     }
//     
// }