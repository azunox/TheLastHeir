using UnityEngine;
using UnityEngine.AI;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/SetCombatMode")]
    public class SetCombatModeActionSO : ActionSO
    {
        [Header("[ Combat Mode ]")]
        public bool combatMode = true;

        [Header("[ Exit Combat -> Navigation Sync ]")]
        public bool warpAgentToOwnerOnExit = true;
        public float warpSampleRadius = 2.0f;

        [Header("[ Animator Cleanup] ")]
        public bool zeroMoveParams = true;

        public override void Execute(EnemyContext ctx)
        {
            if (ctx == null || ctx.owner == null) return;

            // Root Motion 권한 전환
            if (ctx.anim != null)
            {
                ctx.anim.applyRootMotion = combatMode;
                if (zeroMoveParams)
                {
                    ctx.anim.SetFloat("Horizontal", 0f);
                    ctx.anim.SetFloat("Vertical", 0f);
                }
            }

            if (ctx.agent == null) return;

            if (combatMode)
            {
                // NavMesh 정지
                ctx.agent.isStopped = true;
                ctx.agent.velocity = Vector3.zero;
                ctx.agent.updatePosition = false;
                ctx.agent.updateRotation = false; 
                ctx.agent.ResetPath();
                ctx.agent.nextPosition = ctx.owner.position; // tug-of-war 방지
            }
            else
            {
                // NavMesh 재개, 위치 동기화
                ctx.agent.updatePosition = true;
                ctx.agent.updateRotation = false; 
                
                if (warpAgentToOwnerOnExit)
                {
                    Vector3 warpPos = ctx.owner.position;

                    if (!ctx.agent.isOnNavMesh)
                    {
                        if (NavMesh.SamplePosition(warpPos, out var hit, warpSampleRadius, NavMesh.AllAreas))
                            warpPos = hit.position;
                    }

                    // Warp는 isStopped 상태여도 안전하게 위치 동기화 가능
                    ctx.agent.Warp(warpPos);
                }
                else
                {
                    ctx.agent.nextPosition = ctx.owner.position;
                }

                ctx.agent.ResetPath();
                ctx.agent.isStopped = false;
            }
        }
    }
}
