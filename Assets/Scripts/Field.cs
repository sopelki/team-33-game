using System.Collections.Generic;
using UnityEngine;

public class Field
{
    public Dictionary<Vector2Int, Hexagon> Hexagons = new();

    // Плейсхолдерная реализация, потом будет парсер
    public void GenerateFieldData(int width, int height)
    {
        Hexagons.Clear();

        var counter = 0;
        for (var x = -width / 2; x < width / 2; x++)
        {
            for (var y = -height / 2; y < height / 2; y++)
            {
                var offsetPos = new Vector3Int(x, y, 0);

                var type = counter % 3 == 0 ? HexagonType.Path : HexagonType.Land;

                counter++;
                var axialPos = HexagonMath.OffsetToAxial(x, y);
                var hexagon = new Hexagon(axialPos.x, axialPos.y, offsetPos, type);

                Hexagons.Add(axialPos, hexagon);
            }
        }
    }

    public Hexagon GetHex(Vector2Int axialCoords)
    {
        return Hexagons.GetValueOrDefault(axialCoords);
    }
}