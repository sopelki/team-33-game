using Logic.Castle;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        
        private CastleSystem castleSystem;

        public void Construct(CastleSystem system)
        {
            castleSystem = system;
        }
        public void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag?.GetComponent<InventoryItem>();
            if (item == null) 
                return;
            
            if (item.IsFromShop)
            {
                if (transform.childCount > 0)
                    return;
                
                if (castleSystem.TryBuyBuilding(item.BuildingData))
                    item.Place(transform);
                else
                    Destroy(item.gameObject);
                return;
            }
            
            if (transform.childCount != 0)
            {
                var existing = transform.GetChild(0).GetComponent<InventoryItem>();
                existing.Place(item.OriginalParent);
            }
            item.Place(transform); 
        }
    }
}