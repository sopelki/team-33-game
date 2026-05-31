using UnityEngine;
using UnityEngine.EventSystems;
using View;

namespace Misc
{
    public class ViewportRaycaster : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler
    {
        [Header("Настройки")]
        [SerializeField]
        private Camera worldCamera;
        [SerializeField]
        private RectTransform viewportRect;

        private MonsterInteractionHandler lastHoveredMonster;

        public void OnPointerClick(PointerEventData eventData)
        {
            var monster = RaycastMonster(eventData);
            if (monster)
                monster.OnPointerClick(eventData);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            var currentMonster = RaycastMonster(eventData);

            if (currentMonster != lastHoveredMonster)
            {
                if (lastHoveredMonster) lastHoveredMonster.OnPointerExit(eventData);
                if (currentMonster) currentMonster.OnPointerEnter(eventData);

                lastHoveredMonster = currentMonster;
            }
        }

        private MonsterInteractionHandler RaycastMonster(PointerEventData eventData)
        {
            if (!worldCamera || !viewportRect)
                return null;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewportRect, eventData.position,
                    eventData.pressEventCamera, out var localPoint))
                return null;

            var r = viewportRect.rect;
            var normalizedX = (localPoint.x - r.x) / r.width;
            var normalizedY = (localPoint.y - r.y) / r.height;

            var ray = worldCamera.ViewportPointToRay(new Vector3(normalizedX, normalizedY, 0));

            var hit = Physics2D.GetRayIntersection(ray);

            return hit.collider != null ? hit.collider.GetComponent<MonsterInteractionHandler>() : null;
        }
    }
}