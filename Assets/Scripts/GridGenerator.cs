using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    [ExecuteAlways]
    public class HexGridGenerator : MonoBehaviour
    {
        private Tilemap myTilemap;
        public TileBase hexTile;
        
        
        [Header("TileRender")]
        public List<TileMapping> tileMappings; // Список твоих тайлов
        
        private Dictionary<HexagonType, TileBase> tileDictionary;

        [Header("Map size")] public int width = 35;
        public int height = 32;

        public Field currentField;

        private void Start()
        {
            myTilemap = GetComponent<Tilemap>();
            GenerateAndDraw();
        }

        [ContextMenu("Regenerate Grid")]
        public void GenerateAndDraw()
        {
            ClearGrid();
            currentField = new Field();
            currentField.GenerateFieldData(width, height);
            DrawField(currentField);
        }
        
        private void SetupDictionary()
        {
            tileDictionary = new Dictionary<HexagonType, TileBase>();
            foreach (var mapping in tileMappings)
            {
                tileDictionary[mapping.type] = mapping.tileAsset;
            }
        }

        private void DrawField(Field field)
        {
            if (myTilemap == null)
            {
                myTilemap = GetComponent<Tilemap>();
            }
            if (hexTile == null)
            {
                return;
            }
            SetupDictionary();
            foreach (var hex in field.hexagons.Values)
            {
                if (tileDictionary.TryGetValue(hex.type, out var tileToDraw))
                {
                    myTilemap.SetTile(hex.offsetCoordinates, tileToDraw);
                }
                else
                {
                    Debug.LogWarning($"Нет картинки для типа {hex.type}!");
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
}