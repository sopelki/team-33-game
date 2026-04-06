using System;
using UnityEngine;

[Serializable]
public class Hexagon
{
    public Vector2Int coordinates;
    public Vector3Int offsetCoordinates;
    public HexagonType type;
    public bool isWalkable;
    public Texture texture ;

    public Hexagon(int q, int r, Vector3Int offset, HexagonType type, Texture texture = Texture.Grass)
    {
        coordinates = new Vector2Int(q, r);
        this.offsetCoordinates = offset;
        this.type = type;
        isWalkable = (type == HexagonType.Path);
        this.texture = texture;
    }
}