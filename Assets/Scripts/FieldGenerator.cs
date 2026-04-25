using System.Collections.Generic;
using System.Linq;
using HexagonScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldGenerator : MonoBehaviour
{
    public TileBase basicTile;
    
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
            myTilemap = GetComponent<Tilemap>();

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
            Debug.LogWarning("Field is Empty, Generate first.");
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
            myTilemap = GetComponent<Tilemap>();

        SetupDictionaries();
        foreach (var hexagon in field.Hexagons.Values)
        {
            if (typeToTileDict.TryGetValue(hexagon.type, out var tileToDraw))
                myTilemap.SetTile(hexagon.offset, tileToDraw);
            else
                Debug.LogWarning("FieldGenerator: references are not assigned.");
        }
    }

    private void ReadFieldFromBrush()
    {
        if (myTilemap == null)
            myTilemap = GetComponent<Tilemap>();

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
                    Debug.LogWarning($"Tile {tileOnScene.name} at {pos} not found in Mappings.");
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
                tileToTypeDict.Add(mapping.tileAsset, mapping.type);
        }
    }
}