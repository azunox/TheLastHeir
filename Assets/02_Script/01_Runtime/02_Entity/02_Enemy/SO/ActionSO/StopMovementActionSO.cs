using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/StopMovement")]
    public class StopMovementActionSO : ActionSO
    {
        public override void Execute(EnemyContext ctx)
        {
            if (ctx.agent != null)
            {
                ctx.agent.isStopped = true;
                ctx.agent.velocity = Vector3.zero;
                ctx.agent.ResetPath();
            }
        }
    }
}