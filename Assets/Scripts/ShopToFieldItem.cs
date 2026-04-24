using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ShopToFieldItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    [SerializeField] private Canvas canvas;              // главный Canvas
    [SerializeField] private RectTransform mapViewport;  // RectTransform Viewport (RawImage с картой)

    [Header("Field")]
    [SerializeField] private Tilemap fieldTilemap;       // Tilemap поля
    [SerializeField] private TileBase slotTile;          // Tile asset слота (Slot Tile Rule)
    [SerializeField] private GameObject unitPrefab;      // префаб башни (SpriteRenderer)

    [Header("Ghost (след)")]
    [Range(0f, 1f)]
    [SerializeField] private float ghostAlpha = 0.7f;    // прозрачность следа
    [SerializeField] private Vector2 ghostOffset = Vector2.zero;

    private GameObject ghost;
    private RectTransform ghostRect;

    private void Awake()
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    // ===== DRAG =====

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null || mapViewport == null || fieldTilemap == null ||
            slotTile == null || unitPrefab == null)
        {
            Debug.LogWarning("ShopToFieldItem: не все ссылки заданы в инспекторе", this);
            return;
        }

        // создаём пустой объект-призрак под Canvas
        ghost = new GameObject(name + "_ghost",
                               typeof(RectTransform),
                               typeof(CanvasRenderer),
                               typeof(Image));
        ghost.transform.SetParent(canvas.transform, false);
        ghostRect = ghost.GetComponent<RectTransform>();

        // копируем спрайт/цвет из исходной иконки
        var srcImage = GetComponent<Image>();
        var ghostImage = ghost.GetComponent<Image>();

        ghostImage.sprite = srcImage.sprite;
        ghostImage.preserveAspect = true;
        ghostImage.raycastTarget = false;

        Color c = srcImage.color;
        c.a *= ghostAlpha;
        ghostImage.color = c;

        // задаём размер и центрирование
        Rect srcRect = srcImage.rectTransform.rect;
        ghostRect.sizeDelta = new Vector2(srcRect.width, srcRect.height);
        ghostRect.pivot = new Vector2(0.5f, 0.5f);
        ghostRect.anchorMin = ghostRect.anchorMax = new Vector2(0.5f, 0.5f);

        UpdateGhostPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostRect == null) return;
        UpdateGhostPosition(eventData);
    }

    private void UpdateGhostPosition(PointerEventData eventData)
    {
        RectTransform canvasRT = (RectTransform)canvas.transform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRT,
            eventData.position,
            eventData.pressEventCamera,   // для Screen Space Overlay он null, это ок
            out Vector2 localPoint);

        ghostRect.localPosition = localPoint + ghostOffset;
    }

    // ===== DROP =====

    public void OnEndDrag(PointerEventData eventData)
    {
        // убираем след
        if (ghost != null)
            Destroy(ghost);

        Camera cam = Camera.main;
        if (cam == null || mapViewport == null || fieldTilemap == null)
            return;

        // --- 1. экран → локальные координаты Viewport (RawImage) ---
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                mapViewport,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 local))
        {
            return;
        }

        // local.x/y ∈ [-w/2 ; w/2] → нормализуем в [0..1]
        float u = (local.x / mapViewport.rect.width)  + 0.5f;
        float v = (local.y / mapViewport.rect.height) + 0.5f;

        // мышь вышла за пределы окна карты – ничего не делаем
        if (u < 0f || u > 1f || v < 0f || v > 1f)
            return;

        // --- 2. viewport (u,v) → мировая позиция на плоскости тайлмапа ---
        float zDist = Mathf.Abs(cam.transform.position.z - fieldTilemap.transform.position.z);
        Vector3 worldPos = cam.ViewportToWorldPoint(new Vector3(u, v, zDist));
        worldPos.z = fieldTilemap.transform.position.z;

        // --- 3. мир → клетка тайлмапа ---
        Vector3Int cellPos = fieldTilemap.WorldToCell(worldPos);
        TileBase tile = fieldTilemap.GetTile(cellPos);

        // проверка: это именно Slot-тайл?
        if (tile == null || slotTile == null || tile.name != slotTile.name)
            return;

        // --- 4. центр клетки и спавн башни ---
        Vector3 spawnPos = fieldTilemap.GetCellCenterWorld(cellPos);
        spawnPos.z = fieldTilemap.transform.position.z;

        Instantiate(unitPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"Spawn tower at {spawnPos}");
    }
}