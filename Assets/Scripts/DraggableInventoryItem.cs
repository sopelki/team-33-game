// using UnityEngine;
// using UnityEngine.EventSystems;
//
// public class DraggableInventoryItem : MonoBehaviour,
//     IBeginDragHandler, IDragHandler, IEndDragHandler
// {
//     private RectTransform rectTransform;
//     private CanvasGroup canvasGroup;
//     private Transform originalParent;
//     private Vector2 originalPosition;
//     private Canvas canvas;
//
//     private void Awake()
//     {
//         rectTransform = GetComponent<RectTransform>();
//         canvasGroup = GetComponent<CanvasGroup>();
//         
//         if (canvasGroup == null)
//             canvasGroup = gameObject.AddComponent<CanvasGroup>();
//     }
//
//     public void StartDragging(PointerEventData eventData, Canvas mainCanvas)
//     {
//         canvas = mainCanvas;
//         canvasGroup.blocksRaycasts = false;
//
//         RectTransformUtility.ScreenPointToLocalPointInRectangle(
//             canvas.transform as RectTransform,
//             eventData.position,
//             eventData.pressEventCamera,
//             out var localPoint);
//         
//         rectTransform.anchoredPosition = localPoint;
//     }
//
//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         originalParent = transform.parent;
//         originalPosition = rectTransform.anchoredPosition;
//         
//         if (canvas == null)
//             canvas = GetComponentInParent<Canvas>();
//         
//         transform.SetParent(canvas.transform, true);
//         canvasGroup.blocksRaycasts = false;
//     }
//
//     public void OnDrag(PointerEventData eventData)
//     {
//         if (canvas != null)
//             rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
//     }
//
//     public void OnEndDrag(PointerEventData eventData)
//     {
//         canvasGroup.blocksRaycasts = true;
//
//         if (transform.parent == canvas.transform)
//             Destroy(gameObject);
//     }
//
//     public void CenterInParent()
//     {
//         rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
//         rectTransform.anchoredPosition = Vector2.zero;
//     }
// }