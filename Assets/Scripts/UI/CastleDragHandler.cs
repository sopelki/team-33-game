using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class CastleDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        public Canvas MainCanvas { get; private set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            MainCanvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f; // Делаем предмет полупрозрачным при таскании
            transform.SetParent(MainCanvas.transform, true);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Используем scaleFactor, чтобы скорость мышки не зависела от разрешения экрана
            rectTransform.anchoredPosition += eventData.delta / MainCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Возвращаем стандартный вид
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }

        // Вызывается из InventoryItem.Place() или ReturnToStart()
        public void ResetPosition()
        {
            rectTransform.anchoredPosition = Vector2.zero;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }
}