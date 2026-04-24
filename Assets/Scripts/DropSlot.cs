using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if (item == null)
        {
            return;
        }

        // переезд предмета в этот слот
        if (transform.childCount != 0)
        {
            return;
        }
        item.transform.SetParent(transform, false);
        item.CenterInParent();
        
        // попал в инвентарь → больше НЕ товар магазина
        item.IsShopItem = false;
    }
}