using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CastleScripts
{
    public class ShopToCastleItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject inventoryItemPrefab;
        [SerializeField] private BuildingData buildingData;

        private InventoryItem currentClone;
        private Image sourceImage;

        private void Awake()
        {
            sourceImage = GetComponent<Image>();
            if (canvas == null) canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (inventoryItemPrefab == null || canvas == null) return;

            var itemGo = Instantiate(inventoryItemPrefab, canvas.transform);
            itemGo.transform.SetAsLastSibling();

            currentClone = itemGo.GetComponent<InventoryItem>();
            currentClone.SetBuildingData(buildingData);

            var itemImage = itemGo.GetComponent<Image>();
            if (sourceImage != null && itemImage != null)
            {
                itemImage.sprite = sourceImage.sprite;
                itemImage.color = sourceImage.color;
                itemImage.preserveAspect = sourceImage.preserveAspect;
            }

            currentClone.InitializeFromShop(canvas, eventData);

            eventData.pointerDrag = currentClone.gameObject;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (currentClone != null)
                currentClone.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (currentClone == null) return;
            currentClone.OnEndDrag(eventData);
            currentClone = null;
        }
    }
}