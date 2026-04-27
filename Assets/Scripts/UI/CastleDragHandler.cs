using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class CastleDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Canvas canvas;

        public Canvas MainCanvas => canvas;
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
            // Чтобы предмет был над всеми остальными элементами UI
            transform.SetParent(canvas.transform, true);
            transform.SetAsLastSibling(); 
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Универсальная формула движения для любого Canvas
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }

        // Позволяет другим скриптам вручную центровать объект
        public void ResetPosition()
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}