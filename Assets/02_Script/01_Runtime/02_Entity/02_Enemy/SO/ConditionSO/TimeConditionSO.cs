using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Condition/TimeCondition")]
    public class TimeConditionSO : ConditionSO
    {
        [Header("[ 다음 공격까지 대기 시간 ]")]
        public float waitTime;

        public override bool Evaluate(EnemyContext ctx)
        {
            return ctx.stateTimer >= waitTime;
        }
    }
}