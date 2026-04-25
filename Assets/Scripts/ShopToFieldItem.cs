using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ShopToFieldItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RectTransform mapViewport;

    [Header("Field")]
    [SerializeField]
    private Tilemap fieldTilemap;
    [SerializeField]
    private TileBase slotTile;
    [SerializeField]
    private GameObject unitPrefab;

    [Header("Ghost (след)")]
    [Range(0f, 1f)]
    [SerializeField]
    private float ghostAlpha = 0.7f;
    [SerializeField]
    private Vector2 ghostOffset = Vector2.zero;

    private GameObject ghost;
    private RectTransform ghostRect;

    private void Awake()
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null || mapViewport == null || fieldTilemap == null ||
            slotTile == null || unitPrefab == null)
        {
            Debug.LogWarning("ShopToFieldItem: не все ссылки заданы в инспекторе", this);
            return;
        }

        ghost = new GameObject(name + "_ghost",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image));
        ghost.transform.SetParent(canvas.transform, false);
        ghostRect = ghost.GetComponent<RectTransform>();

        var srcImage = GetComponent<Image>();
        var ghostImage = ghost.GetComponent<Image>();

        ghostImage.sprite = srcImage.sprite;
        ghostImage.preserveAspect = true;
        ghostImage.raycastTarget = false;

        var c = srcImage.color;
        c.a *= ghostAlpha;
        ghostImage.color = c;

        var srcRect = srcImage.rectTransform.rect;
        ghostRect.sizeDelta = new Vector2(srcRect.width, srcRect.height);
        ghostRect.pivot = new Vector2(0.5f, 0.5f);
        ghostRect.anchorMin = ghostRect.anchorMax = new Vector2(0.5f, 0.5f);

        UpdateGhostPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostRect == null)
            return;
        UpdateGhostPosition(eventData);
    }

    private void UpdateGhostPosition(PointerEventData eventData)
    {
        var canvasRT = (RectTransform)canvas.transform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRT,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint);

        ghostRect.localPosition = new Vector2(localPoint.x, localPoint.y) + ghostOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghost != null)
            Destroy(ghost);

        var cam = Camera.main;
        if (cam == null || mapViewport == null || fieldTilemap == null)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                mapViewport,
                eventData.position,
                eventData.pressEventCamera,
                out var local))
            return;

        var u = local.x / mapViewport.rect.width + 0.5f;
        var v = local.y / mapViewport.rect.height + 0.5f;

        if (u < 0f || u > 1f || v < 0f || v > 1f)
            return;

        var zDist = Mathf.Abs(cam.transform.position.z - fieldTilemap.transform.position.z);
        var worldPos = cam.ViewportToWorldPoint(new Vector3(u, v, zDist));
        worldPos.z = fieldTilemap.transform.position.z;

        var cellPos = fieldTilemap.WorldToCell(worldPos);
        if (!FindNearestSlotTile(cellPos, searchRadius: 2, out var closestSlotPos))
            return;
        if (closestSlotPos == Vector3Int.zero || fieldTilemap.GetTile(closestSlotPos) == null)
            return;

        var spawnPos = fieldTilemap.GetCellCenterWorld(closestSlotPos);
        spawnPos.z = fieldTilemap.transform.position.z;

        Instantiate(unitPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"Spawn tower at {spawnPos}");
    }

    private bool FindNearestSlotTile(Vector3Int centerPos, int searchRadius, out Vector3Int cellPos)
    {
        var nearestPos = Vector3Int.zero;
        var nearestDistance = float.MaxValue;
        var found = false;

        for (var x = centerPos.x - searchRadius; x <= centerPos.x + searchRadius; x++)
        {
            for (var y = centerPos.y - searchRadius; y <= centerPos.y + searchRadius; y++)
            {
                var checkPos = new Vector3Int(x, y, centerPos.z);
                var tile = fieldTilemap.GetTile(checkPos);

                if (tile == null || slotTile == null || tile.name != slotTile.name)
                    continue;

                var distance = Vector3Int.Distance(checkPos, centerPos);

                if (!(distance < nearestDistance))
                    continue;
                nearestDistance = distance;
                nearestPos = checkPos;
                found = true;
            }
        }
        cellPos = nearestPos;
        return found;
    }
}