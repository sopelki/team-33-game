using System.Collections;
using Logic.Castle;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ShopToCastleItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private GameObject inventoryItemPrefab;
        [SerializeField]
        private BuildingData buildingData;

        [Header("Feedback")]
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private float fadeDuration = 0.1f;

        private CanvasGroup iconCanvasGroup;
        private Image sourceImage;
        private GameObject spawnedItem;
        private bool isDragging;

        private void Awake()
        {
            if (buildingData != null && buildingData.viewPrefab != null)
                sourceImage = buildingData.viewPrefab.GetComponentInChildren<Image>();

            if (canvas == null)
                canvas = GetComponentInParent<Canvas>();

            iconCanvasGroup = iconImage.GetComponent<CanvasGroup>();
            if (iconCanvasGroup == null)
                iconCanvasGroup = iconImage.gameObject.AddComponent<CanvasGroup>();

            var trigger = gameObject.AddComponent<TooltipTrigger>();
            trigger.SetContent(buildingData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (inventoryItemPrefab == null || canvas == null)
                return;

            iconCanvasGroup.alpha = 0f;

            if (inventoryItemPrefab == null || canvas == null) return;
            isDragging = true;

            iconCanvasGroup.alpha = 0f;
            spawnedItem = Instantiate(inventoryItemPrefab, canvas.transform);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            spawnedItem.GetComponent<RectTransform>().localPosition = localPoint;

            var item = spawnedItem.GetComponent<InventoryItem>();
            item.SetData(buildingData, true);
            item.OnDropped += OnDroppedHandler;

            var itemImage = spawnedItem.GetComponent<Image>();
            if (sourceImage != null && itemImage != null)
            {
                itemImage.sprite = sourceImage.sprite;
                itemImage.color = sourceImage.color;
                itemImage.preserveAspect = true;
            }

            eventData.pointerDrag = spawnedItem;
            spawnedItem.GetComponent<CastleDragHandler>().OnBeginDrag(eventData);
            return;

            void OnDroppedHandler()
            {
                item.OnDropped -= OnDroppedHandler;
                HandleItemDropped();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData) => isDragging = false;

        private void OnDisable()
        {
            if (isDragging)
            {
                isDragging = false;
                if (spawnedItem != null) Destroy(spawnedItem);
                iconCanvasGroup.alpha = 1f;
            }
        }

        private void HandleItemDropped()
        {
            if (!gameObject.activeInHierarchy)
                return;

            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            var elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                iconCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                yield return null;
            }

            iconCanvasGroup.alpha = 1f;
        }
    }
}