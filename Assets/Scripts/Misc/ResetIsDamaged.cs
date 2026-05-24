using UnityEngine;

namespace Misc
{
    public class ResetIsDamaged: StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsDamaged", false);
        }
    }
}