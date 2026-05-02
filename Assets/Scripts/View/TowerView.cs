using UnityEngine;

namespace View
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform firePoint;

        public void Initialize(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void PlayShootEffect() 
        {
            // Эффекты выстрела
        }
    
        public float debugRange; 
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, debugRange);
        }
    }
}