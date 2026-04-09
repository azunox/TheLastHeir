using UnityEngine;

namespace TheLastHeir.Runtime.Entity
{
    public class AttackResetHandler : StateMachineBehaviour
    {
        [SerializeField] private string triggerName;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Trigger Reset");
            animator.ResetTrigger(triggerName);
        }
    }    
}
