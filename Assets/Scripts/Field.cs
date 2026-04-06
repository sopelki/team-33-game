using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Field
{
    public Dictionary<Vector2Int, Hexagon> hexagons = new();

    public void GenerateFieldData(int width, int height)
    {
        hexagons.Clear();

        var counter = 0;
        for (var x = -width / 2; x < width / 2; x++)
        {
            for (var y = -height / 2; y < height / 2; y++)
            {
                var offsetPos = new Vector3Int(x, y, 0);
                HexagonType type;
                
                if (counter % 3 == 0)
                {
                    type = HexagonType.Path;
                }
                else
                {
                    type = HexagonType.Grass;
                }

                counter++;
                var axialPos = HexagonMath.OffsetToAxial(x, y);
                var hex = new Hexagon(axialPos.x, axialPos.y, offsetPos, type);

                hexagons.Add(axialPos, hex);
            }
        }
    }

    public Hexagon GetHex(Vector2Int axialCoords)
    {
        if (hexagons.TryGetValue(axialCoords, out var hex))
        {
            return hex;
        }

        return null;
    }
}