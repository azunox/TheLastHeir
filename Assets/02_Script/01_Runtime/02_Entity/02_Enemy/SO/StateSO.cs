using UnityEngine;
using System.Collections.Generic;

namespace TheLastHeir.Runtime.Entity.Enemy
{
    [CreateAssetMenu(menuName = "TheLastHeir/Enemy/FSM/State")]
    public class StateSO : ScriptableObject
    {
        public List<ActionSO> enterActions;
        public List<ActionSO> tickActions;
        public List<TransitionSO> transitions;
        
        public void OnEnter(EnemyContext ctx)
        {
            // 진입 시 행동 실행
            foreach (var action in enterActions) action.Execute(ctx);
        }
        
        public void OnTick(EnemyContext ctx)
        {
            // 행동 실행
            foreach (var action in tickActions) action.Execute(ctx);
        }
        
        public virtual void OnExit(EnemyContext ctx) {}
    }
}