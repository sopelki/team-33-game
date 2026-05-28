using UnityEngine;

namespace Misc
{
    public class ResetIsDamaged : StateMachineBehaviour
    {
        private static readonly int isDamaged = Animator.StringToHash("IsDamaged");

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isDamaged, false);
        }
    }
}