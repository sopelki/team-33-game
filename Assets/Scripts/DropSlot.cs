using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var draggedObject = eventData.pointerDrag;
        if (draggedObject == null)
            return;

        var draggedItem = draggedObject.GetComponent<ShopItem>();
        if (draggedItem == null)
            return;

        // Если слот пуст — просто кладём
        if (transform.childCount == 0)
        {
            draggedObject.transform.SetParent(transform, false);
            draggedItem.CenterInParent();
            return;
        }

        // Слот занят
        var existingItem = transform.GetChild(0);
        var existingItemComponent = existingItem.GetComponent<ShopItem>();

        // ✅ ВАЖНО: если предмет был взят из магазина — ЗАПРЕЩАЕМ drop
        if (draggedItem.WasInShop())
        {
            return; // ничего не делаем
        }

        // ✅ Если предмет из инвентаря — делаем swap
        existingItem.SetParent(draggedItem.originalParent, false);
        if (existingItemComponent != null)
            existingItemComponent.CenterInParent();

        draggedObject.transform.SetParent(transform, false);
        draggedItem.CenterInParent();
    }
}