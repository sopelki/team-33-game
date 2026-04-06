using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraSetup : MonoBehaviour
{
    [Header("References")]
    public Tilemap tilemap;

    [Header("Padding")]
    public float padding = -1f;

    [ContextMenu("Fit Camera to Grid")]
    public void FitToGrid()
    {
        var cam = GetComponent<Camera>();

        if (tilemap == null)
        {
            Debug.LogWarning("CameraSetup: references are not assigned.");
            return;
        }

        var tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
        if (tilemapRenderer == null)
        {
            Debug.LogWarning("CameraSetup: references are not assigned.");
            return;
        }

        var bounds = tilemapRenderer.bounds;

        var camPos = transform.position;
        camPos.x = bounds.center.x;
        camPos.y = bounds.center.y;
        transform.position = camPos;

        var halfHeight = bounds.size.y * 0.5f + padding;
        var halfWidth = bounds.size.x * 0.5f + padding;

        var sizeFromWidth = halfWidth / cam.aspect;
        cam.orthographicSize = Mathf.Max(halfHeight, sizeFromWidth);
    }
}