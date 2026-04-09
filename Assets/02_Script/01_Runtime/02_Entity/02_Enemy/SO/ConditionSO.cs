using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Condition/Base")]
    public abstract class ConditionSO : ScriptableObject
    {
        public abstract bool Evaluate(EnemyContext ctx);
    }
}