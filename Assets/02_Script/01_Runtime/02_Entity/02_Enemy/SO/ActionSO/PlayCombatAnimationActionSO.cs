using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/PlayCombatAnimation")]
    public class PlayCombatAnimationActionSO : ActionSO
    {
        public string animationName = "Attack";

        [Header("CrossFade")]
        public float crossFadeTime = 0.1f;

        [Header("Optional Rotate (no snap)")]
        public float rotateDegreesPerSecond = 0f; // 0이면 회전 안 함(Commit에서 정렬)

        [Header("Animator Cleanup")]
        public bool zeroMoveParams = true;

        public override void Execute(EnemyContext ctx)
        {
            if (ctx?.anim == null || ctx.owner == null) return;

            if (rotateDegreesPerSecond > 0f && ctx.target != null)
            {
                Vector3 toTarget = ctx.target.position - ctx.owner.position;
                toTarget.y = 0f;
                if (toTarget.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized);
                    ctx.owner.rotation = Quaternion.RotateTowards(
                        ctx.owner.rotation,
                        targetRot,
                        rotateDegreesPerSecond * Time.deltaTime
                    );
                }
            }

            ctx.anim.CrossFadeInFixedTime(animationName, crossFadeTime);

            if (zeroMoveParams)
            {
                ctx.anim.SetFloat("Vertical", 0f);
                ctx.anim.SetFloat("Horizontal", 0f);
            }
        }
    }
}