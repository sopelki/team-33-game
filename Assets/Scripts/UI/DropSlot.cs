using Logic.Castle;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private Transform itemContainer;
        private CastleSystem castleSystem;

        public void Construct(CastleSystem system) => castleSystem = system;

        private InventoryItem GetStoredItem() => itemContainer.GetComponentInChildren<InventoryItem>();

        public void OnDrop(PointerEventData eventData)
        {
            var draggingItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
            if (draggingItem == null)
                return;

            var existingItem = GetStoredItem();

            if (draggingItem.IsFromShop)
            {
                if (existingItem != null)
                    return;

                if (castleSystem.TryBuyBuilding(draggingItem.BuildingData))
                    draggingItem.Place(itemContainer);
                else
                    Destroy(draggingItem.gameObject);

                return;
            }

            if (existingItem != null)
                existingItem.Place(draggingItem.OriginalParent);

            draggingItem.Place(itemContainer);
        }
    }
}