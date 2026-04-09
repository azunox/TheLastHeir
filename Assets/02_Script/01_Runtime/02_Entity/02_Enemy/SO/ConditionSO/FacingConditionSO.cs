using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Condition/FacingCondition")]
    public class FacingConditionSO : ConditionSO
    {
        [Range(0f, 180f)]
        public float maxAngle = 25f;

        public override bool Evaluate(EnemyContext ctx)
        {
            if (ctx?.owner == null || ctx.target == null) return false;

            Vector3 toTarget = ctx.target.position - ctx.owner.position;
            toTarget.y = 0f;
            if (toTarget.sqrMagnitude < 0.0001f) return true;

            float angle = Vector3.Angle(ctx.owner.forward, toTarget.normalized);
            return angle <= maxAngle;
        }
    }
}