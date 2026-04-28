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

    [Header("Object Render")]
    [Tooltip("Prefab names should start with objectMapping IDs")]
    public List<ObjectMapping> objectMappings;
    public Transform objectsContainer;

    public Field CurrentField;

    private Tilemap myTilemap;
    private Dictionary<HexagonType, TileBase> typeToTileDict;
    private Dictionary<TileBase, HexagonType> tileToTypeDict;

    private const int FieldWidth = 39;
    private const int FieldHeight = 44;

    private void Awake()
    {
        myTilemap = GetComponent<Tilemap>();
    }


    [ContextMenu("Load Field From File")]
    public void LoadAndDraw()
    {
        ClearField();

        var loadedField = SaveLoadManager.LoadMapFromFile();

        if (loadedField != null)
        {
            CurrentField = loadedField;
            DrawHexagons(CurrentField);
            DrawObjects();
            Debug.Log($"Field Loaded: {CurrentField.Hexagons.Count} tiles, {CurrentField.MapObjects.Count} objects.");
        }
        else
        {
            Debug.LogWarning("Load Field From File: File not found. Generating default grid.");
            GenerateAndDraw();
        }
    }

    [ContextMenu("Save Field To File")]
    public void SaveGrid()
    {
        CurrentField = new Field();

        ReadHexagonsFromBrush();
        ReadObjectsFromScene();

        if (CurrentField.Hexagons.Count > 0)
        {
            SaveLoadManager.SaveMapToFile(CurrentField);
            Debug.Log($"Saved {CurrentField.Hexagons.Count} tiles and {CurrentField.MapObjects.Count} objects.");
        }
        else
            Debug.LogWarning("Save Field To File: Tilemap is empty.");
    }

    [ContextMenu("Generate Random Grid")]
    public void GenerateAndDraw()
    {
        ClearField();
        CurrentField = new Field();
        CurrentField.GenerateFieldData(FieldWidth, FieldHeight);
        DrawHexagons(CurrentField);
    }


    [ContextMenu("Clear Field")]
    public void ClearField()
    {
        ClearGrid();
        ClearDecorations();
        CurrentField = new Field();
    }

    public void ClearGrid()
    {
        if (myTilemap == null)
            myTilemap = GetComponent<Tilemap>();
        
        myTilemap?.ClearAllTiles();
    }

    public void ClearDecorations()
    {
        if (objectsContainer == null)
            return;

        for (var i = objectsContainer.childCount - 1; i >= 0; i--)
        {
            var obj = objectsContainer.GetChild(i).gameObject;
            if (Application.isPlaying)
                Destroy(obj);
            else
                DestroyImmediate(obj);
        }
    }


    private void DrawHexagons(Field field)
    {
        if (myTilemap == null)
            myTilemap = GetComponent<Tilemap>();
        
        SetupDictionaries();
        
        foreach (var hexagon in field.Hexagons.Values)
        {
            if (typeToTileDict.TryGetValue(hexagon.type, out var tileToDraw))
                myTilemap.SetTile(hexagon.offset, tileToDraw);
        }
    }

    private void DrawObjects()
    {
        if (objectsContainer == null || CurrentField.MapObjects == null)
            return;

        foreach (var objData in CurrentField.MapObjects)
        {
            var mapping = objectMappings.Find(m => m.id == objData.objectId);
            if (mapping != null && mapping.prefab != null)
            {
                var worldPos = myTilemap.GetCellCenterWorld(objData.position);
                var newObj = Instantiate(mapping.prefab, worldPos, Quaternion.identity, objectsContainer);
                newObj.name = mapping.id;
            }
        }
    }

    private void ReadHexagonsFromBrush()
    {
        if (myTilemap == null)
            myTilemap = GetComponent<Tilemap>();
        
        SetupDictionaries();

        var bounds = myTilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            var tileOnScene = myTilemap.GetTile(pos);
            if (tileOnScene != null && tileToTypeDict.TryGetValue(tileOnScene, out var foundType))
                CurrentField.AddHexagon(pos.x, pos.y, foundType);
        }
    }

    private void ReadObjectsFromScene()
    {
        CurrentField.MapObjects.Clear();
        if (objectsContainer == null)
            return;

        foreach (Transform child in objectsContainer)
        {
            var mapping = objectMappings.Find(m => child.name.StartsWith(m.id));
            if (mapping != null)
            {
                CurrentField.MapObjects.Add(new MapObjectData
                {
                    position = myTilemap.WorldToCell(child.position),
                    objectId = mapping.id
                });
            }
        }
    }

    private void SetupDictionaries()
    {
        typeToTileDict = new Dictionary<HexagonType, TileBase>();
        tileToTypeDict = new Dictionary<TileBase, HexagonType>();

        foreach (var mapping in tileMappings.Where(m => m.tileAsset != null))
        {
            typeToTileDict[mapping.type] = mapping.tileAsset;
            if (!tileToTypeDict.ContainsKey(mapping.tileAsset))
                tileToTypeDict.Add(mapping.tileAsset, mapping.type);
        }
    }
}