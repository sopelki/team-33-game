using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class FieldGenerator: MonoBehaviour
{
    public TileBase basicTile;

    [Header("TileRender")]
    public List<TileMapping> tileMappings;
    
    public Field CurrentField;

    private Tilemap myTilemap;
    private Dictionary<HexagonType, TileBase> typeToTileDict;
    private Dictionary<TileBase, HexagonType> tileToTypeDict;

    private const int FieldWidth = 38;
    private const int FieldHeight = 32;

    private void Awake()
    {
        myTilemap = GetComponent<Tilemap>();
    }

    [ContextMenu("Regenerate Grid")]
    public void GenerateAndDraw()
    {
        ClearGrid();
        CurrentField = new Field();
        // Потом нужно убрать отсюда высоту и ширину, они уже должны быть в поле, их фиксируем
        CurrentField.GenerateFieldData(FieldWidth, FieldHeight);
        DrawField(CurrentField);
    }
    
    [ContextMenu("Read Field from editor")]
    public void ReadFieldFromBrush()
    {
        if (myTilemap == null)
        {
            myTilemap = GetComponent<Tilemap>();
        }

        SetupDictionaries();

        CurrentField = new Field();
        var bounds = myTilemap.cellBounds;
        var parsedHexes = 0;

        foreach (var pos in bounds.allPositionsWithin)
        {
            var tileOnScene = myTilemap.GetTile(pos);
            if (tileOnScene != null)
            {
                if (tileToTypeDict.TryGetValue(tileOnScene, out var foundType))
                {
                    CurrentField.AddHexagon(pos.x, pos.y, foundType);
                    parsedHexes++;
                }
                else
                {
                    Debug.LogWarning($"Тайл {tileOnScene.name} на {pos} не найден в списке Mappings!");
                }
            }
        }
        Debug.Log($"<color=green>Считывание завершено!</color> Гексов: {parsedHexes}");
    }
    
    [ContextMenu("Save Field To File")]
    public void SaveGrid()
    {
        if (CurrentField != null && CurrentField.Hexagons.Count > 0)
        {
            SaveLoadManager.SaveMapToFile(CurrentField);
        }
        else
        {
            Debug.LogWarning("Поле пустое! Сначала сгенерируйте его.");
        }
    }

    [ContextMenu("Load Field From File")]
    public void LoadAndDraw()
    {
        ClearGrid();
        
        var loadedField = SaveLoadManager.LoadMapFromFile();
        if (loadedField != null)
        {
            CurrentField = loadedField;
            DrawField(CurrentField);
            Debug.Log($"Отрисовано гексов из файла: {CurrentField.Hexagons.Count}");
        }
        else
        {
            Debug.LogWarning("Файл не найден. Генерируем базовое поле...");
            GenerateAndDraw(); // Запасной вариант, если файла еще нет
        }
    }

    private void SetupDictionaries()
    {
        typeToTileDict = new Dictionary<HexagonType, TileBase>();
        tileToTypeDict = new Dictionary<TileBase, HexagonType>();

        foreach (var mapping in tileMappings)
        {
            if (mapping.tileAsset != null)
            {
                typeToTileDict[mapping.type] = mapping.tileAsset;
                if (!tileToTypeDict.ContainsKey(mapping.tileAsset))
                {
                    tileToTypeDict.Add(mapping.tileAsset, mapping.type);
                }
            }
        }
    }

    private void DrawField(Field field)
    {
        if (myTilemap == null)
        {
            myTilemap = GetComponent<Tilemap>();
        }

        SetupDictionaries();
        foreach (var hexagon in field.Hexagons.Values)
        {
            if (typeToTileDict.TryGetValue(hexagon.type, out var tileToDraw))
            {
                myTilemap.SetTile(hexagon.offset, tileToDraw);
            }
            else
            {
                Debug.LogWarning($"Нет картинки для типа {hexagon.type}!");
            }
        }
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        if (myTilemap == null)
        {
            myTilemap = GetComponent<Tilemap>();
        }

        myTilemap?.ClearAllTiles();
    }
}