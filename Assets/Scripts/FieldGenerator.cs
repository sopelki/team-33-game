using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class FieldGenerator : MonoBehaviour
{
    public TileBase basicTile;

    [Header("TileRender")]
    public List<TileMapping> tileMappings;
    
    public Field CurrentField;

    private Tilemap myTilemap;
    private Dictionary<HexagonType, TileBase> tileTypeSpriteDictionary;

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

    private void SetupDictionary()
    {
        tileTypeSpriteDictionary = new Dictionary<HexagonType, TileBase>();
        foreach (var mapping in tileMappings)
        {
            tileTypeSpriteDictionary[mapping.type] = mapping.tileAsset;
        }
    }

    private void DrawField(Field field)
    {
        if (myTilemap == null)
        {
            myTilemap = GetComponent<Tilemap>();
        }

        SetupDictionary();
        foreach (var hexagon in field.Hexagons.Values)
        {
            if (tileTypeSpriteDictionary.TryGetValue(hexagon.type, out var tileToDraw))
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