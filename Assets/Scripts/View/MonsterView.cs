using UnityEngine;
using Logic.Monster;

namespace View
{
    public class MonsterView : MonoBehaviour
    {
        private MonsterModel model;
        private Vector3 previousPosition;
        
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Direction Sprites")]
        [SerializeField] private Sprite up;
        [SerializeField] private Sprite down;
        [SerializeField] private Sprite left;
        [SerializeField] private Sprite right;
        [SerializeField] private Sprite upLeft;
        [SerializeField] private Sprite upRight;
        [SerializeField] private Sprite downLeft;
        [SerializeField] private Sprite downRight;


        public void Initialize(MonsterModel model)
        {
            this.model = model;
            transform.position = model.WorldPosition;
            previousPosition = model.WorldPosition;
        }
        
        public void UpdateView()
        {
            Vector3 currentPosition = model.WorldPosition;
            Vector3 direction = currentPosition - previousPosition;

            transform.position = currentPosition;

            if (direction.sqrMagnitude > 0.0001f)
            {
                UpdateDirection(direction);
                previousPosition = currentPosition;
            }
        }
        
        private void UpdateDirection(Vector3 direction)
        {
            direction.Normalize();

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle >= -22.5f && angle < 22.5f)
                spriteRenderer.sprite = right;
            else if (angle >= 22.5f && angle < 67.5f)
                spriteRenderer.sprite = upRight;
            else if (angle >= 67.5f && angle < 112.5f)
                spriteRenderer.sprite = up;
            else if (angle >= 112.5f && angle < 157.5f)
                spriteRenderer.sprite = upLeft;
            else if (angle >= 157.5f || angle < -157.5f)
                spriteRenderer.sprite = left;
            else if (angle >= -157.5f && angle < -112.5f)
                spriteRenderer.sprite = downLeft;
            else if (angle >= -112.5f && angle < -67.5f)
                spriteRenderer.sprite = down;
            else
                spriteRenderer.sprite = downRight;
        }
        
    }
}