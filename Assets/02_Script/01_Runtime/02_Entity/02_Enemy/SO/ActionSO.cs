using UnityEngine;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/Action/Base")]
    public abstract class ActionSO : ScriptableObject
    {
        public abstract void Execute(EnemyContext ctx);
    }
}