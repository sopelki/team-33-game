using UnityEngine;

namespace View
{
    public class TrapView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Header("X-Ray Settings")]
        [SerializeField] private GameObject outline;
        
        private int occludersCount = 0;
        public void Initialize(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            SetOutlineVisible(false);
            occludersCount = 0;
        }
        
        public void SetOutlineVisible(bool value)
        {
            if (outline != null)
                outline.SetActive(value);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Tower"))
            {
                if (transform.position.y > other.transform.position.y)
                {
                    occludersCount++;
                    SetOutlineVisible(occludersCount > 0);
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Tower"))
            {
                if (transform.position.y > other.transform.position.y)
                {
                    occludersCount--;
                    if (occludersCount < 0) 
                        occludersCount = 0; 
                    SetOutlineVisible(occludersCount > 0);
                }
            }
        }

    }
}