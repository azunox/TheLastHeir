using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/MoveToTarget")]
    public class MoveToTargetActionSO : ActionSO
    {
        [Header("[ 멈출 거리(해당 거리되면 Attack 전환) ]")]
        public float StopDistance;
        public float rotationSpeed;

        public override void Execute(EnemyContext ctx)
        {
            if (ctx.target == null || ctx.agent == null || ctx.anim == null) return;
            
            ctx.agent.updateRotation = false;

            // 목적지 및 멈춤 거리 설정
            ctx.agent.SetDestination(ctx.target.position);
            ctx.agent.stoppingDistance = StopDistance;
            
            Vector3 direction = (ctx.target.position - ctx.owner.position).normalized;
            direction.y = 0;
            
            if (direction != Vector3.zero)
            {
                ctx.owner.rotation = Quaternion.Slerp(ctx.owner.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
            }
            
            // 월드 기준에서 캐릭터 로컬 기준으로 변환
            Vector3 localVelocity = ctx.owner.InverseTransformDirection(ctx.agent.velocity);
            
            float h = localVelocity.x / ctx.agent.speed;
            float v = localVelocity.z / ctx.agent.speed;
            
            ctx.anim.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
            ctx.anim.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
        }
    }
}