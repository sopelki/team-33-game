using UnityEngine;
using Logic.Monster;

namespace View
{
    public class MonsterView : MonoBehaviour
    {
        private static readonly int moveX = Animator.StringToHash("MoveX");
        private static readonly int moveY = Animator.StringToHash("MoveY");
        private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
        private static readonly int LastMoveY = Animator.StringToHash("LastMoveY");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int IsDamaged = Animator.StringToHash("IsDamaged");
        private static readonly int IsDead = Animator.StringToHash("IsDead");
        private MonsterModel model;
        private Vector3 previousPosition;

        [SerializeField]
        private Animator animator;
        private Vector2 targetDirection;
        private Vector2 currentSmoothDirection;
        [SerializeField]
        private float smoothingSpeed = 10f;
        private Vector3 visualOffset;
        private bool isDeadAnimationPlaying;


        public void Initialize(MonsterModel monsterModel, float visualOffset)
        {
            model = monsterModel;
            transform.position = model.WorldPosition;
            previousPosition = model.WorldPosition;
            this.visualOffset = new Vector3(0, model.Data.visualOffsetY, 0);
            
            model.OnAttack += HandleAttack;
            model.OnDied += HandleDeath;
            model.OnDamaged += HandleDamaged;
        }

        public void UpdateView()
        {
            if (model.IsDead)
            {
                if (!isDeadAnimationPlaying)
                {
                    isDeadAnimationPlaying = true;
                    animator.SetBool(IsMoving, false);
                    animator.SetBool(IsDead, true);
                }
                return; 
            }
            
            var logicalPosition = model.WorldPosition;
            var direction = logicalPosition - previousPosition;
            var moving = direction.sqrMagnitude > 0.0001f;

            transform.position = logicalPosition + visualOffset;
            animator.SetBool(IsMoving, moving);

            if (moving)
            {
                // targetDirection = new Vector2(direction.x, direction.y).normalized;
                targetDirection = SnapTo4Directions(direction);
                animator.SetFloat(LastMoveX, targetDirection.x);
                animator.SetFloat(LastMoveY, targetDirection.y);
                
                previousPosition = logicalPosition;
            }
        }
        
        private Vector2 SnapTo4Directions(Vector3 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            return angle switch
            {
                >= -45f and < 45f   => new Vector2(1, 0),   // Right
                >= 45f and < 135f   => new Vector2(0, 1),   // Up
                >= -135f and < -45f => new Vector2(0, -1),  // Down
                _                    => new Vector2(-1, 0)   // Left
            };
        }
        
        private void HandleAttack()
        {
            animator.SetTrigger(IsAttacking);
        }
        private void HandleDamaged()
        {
            animator.SetTrigger(IsDamaged);
        }

        
        private void HandleDeath()
        {
            isDeadAnimationPlaying = true;
            animator.SetBool(IsDead, true);
        }
        
        public void OnDeathAnimationEnd()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            if (!animator || isDeadAnimationPlaying) return;

            currentSmoothDirection = Vector2.Lerp(
                currentSmoothDirection,
                targetDirection,
                Time.deltaTime * smoothingSpeed
            );

            animator.SetFloat(moveX, currentSmoothDirection.x);
            animator.SetFloat(moveY, currentSmoothDirection.y);
        }

        private void OnDestroy()
        {
            if (model != null)
            {
                model.OnAttack -= HandleAttack;
                model.OnDamaged -= HandleDamaged;
                model.OnDied -= HandleDeath;
            }
        }

        // private void UpdateDirection(Vector3 direction)
        // {
        //     direction.Normalize();
        //     animator.SetFloat("MoveX", direction.x, 0.1f, Time.deltaTime);
        //     animator.SetFloat("MoveY", direction.y, 0.1f, Time.deltaTime);
        // }

        // private void Update()
        // {
        //     if (currentFrames == null || currentFrames.Length == 0) return;
        //     
        //     timer += Time.deltaTime;
        //
        //     if (timer >= animationData.ticksPerFrame)
        //     {
        //         timer = 0;
        //         currentFrameIndex = (currentFrameIndex + 1) % currentFrames.Length;
        //         spriteRenderer.sprite = currentFrames[currentFrameIndex];
        //         Debug.Log($"Смена кадра: {currentFrameIndex} у объекта {gameObject.name}");
        //     }
        // }
        //
        // private void UpdateDirection(Vector3 direction)
        // {
        //     direction.Normalize();
        //     var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //
        //     Sprite[] nextFrames;
        //     
        //     if (angle >= -22.5f && angle < 22.5f)
        //         nextFrames = animationData.right;
        //     else if (angle >= 22.5f && angle < 67.5f)
        //         nextFrames = animationData.upRight;
        //     else if (angle >= 67.5f && angle < 112.5f)
        //         nextFrames = animationData.up;
        //     else if (angle >= 112.5f && angle < 157.5f)
        //         nextFrames = animationData.upLeft;
        //     else if (angle >= 157.5f || angle < -157.5f)
        //         nextFrames = animationData.left;
        //     else if (angle >= -157.5f && angle < -112.5f)
        //         nextFrames = animationData.downLeft;
        //     else if (angle >= -112.5f && angle < -67.5f)
        //         nextFrames = animationData.down;
        //     else
        //         nextFrames = animationData.downRight;
        //     
        //     if (currentFrames != nextFrames)
        //     {
        //         currentFrames = nextFrames;
        //         currentFrameIndex = 0;
        //         timer = 0; 
        //         
        //         if (currentFrames != null && currentFrames.Length > 0)
        //             spriteRenderer.sprite = currentFrames[0];
        //     }
        // }
    }
}