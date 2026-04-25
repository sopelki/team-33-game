using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag?.GetComponent<InventoryItem>();
        
        if (item == null)
            return;

        if (transform.childCount == 0)
        {
            item.PlaceInto(transform);
            return;
        }

        if (item.SpawnedFromShop)
            return;


        var existing = transform.GetChild(0).GetComponent<InventoryItem>();

        existing.transform.SetParent(item.OriginalParent, false);
        existing.Center();

        item.PlaceInto(transform);
    }
}