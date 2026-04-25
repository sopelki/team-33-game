using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private bool isShopItem;

    public bool IsShopItem
    {
        get => isShopItem;
        set => isShopItem = value;
    }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Transform originalParent;
    private Vector2 originalPosition;
    private int originalSiblingIndex;
    
    private bool wasInShop;
    private Transform shopParent; // ✨ НОВОЕ - сохраняем родителя МАГАЗИНА

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        originalSiblingIndex = transform.GetSiblingIndex();
        
        wasInShop = isShopItem;
        
        // ✨ НОВОЕ - если из магазина, сохраняем магазин отдельно
        if (isShopItem)
        {
            shopParent = originalParent;
            CreateShopCopy();
            isShopItem = false;
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

        StartCoroutine(CheckDropAfterFrame());
    }

    private IEnumerator CheckDropAfterFrame()
    {
        yield return null;

        var wasDroppedInSlot = transform.parent != canvas.transform;

        if (!wasDroppedInSlot)
        {
            if (wasInShop)
            {
                // Был из магазина - удаляем (копия осталась)
                Destroy(gameObject);
            }
            else
            {
                // Был из инвентаря - возвращаем на место
                ReturnToOriginalPosition();
            }
        }
    }

    private void CreateShopCopy()
    {
        var copy = Instantiate(gameObject, originalParent);
        copy.transform.SetSiblingIndex(originalSiblingIndex);
        
        var copyRect = copy.GetComponent<RectTransform>();
        copyRect.anchoredPosition = originalPosition;
        
        var copyComponent = copy.GetComponent<ShopItem>();
        if (copyComponent != null)
        {
            copyComponent.isShopItem = true;
            copyComponent.shopParent = shopParent; // ✨ Копия помнит магазин
        }
    }

    private void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalPosition;
        transform.SetSiblingIndex(originalSiblingIndex);
    }

    public void CenterInParent()
    {
        rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
    }
    
    public bool WasInShop()
    {
        return wasInShop;
    }
}