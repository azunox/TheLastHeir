using UnityEngine;
using System.Collections.Generic;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Transition")]
    public class TransitionSO : ScriptableObject
    {
        public StateSO targetState;
        public List<ConditionSO> conditions;

        public enum ConditionLogic
        {
            And,
            Or
        }
        
        public ConditionLogic conditionLogic = ConditionLogic.And;

        public bool CanTransition(EnemyContext ctx)
        {
            if (conditions == null || conditions.Count == 0) return false;

            switch (conditionLogic)
            {
                case ConditionLogic.And:
                {
                    foreach (var condition in conditions)
                    {
                        if (!condition.Evaluate(ctx))
                            return false;
                    }
                    return true;
                }

                case ConditionLogic.Or:
                {
                    foreach (var condition in conditions)
                    {
                        if (condition.Evaluate(ctx))
                            return true;
                    }
                    return false;
                }
            }
            
            return false;
        }
    }
}