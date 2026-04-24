using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private bool isShopItem;   // true только для иконок в магазине

    public bool IsShopItem
    {
        get => isShopItem;
        set => isShopItem = value;
    }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        // Если это товар в магазине — оставляем копию
        if (isShopItem)
        {
            var placeholder = Instantiate(gameObject, originalParent);
        
            // у копии не должно быть своего Drag'а (чтобы она не таскалась)
            var drag = placeholder.GetComponent<DraggableItem>();
            if (drag != null)
            {
                Destroy(drag);
            }
        }

        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // если не попали ни в какой слот
        if (transform.parent == canvas.transform)
        {
            if (isShopItem)
            {
                // это "призрак" из магазина — просто удалить
                Destroy(gameObject);
            }
            else
            {
                // обычный инвентарный предмет — вернуть на место
                transform.SetParent(originalParent, false);
                CenterInParent();
            }
        }
    }

    public void CenterInParent()
    {
        rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}