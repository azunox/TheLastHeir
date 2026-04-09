using UnityEngine;
using UnityEngine.AI;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/PressureMove")]
    public class PressureMoveActionSO : ActionSO
    { 
        [Header("[ 회전 속도 ]")]
        public float rotationSpeed = 10f;
        public override void Execute(EnemyContext ctx)
        {
            if (ctx.target == null) return;
            
            Vector3 dir = ctx.target.position - ctx.owner.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.01f)
            {
                ctx.owner.rotation = Quaternion.Slerp(ctx.owner.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
            }
            
            ctx.anim.SetFloat("Vertical", 0.6f, 0.15f, Time.deltaTime);
        }
    }
}