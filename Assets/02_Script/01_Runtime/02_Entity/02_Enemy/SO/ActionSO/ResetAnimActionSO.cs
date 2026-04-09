using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/Reset")]
    public class ResetAnimActionSO : ActionSO
    {
        public override void Execute(EnemyContext ctx)
        {
            ctx.anim.SetFloat("Vertical", 0f);
            ctx.anim.SetFloat("Horizontal", 0f);
        }
    }
}