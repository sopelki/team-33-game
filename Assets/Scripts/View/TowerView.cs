using UnityEngine;

namespace View
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public float debugRange;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, debugRange);
        }

        public void Initialize(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}