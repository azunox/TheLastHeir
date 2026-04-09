using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/FaceTargetClamped")]
    public class FaceTargetClampedActionSO : ActionSO
    {
        public float maxDegreesPerSecond = 180f;
        public float minAngleToRotate = 2f;

        public override void Execute(EnemyContext ctx)
        {
            if (ctx?.owner == null || ctx.target == null) return;

            Vector3 toTarget = ctx.target.position - ctx.owner.position;
            toTarget.y = 0f;
            if (toTarget.sqrMagnitude < 0.0001f) return;

            Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized);
            float angle = Quaternion.Angle(ctx.owner.rotation, targetRot);
            if (angle < minAngleToRotate) return;

            ctx.owner.rotation = Quaternion.RotateTowards(
                ctx.owner.rotation,
                targetRot,
                maxDegreesPerSecond * Time.deltaTime
            );
        }
    }
}