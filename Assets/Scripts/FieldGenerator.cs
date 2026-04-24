using System.Collections.Generic;
using System.Linq;
using HexagonScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldGenerator : MonoBehaviour
{
    public TileBase basicTile;

    [Header("Monster Settings")]
    public GameObject monsterPrefab;
    
    [Header("TileRender")]
    public List<TileMapping> tileMappings;

    public Field CurrentField;

    private Tilemap myTilemap;
    private Dictionary<HexagonType, TileBase> typeToTileDict;
    private Dictionary<TileBase, HexagonType> tileToTypeDict;

    private const int FieldWidth = 40;
    private const int FieldHeight = 42;

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

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        if (myTilemap == null)
        {
            myTilemap = GetComponent<Tilemap>();
        }

        myTilemap?.ClearAllTiles();
    }


    [ContextMenu("Save Field To File")]
    public void SaveGrid()
    {
        ReadFieldFromBrush();
        
        if (CurrentField.Count > 0)
        {
            ReadFieldFromBrush();
            SaveLoadManager.SaveMapToFile(CurrentField);
        }
        else
        {
            Debug.LogWarning("Field is Empty, Generate first.");
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
            Debug.Log($"Rendered Hexagons From File: {CurrentField.Count}");
        }
        else
        {
            Debug.LogWarning("File Not Found, Generating Basic Field.");
            GenerateAndDraw();
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
                Debug.LogWarning("FieldGenerator: references are not assigned.");
            }
        }
    }

    private void ReadFieldFromBrush()
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
                    Debug.LogWarning($"Tile {tileOnScene.name} at {pos} not found in Mappings.");
                }
            }
        }
        Debug.Log($"Hex parsed successfully: {parsedHexes}.");
    }

    private void SetupDictionaries()
    {
        typeToTileDict = new Dictionary<HexagonType, TileBase>();
        tileToTypeDict = new Dictionary<TileBase, HexagonType>();

        foreach (var mapping in tileMappings.Where(mapping => mapping.tileAsset != null))
        {
            typeToTileDict[mapping.type] = mapping.tileAsset;
            if (!tileToTypeDict.ContainsKey(mapping.tileAsset))
            {
                tileToTypeDict.Add(mapping.tileAsset, mapping.type);
            }
        }
    }
    
    [ContextMenu("Spawn Monster")]
    private void SpawnMonsters()
    {
        if (monsterPrefab == null)
        {
            Debug.LogError("Префаб монстра не назначен в FieldGenerator!");
            return;
        }
        if (CurrentField == null || CurrentField.Count == 0)
        {
            Debug.LogWarning("Поле не сгенерировано. Сначала сгенерируйте или загрузите поле.");
            return;
        }
    
        // --- Начало основной логики ---
    
        // 1. Находим максимальную X координату на всем поле
        int maxX = CurrentField.Hexagons.Values.Max(hex => hex.offset.x);
        Debug.Log($"Самая правая координата X на карте: {maxX}");
    
        // 2. Находим все гексы, которые находятся на этой крайней правой линии И являются путем (Path)
        List<Hexagon> spawnPoints = CurrentField.Hexagons.Values
            .Where(hex => hex.offset.x == maxX && hex.type == HexagonType.Path)
            .ToList();
    
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("На правом краю карты не найдено ни одной клетки типа 'Path' для спавна.");
            return;
        }
    
        Debug.Log($"Найдено {spawnPoints.Count} точек для спавна на правом краю.");
    
        // 3. Создаем по одному монстру в каждой найденной точке
        foreach (var spawnPoint in spawnPoints)
        {
            // Получаем мировую позицию центра гекса
            Vector3 spawnPosition = myTilemap.GetCellCenterWorld(spawnPoint.offset);
        
            // Создаем монстра в этой точке
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }
}