using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class HexGridGenerator : MonoBehaviour
{
    // [Header("Grid properties")]
    private Tilemap myTilemap;

    public TileBase hexTile;

    [Header("Map size")]
    public int width = 33;

    public int height = 28;

    private void Start()
    {
        myTilemap = GetComponent<Tilemap>();
        GenerateGrid();
    }

    [ContextMenu("Regenerate Grid")]
    public void GenerateGrid()
    {
        ClearGrid();

        for (var x = -width / 2; x < width / 2; x++)
        {
            for (var y = -height / 2; y < height / 2; y++)
            {
                var cellPosition = new Vector3Int(x, y, 0);
                if (hexTile != null)
                    myTilemap?.SetTile(cellPosition, hexTile);
                else
                    return;
            }
        }
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        myTilemap?.ClearAllTiles();
    }
}