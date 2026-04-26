using UnityEngine;
using UnityEngine.EventSystems;

namespace CastleScripts
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        
        [SerializeField] private Castle castle;

        public void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag?.GetComponent<InventoryItem>();
            if (item == null) 
                return;
            
            if (item.SpawnedFromShop)
            {
                if (transform.childCount > 0)
                    return;

                var instance = castle.TryAddBuilding(item.BuildingData);
                if (instance == null)
                    return;

                item.PlaceInto(transform);
                item.BuildingInstance = instance;
                return;
            }
            if (transform.childCount == 0)
            {
                item.PlaceInto(transform);
                return;
            }

            var existing = transform.GetChild(0).GetComponent<InventoryItem>();

            existing.transform.SetParent(item.OriginalParent, false);
            existing.Center();

            item.PlaceInto(transform);
        }
    }
}