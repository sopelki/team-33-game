using System.Collections.Generic;
using UnityEngine;
using HexagonScripts;

public class Field
{
    public IReadOnlyDictionary<Vector2Int, Hexagon> Hexagons => hexagons;
    private readonly Dictionary<Vector2Int, Hexagon> hexagons = new();
    
    public int Count => hexagons.Count;

    public void AddHexagon(int x, int y, HexagonType type)
    {
        var offsetPos = new Vector3Int(x, y, 0);
        var axialPos = HexagonMath.OffsetToAxial(x, y);

        var hex = new Hexagon(axialPos.x, axialPos.y, offsetPos, type);

        // Используем [axialPos] вместо Add, чтобы при перезаписи тайла кисточкой не было ошибки
        hexagons[axialPos] = hex;
    }

    public FieldData ExportToSaveData()
    {
        var data = new FieldData { savedHexes = new List<Hexagon>(hexagons.Values) };
        return data;
    }

    public void ImportFromFieldData(FieldData data)
    {
        hexagons.Clear();
        foreach (var hexagon in data.savedHexes)
        {
            hexagons.Add(hexagon.coordinates, hexagon);
        }
    }

    // Плейсхолдерная реализация
    public void GenerateFieldData(int width, int height)
    {
        hexagons.Clear();

        var counter = 0;
        for (var x = -width / 2; x < width / 2; x++)
        {
            for (var y = -height / 2; y < height / 2; y++)
            {
                var type = (counter % 3 == 0) ? HexagonType.Path : HexagonType.Land;
                counter++;

                AddHexagon(x, y, type);
            }
        }
    }

    public Hexagon GetHex(Vector2Int axialCoords)
    {
        return hexagons.GetValueOrDefault(axialCoords);
    }

    public List<Hexagon> GetNeighbors(Hexagon currentHex)
    {
        var neighbors = new List<Hexagon>();
        
        var neighborDirections = new List<Vector2Int>
        {
            new(0, +1),
            new(+1, 0),
            new(+1, -1),
            new(0, -1),
            new(-1, 0),
            new(-1, +1)
        };
        
        foreach (var direction in neighborDirections)
        {
            var neighborCoords = currentHex.coordinates + direction; 
            var neighbor = GetHex(neighborCoords);
            
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}