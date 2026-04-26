using CastleScrripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CastleScripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class InventoryItem : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Canvas canvas;

        private Vector2 originalPosition;
        private int originalSiblingIndex;
        private BuildingData buildingData;
        
        
        public BuildingData BuildingData => buildingData;

        public void SetBuildingData(BuildingData data)
        {
            buildingData = data;
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void InitializeFromShop(Canvas parentCanvas, PointerEventData eventData)
        {
            canvas = parentCanvas;
            SpawnedFromShop = true;
            OriginalParent = transform.parent;

            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out var localPoint);

            rectTransform.anchoredPosition = localPoint;

            var pos = rectTransform.localPosition;
            pos.z = 0;
            rectTransform.localPosition = pos;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (SpawnedFromShop) return;

            if (canvas == null) canvas = GetComponentInParent<Canvas>();

            OriginalParent = transform.parent;
            originalPosition = rectTransform.anchoredPosition;
            originalSiblingIndex = transform.GetSiblingIndex();

            transform.SetParent(canvas.transform, true);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canvas == null) return;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        
            if (transform.parent != canvas.transform)
                return;
            if (SpawnedFromShop)
                Destroy(gameObject);
            else
                ReturnToOriginal();
        }

        private void ReturnToOriginal()
        {
            transform.SetParent(OriginalParent, false);
            rectTransform.anchoredPosition = originalPosition;
            transform.SetSiblingIndex(originalSiblingIndex);
        }

        public void PlaceInto(Transform slot)
        {
            transform.SetParent(slot, false);
            Center();
            SpawnedFromShop = false;
        }

        public void Center()
        {
            rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
        }

        public Transform OriginalParent { get; private set; }
        public bool SpawnedFromShop { get; private set; }
        public BuildingInstance BuildingInstance { get; set; }
    }
}