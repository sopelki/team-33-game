using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HexagonScripts;

public class Field
{
    public IReadOnlyDictionary<Vector2Int, Hexagon> Hexagons => hexagons;
    public List<MapObjectData> MapObjects = new();
    private readonly Dictionary<Vector2Int, Hexagon> hexagons = new();

    // public int Count => hexagons.Count;

    public void AddHexagon(int x, int y, HexagonType type)
    {
        var offsetPos = new Vector3Int(x, y, 0);
        var axialPos = HexagonMath.OffsetToAxial(x, y);

        var hex = new Hexagon(axialPos.x, axialPos.y, offsetPos, type);

        hexagons[axialPos] = hex;
    }

    public FieldData ExportToSaveData()
    {
        var data = new FieldData
        {
            savedHexes = new List<Hexagon>(hexagons.Values),
            savedObjects = new List<MapObjectData>(MapObjects)
        };
        return data;
    }

    public void ImportFromFieldData(FieldData data)
    {
        hexagons.Clear();
        foreach (var hexagon in data.savedHexes)
            hexagons.Add(hexagon.coordinates, hexagon);

        MapObjects = data.savedObjects != null ? new List<MapObjectData>(data.savedObjects) : new List<MapObjectData>();
    }

    public void GenerateFieldData(int width, int height)
    {
        hexagons.Clear();

        HexagonType[] pool = {
            HexagonType.Grass, HexagonType.Grass, HexagonType.Grass, HexagonType.Grass, HexagonType.Grass,
            HexagonType.GrassWithMushroom,
            HexagonType.HighGrass, HexagonType.HighGrass
        };

        for (var x = -width / 2; x < width / 2; x++)
        {
            for (var y = -height / 2; y < height / 2; y++)
            {
                var randomType = pool[Random.Range(0, pool.Length)];
                AddHexagon(x, y, randomType);
            }
        }
    }

    // public Hexagon GetHex(Vector2Int axialCoords)
    // {
    //     return hexagons.GetValueOrDefault(axialCoords);
    // }

    // public List<Hexagon> GetNeighbors(Hexagon currentHex)
    // {
    //     var neighborDirections = new List<Vector2Int>
    //     {
    //         new(0, +1),
    //         new(+1, 0),
    //         new(+1, -1),
    //         new(0, -1),
    //         new(-1, 0),
    //         new(-1, +1)
    //     };
    //
    //     return neighborDirections
    //         .Select(direction => currentHex.coordinates + direction)
    //         .Select(GetHex)
    //         .Where(neighbor => neighbor != null)
    //         .ToList();
    // }
}