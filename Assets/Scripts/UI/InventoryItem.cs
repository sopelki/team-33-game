using Logic.Castle;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [Header("Data")]
        [SerializeField] private BuildingData buildingData;
        
        public BuildingData BuildingData => buildingData;
        public Transform OriginalParent { get; private set; }
        public bool IsFromShop { get; private set; }

        private Vector2 startPosition;
        private int startIndex;
        private CastleDragHandler dragHandler; 

        private void Awake()
        {
            dragHandler = GetComponent<CastleDragHandler>();
        }

        public void SetData(BuildingData data, bool fromShop)
        {
            buildingData = data;
            IsFromShop = fromShop;
        }

        private void CaptureState()
        {
            OriginalParent = transform.parent;
            startPosition = GetComponent<RectTransform>().anchoredPosition;
            startIndex = transform.GetSiblingIndex();
        }

        private void ReturnToStart()
        {
            transform.SetParent(OriginalParent);
            transform.SetSiblingIndex(startIndex);
            GetComponent<RectTransform>().anchoredPosition = startPosition;
        }

        public void Place(Transform slot)
        {
            transform.SetParent(slot);
            dragHandler.ResetPosition();
            IsFromShop = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsFromShop) 
                return; 
            CaptureState();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.parent == dragHandler.MainCanvas.transform)
            {
                if (IsFromShop) 
                    Destroy(gameObject);
                else 
                    ReturnToStart();
            }
        }
    }
}