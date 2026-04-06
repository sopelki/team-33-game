using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class CreateMap : MonoBehaviour
{
    private Tilemap myTilemap;

    public TileBase hexTile;

    [Header("Map size")]
    public int width = 33;
    public int height = 28;

    private void Awake()
    {
        myTilemap = GetComponent<Tilemap>();
    }

    [ContextMenu("Regenerate Grid")]
    public void GenerateGrid()
    {
        if (myTilemap == null)
            myTilemap = GetComponent<Tilemap>();

        if (hexTile == null)
        {
            Debug.LogWarning("CreateMap: references are not assigned.");
            return;
        }

        ClearGrid();

        for (var x = -width / 2; x < width / 2; x++)
        {
            for (var y = -height / 2; y < height / 2; y++)
            {
                var cellPosition = new Vector3Int(x, y, 0);
                myTilemap.SetTile(cellPosition, hexTile);
            }
        }
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        if (myTilemap == null)
            myTilemap = GetComponent<Tilemap>();

        myTilemap.ClearAllTiles();
    }
}