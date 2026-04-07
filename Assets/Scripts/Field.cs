using System.Collections.Generic;
using UnityEngine;
using HexagonScripts;

public class Field
{
    public Dictionary<Vector2Int, Hexagon> Hexagons = new();
    
    public void AddHexagon(int x, int y, HexagonType type)
    {
        var offsetPos = new Vector3Int(x, y, 0);
        var axialPos = HexagonMath.OffsetToAxial(x, y);
        
        var hex = new Hexagon(axialPos.x, axialPos.y, offsetPos, type);

        // Используем [axialPos] вместо Add, чтобы при перезаписи тайла кисточкой не было ошибки
        Hexagons[axialPos] = hex; 
    }
    
    public FieldData ExportToSaveData()
    {
        var data = new FieldData();
        data.savedHexes = new List<Hexagon>(Hexagons.Values);
        return data;
    }
    
    public void ImportFromFieldData(FieldData data)
    {
        Hexagons.Clear();
        foreach (var hex in data.savedHexes)
        {
            Hexagons.Add(hex.coordinates, hex); 
        }
    }
    
    // Плейсхолдерная реализация
    public void GenerateFieldData(int width, int height)
    {
        Hexagons.Clear();

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
        return Hexagons.GetValueOrDefault(axialCoords);
    }
}